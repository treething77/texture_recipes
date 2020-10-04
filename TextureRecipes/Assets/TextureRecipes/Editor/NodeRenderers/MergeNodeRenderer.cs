using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TextureRecipes
{
    public class MergeNodeRenderer : DefaultNodeRenderer
    {
        Texture input_r;
        Texture input_g;
        Texture input_b;
        Texture input_a;

        public MergeNodeRenderer()
        {
            input_r = AssetDatabase.LoadAssetAtPath<Texture>("Assets/TextureRecipes/Editor/Icons/input_r.png");
            input_g = AssetDatabase.LoadAssetAtPath<Texture>("Assets/TextureRecipes/Editor/Icons/input_g.png");
            input_b = AssetDatabase.LoadAssetAtPath<Texture>("Assets/TextureRecipes/Editor/Icons/input_b.png");
            input_a = AssetDatabase.LoadAssetAtPath<Texture>("Assets/TextureRecipes/Editor/Icons/input_a.png");
        }

        public override void drawNode(BaseNode node, int nodeId, NodeDrawState drawState)
        {
            drawNodeLabel(node, nodeId, drawState);

            switch (Event.current.type)
            {
                case EventType.Repaint:
                    {
                        drawNodeBackground(node, nodeId, drawState);

                        var inputPos = InputStartOffset;
                        var rect = new Rect(inputPos, ConnectorSize);
                        EditorGUI.DrawPreviewTexture(rect, input_r, drawState.uiMat);

                        inputPos += InputIncrementOffset;
                        rect = new Rect(inputPos, ConnectorSize);
                        EditorGUI.DrawPreviewTexture(rect, input_g, drawState.uiMat);

                        inputPos += InputIncrementOffset;
                        rect = new Rect(inputPos, ConnectorSize);
                        EditorGUI.DrawPreviewTexture(rect, input_b, drawState.uiMat);

                        inputPos += InputIncrementOffset;
                        rect = new Rect(inputPos, ConnectorSize);
                        EditorGUI.DrawPreviewTexture(rect, input_a, drawState.uiMat);

                        drawNodeOutputs(node, nodeId, drawState);
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
