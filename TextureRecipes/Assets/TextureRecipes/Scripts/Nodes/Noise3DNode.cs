using System;
using UnityEngine;

namespace TextureRecipes
{
    [Serializable]
    public class Noise3DNode : BaseNode
    {
        public float noiseScale = 1.0f;
        public float noiseZ;

        public override void setParameters(Material mat)
        {
            mat.SetFloat("noiseScale" + getNodeID(), noiseScale);
            mat.SetFloat("noiseZ" + getNodeID(), noiseZ);
            base.setParameters(mat);
        }

        //Hide the constructor since this is a ScriptableObject and should be instantiated using CreateInstance
        protected Noise3DNode()
        {
        }
    }
}
