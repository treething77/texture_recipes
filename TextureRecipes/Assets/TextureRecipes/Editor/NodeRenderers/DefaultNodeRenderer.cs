using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TextureRecipes
{
    public class DefaultNodeRenderer : BaseNodeRenderer
    {
        protected const int NodeWindowWidth = 100;
        protected const int NodeWindowHeight = 100;
        protected const int NodeWindowLabelHeight = 20;
        protected const int NodeWindowSizeBuffer = 5;
        protected const int NodeWindowWidthTotal = NodeWindowWidth + (2 * NodeWindowSizeBuffer);
        protected const int NodeWindowHeightTotal = NodeWindowHeight + (2 * NodeWindowSizeBuffer);
        protected const int NodeWindowOutputHeight = 20;

        protected const float SelectedNodeAlpha = 0.8f;
        protected const float NodeAlpha = 0.2f;

        static protected readonly Vector2 CurveStartOffset = new Vector2(5, 5);
        static protected readonly Vector2 InputStartOffset = new Vector2(0, 30);
        static protected readonly Vector2 InputIncrementOffset = new Vector2(0, 16);
        static protected readonly Vector2 OutputStartOffset = new Vector2(-20, 30);
        static protected readonly Vector2 ConnectorSize = new Vector2(20, 13);

        public DefaultNodeRenderer()
        {
        }

        public override Rect getNodeSize(BaseNode node)
        {
            Rect nodeSize = new Rect(0.0f, 0.0f, NodeWindowWidthTotal, InputStartOffset.y + (Math.Max(node.outputCount* NodeWindowOutputHeight, node.inputs.Count * InputIncrementOffset.y)));
            return nodeSize;
        }

        protected virtual void drawNodeLabel(BaseNode node, int nodeId, NodeDrawState drawState)
        {
            if ((node != drawState.SelectedNode) || (node is RootNode))
            {
                EditorGUI.LabelField(new Rect(new Vector2(NodeWindowSizeBuffer, 0),
                                              new Vector2(NodeWindowWidth, NodeWindowLabelHeight)), 
                                              node.nodeName);
            }
        }

        protected virtual void handleNodeRepaint(BaseNode node, int nodeId, NodeDrawState drawState)
        {
            drawNodeBackground(node, nodeId, drawState);
            drawNodeInputs(node, nodeId, drawState);
            drawNodeOutputs(node, nodeId, drawState);
        }

        protected void drawNodeOutputs(BaseNode node, int nodeId, NodeDrawState drawState)
        {
            if (!(node is RootNode))
            {
                var outputPos = new Vector2(getNodeSize(node).width, 0) + OutputStartOffset;
                var rect = new Rect(outputPos, ConnectorSize);
                EditorGUI.DrawPreviewTexture(rect, drawState.outputConnectionTexture, drawState.uiMat);
            }
        }

        protected void drawNodeInputs(BaseNode node, int nodeId, NodeDrawState drawState)
        {
            for (int i = 0; i < node.inputs.Count; i++)
            {
                var inputPos = InputStartOffset + (InputIncrementOffset * i);
                var rect = new Rect(inputPos, ConnectorSize);
                EditorGUI.DrawPreviewTexture(rect, drawState.inputConnectionTexture, drawState.uiMat);
            }
        }

        protected void drawNodeBackground(BaseNode node, int nodeId, NodeDrawState drawState)
        {
            EditorGUI.DrawRect(new Rect(NodeWindowSizeBuffer, NodeWindowLabelHeight, NodeWindowWidth, NodeWindowHeight), 
                               getNodeColor(node, nodeId, drawState));
        }

        protected Color getNodeColor(BaseNode node, int nodeId, NodeDrawState drawState)
        {
            Color nodeColor = Color.gray;
            if (node is RootNode)
            {
                nodeColor = Color.blue;
            }
            if (node == drawState.SelectedNode)
            {
                nodeColor.a = SelectedNodeAlpha;
            }
            else
            {
                nodeColor.a = NodeAlpha;
            }
            return nodeColor;
        }

        protected virtual void handleNodeMouseDown(BaseNode node, int nodeId, NodeDrawState drawState)
        {
            for (int i = 0; i < node.inputs.Count; i++)
            {
                var inputPos = InputStartOffset + (InputIncrementOffset * i);
                var rect = new Rect(inputPos, ConnectorSize);

                if (rect.Contains(drawState.screenPos))
                {
                    if (!drawState.drawCurve)
                    {
                        drawState.drawCurve = true;
                        NodeDrawState.WindowData d = drawState.windowData[nodeId];
                        drawState.curveStart = d.pos + Event.current.mousePosition;
                        drawState.connectionInfo.input = true;
                        drawState.connectionInfo.nodeIndex = nodeId;
                        drawState.connectionInfo.connectionIndex = i;
                        Event.current.Use();
                    }
                }
            }

            if (!(node is RootNode))
            {
                for (int i = 0; i < node.outputCount; i++)
                {
                    var outputPos = new Vector2(getNodeSize(node).width, 0) + OutputStartOffset + (InputIncrementOffset * i);
                    var rect = new Rect(outputPos, ConnectorSize);

                    if (rect.Contains(drawState.screenPos))
                    {
                        if (!drawState.drawCurve)
                        {
                            drawState.drawCurve = true;
                            NodeDrawState.WindowData d = drawState.windowData[nodeId];
                            drawState.curveStart = d.pos + Event.current.mousePosition;
                            drawState.connectionInfo.input = false;
                            drawState.connectionInfo.nodeIndex = nodeId;
                            drawState.connectionInfo.connectionIndex = i;
                            Event.current.Use();
                        }
                    }
                }
            }
        }

        protected virtual void handleNodeMouseUp(BaseNode node, int nodeId, NodeDrawState drawState)
        {
            for (int i = 0; i < node.inputs.Count; i++)
            {
                var inputPos = InputStartOffset + (InputIncrementOffset * i);
                var rect = new Rect(inputPos, ConnectorSize);

                if (rect.Contains(drawState.screenPos))
                {
                    if (Event.current.type == EventType.MouseUp)
                    {
                        drawState.drawCurve = false;
                        //connect?
                        if (!drawState.connectionInfo.input)
                        {
                            if (drawState.connectionInfo.nodeIndex != nodeId)
                            {
                                //output to input
                                var inputNode = drawState.layerObject.nodes[nodeId];
                                var outputNode = drawState.layerObject.nodes[drawState.connectionInfo.nodeIndex];
                                var nodeInput = inputNode.inputs[i];
                                nodeInput.inputNode = outputNode;
                                nodeInput.outputIndex = drawState.connectionInfo.connectionIndex;

                                inputNode.inputs[i] = nodeInput;
                                Event.current.Use();

                                drawState.connectivityChange = true;
                            }
                        }
                    }
                }
            }

            if (!(node is RootNode))
            {
                var outputPos = new Vector2(getNodeSize(node).width, 0) + OutputStartOffset;
                var rect = new Rect(outputPos, ConnectorSize);

                if (rect.Contains(drawState.screenPos))
                {
                    if (Event.current.type == EventType.MouseUp)
                    {
                        drawState.drawCurve = false;
                        //connect?
                        if (drawState.connectionInfo.nodeIndex != nodeId)
                        {
                            //input to output
                            var outputNode = drawState.layerObject.nodes[nodeId];
                            var inputNode = drawState.layerObject.nodes[drawState.connectionInfo.nodeIndex];
                            var nodeInput = inputNode.inputs[drawState.connectionInfo.connectionIndex];
                            nodeInput.inputNode = outputNode;
                            nodeInput.outputIndex = drawState.connectionInfo.connectionIndex;

                            inputNode.inputs[drawState.connectionInfo.connectionIndex] = nodeInput;
                            Event.current.Use();

                            drawState.connectivityChange = true;
                        }
                    }
                }
            }
        }

        internal static string DrawLabelEdit(BaseNode selectedNode, NodeDrawState drawState, string labelValue)
        {
            return EditorGUI.TextField(new Rect(
                                        new Vector2(NodeWindowSizeBuffer, 0) + 
                                            selectedNode.nodePosition + drawState.guiOffset + drawState.scrollOffset,
                                        new Vector2(NodeWindowWidth, NodeWindowLabelHeight)), 
                                        labelValue);
        }

        public override void drawNode(BaseNode node, int nodeId, NodeDrawState drawState)
        {
            drawNodeLabel(node, nodeId, drawState);
            
            switch (Event.current.type)
            {
                case EventType.Repaint:
                    {
                        handleNodeRepaint(node, nodeId, drawState);
                        break;
                    }
                case EventType.MouseDown:
                    {
                        handleNodeMouseDown(node, nodeId, drawState);
                        break;
                    }
                case EventType.MouseUp:
                    {
                        handleNodeMouseUp(node, nodeId, drawState);
                        break;
                    }
            }
        }
    }
}
