using System;
using System.Collections.Generic;
using UnityEngine;

namespace TextureRecipes
{
    [Serializable]
    public class ScaleNode : BaseNode, ISerializationCallbackReceiver
    {
        public float scaleU;
        public float scaleV;

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            if (null == inputs)
            {
                inputs = new List<NodeInput>();
            }

            if (inputs.Count != 1)
            {
                scaleU = 1.0f;
                scaleV = 1.0f;

                inputs.Clear();
                inputs.Add(new NodeInput());
            }
        }

        public override void setParameters(Material mat)
        {
            mat.SetFloat("scaleU" + getNodeID(), scaleU);
            mat.SetFloat("scaleV" + getNodeID(), scaleV);
            base.setParameters(mat);
        }

        //Hide the constructor since this is a ScriptableObject and should be instantiated using CreateInstance
        protected ScaleNode()
        {
        }
    }
}
