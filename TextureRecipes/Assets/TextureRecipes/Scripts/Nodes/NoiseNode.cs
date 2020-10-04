using System;
using UnityEngine;

namespace TextureRecipes
{
    [Serializable]
    public class NoiseNode : BaseNode
    {
        public float noiseScale = 1.0f;

        public override void setParameters(Material mat)
        {
            mat.SetFloat("noiseScale" + getNodeID(), noiseScale);
            base.setParameters(mat);
        }

        //Hide the constructor since this is a ScriptableObject and should be instantiated using CreateInstance
        protected NoiseNode()
        {
        }
    }
}
