using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TextureRecipes
{
    public abstract class BaseNodeRenderer
    {
        public class NodeDrawState
        {
            public BaseNode SelectedNode;
            public ShaderLayer layerObject;

            public Vector2 menuPos;
            public bool drawCurve;
            public Vector2 curveStart;
            public Vector2 screenPos;

            public Texture inputConnectionTexture;
            public Texture outputConnectionTexture;
            public Material uiMat;

            public struct WindowData
            {
                public Vector2 pos;
            }
            public List<WindowData> windowData = new List<WindowData>();

            public struct ConnectionInfo
            {
                public int connectionIndex;
                public bool input;
                public int nodeIndex;
            }
            public ConnectionInfo connectionInfo = new ConnectionInfo();
            
            public Vector2 guiOffset;
            public Vector2 scrollStart;
            public Vector2 scrollOffset;
            public bool scrolling;
            public bool connectivityChange;
        }

        public abstract Rect getNodeSize(BaseNode node);
        public abstract void drawNode(BaseNode node, int nodeId, NodeDrawState drawState);
    }
}
