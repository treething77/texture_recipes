using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TextureRecipes
{
    public class SplitNodeRenderer : DefaultNodeRenderer
    {
        Texture output_r;
        Texture output_g;
        Texture output_b;
        Texture output_a;

        public SplitNodeRenderer()
        {
            output_r = AssetDatabase.LoadAssetAtPath<Texture>("Assets/TextureRecipes/Editor/Icons/output_r.png");
            output_g = AssetDatabase.LoadAssetAtPath<Texture>("Assets/TextureRecipes/Editor/Icons/output_g.png");
            output_b = AssetDatabase.LoadAssetAtPath<Texture>("Assets/TextureRecipes/Editor/Icons/output_b.png");
            output_a = AssetDatabase.LoadAssetAtPath<Texture>("Assets/TextureRecipes/Editor/Icons/output_a.png");
        }

        public override void drawNode(BaseNode node, int nodeId, NodeDrawState drawState)
        {
            drawNodeLabel(node, nodeId, drawState);

            switch (Event.current.type)
            {
                case EventType.Repaint:
                    {
                        drawNodeBackground(node, nodeId, drawState);
                        drawNodeInputs(node, nodeId, drawState);

                        if (!(node is RootNode))
                        {
                            var outputPos = new Vector2(getNodeSize(node).width, 0) + OutputStartOffset;
                            var rect = new Rect(outputPos, ConnectorSize);
                            EditorGUI.DrawPreviewTexture(rect, output_r, drawState.uiMat);

                            outputPos += new Vector2(0, NodeWindowOutputHeight);
                            rect = new Rect(outputPos, ConnectorSize);
                            EditorGUI.DrawPreviewTexture(rect, output_g, drawState.uiMat);

                            outputPos += new Vector2(0, NodeWindowOutputHeight);
                            rect = new Rect(outputPos, ConnectorSize);
                            EditorGUI.DrawPreviewTexture(rect, output_b, drawState.uiMat);

                            outputPos += new Vector2(0, NodeWindowOutputHeight);
                            rect = new Rect(outputPos, ConnectorSize);
                            EditorGUI.DrawPreviewTexture(rect, output_a, drawState.uiMat);
                        }
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
