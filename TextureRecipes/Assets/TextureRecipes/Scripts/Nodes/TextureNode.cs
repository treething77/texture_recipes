using System;
using UnityEngine;

namespace TextureRecipes
{
    [Serializable]
    public class TextureNode : BaseNode
    {
        public Texture2D Texture;

        //Hide the constructor since this is a ScriptableObject and should be instantiated using CreateInstance
        protected TextureNode()
        {
        }

        public override void setParameters(Material mat)
        {
            mat.SetTexture("texture" + getNodeID(), Texture);
            base.setParameters(mat);
        }
    }
}
