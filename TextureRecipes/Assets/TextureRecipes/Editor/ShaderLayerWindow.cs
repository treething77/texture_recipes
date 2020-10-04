using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace TextureRecipes
{
    public class ShaderLayerWindow : EditorWindow
    {
        public TextureRecipe recipe { set; private get; }
        public BaseNode SelectedNode { get; private set; }

        public ShaderLayer layerObject;
        public bool refreshShader;

        const float NodeActiveAlpha = 1.0f;
        const float NodeInactiveAlpha = 0.5f;

        BaseNodeRenderer.NodeDrawState drawState = new BaseNodeRenderer.NodeDrawState();

        public void OnEnable()
        {
            autoRepaintOnSceneChange = true;
            wantsMouseMove = true;

            drawState.inputConnectionTexture = AssetDatabase.LoadAssetAtPath<Texture>("Assets/TextureRecipes/Editor/Icons/input.png");
            drawState.outputConnectionTexture = AssetDatabase.LoadAssetAtPath<Texture>("Assets/TextureRecipes/Editor/Icons/output.png");
            drawState.uiMat = (Material)EditorGUIUtility.Load("UI.mat");

            RendererFactory.createMap();
        }

        void NodeCreateCallback(object obj)
        {
            Type objectType = (Type)obj;

            Undo.RecordObject(layerObject, "Create " + objectType.Name);
            {
                var nodeObject = (BaseNode)CreateInstance(objectType);

                nodeObject.nodeID = layerObject.nodeGenID++;
                nodeObject.nodePosition = drawState.menuPos - drawState.guiOffset - drawState.scrollOffset;
                nodeObject.nodeName = objectType.Name + nodeObject.getNodeID();

                layerObject.nodes.Add(nodeObject);

                AssetDatabase.AddObjectToAsset(nodeObject, layerObject);
                AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(nodeObject));
            }
            Undo.FlushUndoRecordObjects();
        }

        //TODO: this doesn't always work correctly
        void OnFocus()
        {
            if (null != recipe)
            {
                if (Selection.activeObject != recipe)
                {
                    ShaderLayerEditor.nodeEditor = this;
                    Selection.activeObject = recipe;
                }
            }
        }

        void OnGUI()
        {
            if (EditorApplication.isPlaying)
            {
                return;
            }
            if (!layerObject)
            {
                return;
            }
            layerObject.getRoot();//trigger lazy creation of the root object
            drawState.layerObject = layerObject;

            switch (Event.current.type)
            {
                case EventType.KeyDown:
                    {
                        if (Event.current.keyCode == KeyCode.Delete)
                        {
                            if ((null != SelectedNode) && !(SelectedNode is RootNode))
                            {
                                Undo.RecordObject(layerObject, "Delete Node");
                                {
                                    layerObject.nodes.Remove(SelectedNode);

                                    foreach(var node in layerObject.nodes)
                                    {
                                        for(int i=0;i<node.inputs.Count;i++)
                                        {
                                            var nodeInput = node.inputs[i];
                                            if (nodeInput.inputNode == SelectedNode)
                                            {
                                                node.inputs.RemoveAt(i);
                                                i--;
                                            }                                                    
                                        }
                                    }
                                }
                                Undo.FlushUndoRecordObjects();

                                SelectedNode = null;
                                Event.current.Use();
                            }
                        }
                        break;
                    }
                case EventType.MouseDown:
                    {
                        if (Event.current.button == 2)//Middle mouse button
                        {
                            drawState.scrollStart = Event.current.mousePosition;
                            drawState.scrolling = true;
                            drawState.scrollOffset = Vector2.zero;
                            Event.current.Use();
                        }
                        break;
                    }
                case EventType.MouseUp:
                    {
                        if (Event.current.button == 2)//Middle mouse button
                        {
                            drawState.scrolling = false;
                            drawState.guiOffset += drawState.scrollOffset;
                            drawState.scrollOffset = Vector2.zero;
                            Event.current.Use();
                        }
                        break;
                    }
                case EventType.MouseDrag:
                    {
                        if (drawState.scrolling)
                        {
                            drawState.scrollOffset = Event.current.mousePosition - drawState.scrollStart;
                        }
                        break;
                    }
                case EventType.ContextClick:
                    {
                        drawState.menuPos = Event.current.mousePosition;
                        GenericMenu menu = new GenericMenu();

                        foreach (Type type in Assembly.GetAssembly(typeof(RecipeLayerBase)).GetTypes()
                            .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(BaseNode))))
                        {
                            if (type != typeof(RootNode))
                            {
                                menu.AddItem(new GUIContent(Utilities.MakeNiceName(type.Name)), false, NodeCreateCallback, type);
                            }
                        }

                        menu.ShowAsContext();
                        Event.current.Use();
                        break;
                    }
            }

            if (null != SelectedNode)
            {
                if (!(SelectedNode is RootNode))
                {
                    SerializedObject nodeObject = new SerializedObject(SelectedNode);
                    var nameProp = nodeObject.FindProperty("nodeName");
                    nameProp.stringValue = DefaultNodeRenderer.DrawLabelEdit(SelectedNode, drawState, nameProp.stringValue);

                    nodeObject.ApplyModifiedProperties();
                }
            }

            {
                BeginWindows();

                if (Event.current.type == EventType.Layout)
                {
                    drawState.windowData.Clear();
                }
                int id = 0;
                Event currentEvent = Event.current;
                drawState.menuPos = currentEvent.mousePosition;

                foreach (var node in layerObject.nodes)
                {
                    BaseNodeRenderer.NodeDrawState.WindowData d = new BaseNodeRenderer.NodeDrawState.WindowData();
                    if (Event.current.type == EventType.Layout)
                    {
                        d.pos = node.nodePosition + drawState.guiOffset + drawState.scrollOffset;
                        drawState.windowData.Add(d);
                    }
                    else
                    {
                        d = drawState.windowData[id];
                    }

                    //update the window positions iff we are currently scrolling the view
                    if (drawState.scrolling && Event.current.type == EventType.MouseDrag)
                    { 
                        d.pos = node.nodePosition + drawState.guiOffset + drawState.scrollOffset;
                        drawState.windowData[id] = d;
                    }

                    BaseNodeRenderer nodeRenderer = RendererFactory.getRenderer(node);

                    Rect windowRect = new Rect(d.pos, nodeRenderer.getNodeSize(node).size);
                    SerializedObject nodeObject = new SerializedObject(node);
                    var posProp = nodeObject.FindProperty("nodePosition");

                    windowRect = GUI.Window(id, windowRect, DrawNodeWindow, "", GUIStyle.none);

                    posProp.vector2Value = windowRect.position - drawState.guiOffset - drawState.scrollOffset;

                    nodeObject.ApplyModifiedProperties();

                    id++;
                }

                EndWindows();//triggers the actual drawing of the windows, anything drawn after this will be drawn above all the windows
            }

            if (drawState.connectivityChange)
            {
                refreshShader = true;
                drawState.connectivityChange = false;
            }                  

            switch (Event.current.type)
            {
                case EventType.MouseUp:
                    {
                        drawState.drawCurve = false;
                        break;
                    }
                case EventType.Repaint:
                    {
                        foreach (var node in layerObject.nodes)
                        {
                            BaseNodeRenderer nodeRenderer = RendererFactory.getRenderer(node);
                            var nodeSize = nodeRenderer.getNodeSize(node);

                            int i = 0;
                            foreach (var input in node.inputs)
                            {
                                if (null != input.inputNode)
                                {
                                    var inputPos = node.nodePosition + new Vector2(0, (i * 16) + 35) + drawState.guiOffset + drawState.scrollOffset;
                                    var endPos = input.inputNode.nodePosition + new Vector2(nodeSize.width, 35 + (20* input.outputIndex)) + drawState.guiOffset + drawState.scrollOffset;

                                    DrawNodeCurve(inputPos, endPos, Color.cyan);
                                }
                                i++;
                            }
                        }
                        if (drawState.drawCurve)
                        {
                            DrawNodeCurve(drawState.curveStart, Event.current.mousePosition, Color.red);
                        }
                        break;
                    }
            }
        }

        void DrawNodeWindow(int id)
        {
            drawState.screenPos = Event.current.mousePosition;

            if (id >= layerObject.nodes.Count)
            {
                return;
            }
            var node = layerObject.nodes[id];
            if (Event.current.type == EventType.MouseDown)
            {
                SelectedNode = node;
            }
            drawState.SelectedNode = SelectedNode;
            BaseNodeRenderer nodeRenderer = RendererFactory.getRenderer(node);
            nodeRenderer.drawNode(node, id, drawState);

            if (!drawState.drawCurve)
            {
                GUI.DragWindow();
            }
        }

        void DrawNodeCurve(Vector2 start, Vector2 end, Color lineColor)
        {
            Vector3 startPos = new Vector3(start.x, start.y, 0);
            Vector3 endPos = new Vector3(end.x, end.y, 0);
            Vector3 dPos = endPos - startPos;
            float handleSize = 2.0f;

            if (dPos.sqrMagnitude > 0.0f)
            {
                float tanLen = dPos.magnitude;
                if (tanLen > 60.0f) tanLen = 60.0f;
                Vector3 startTan = startPos + Vector3.left * tanLen;
                Vector3 endTan = endPos - dPos.normalized * tanLen;

                Handles.DrawBezier(startPos, endPos, startTan, endTan, lineColor, null, 2);
            }
            EditorGUI.DrawRect(new Rect(start - new Vector2(handleSize, handleSize), new Vector2(handleSize*2.0f, handleSize*2.0f)), Color.blue);
            EditorGUI.DrawRect(new Rect(end - new Vector2(handleSize, handleSize), new Vector2(handleSize * 2.0f, handleSize * 2.0f)), Color.blue);
        }
    }
}

