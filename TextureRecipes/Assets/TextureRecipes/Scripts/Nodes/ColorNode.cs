using System;
using UnityEngine;

namespace TextureRecipes
{
    [Serializable]
    public class ColorNode : BaseNode
    {
        [SerializeField] public Color color;
 
        //Hide the constructor since this is a ScriptableObject and should be instantiated using CreateInstance
        protected ColorNode()
        {
        }

        public override void setParameters(Material mat)
        {
            mat.SetColor("color" + getNodeID(), color);
            base.setParameters(mat);
        }
    }
}
