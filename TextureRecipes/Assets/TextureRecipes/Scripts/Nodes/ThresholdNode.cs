using System;
using System.Collections.Generic;
using UnityEngine;

namespace TextureRecipes
{
    [Serializable]
    public class ThresholdNode : BaseNode, ISerializationCallbackReceiver
    {
        public float threshold;

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
                inputs.Clear();
                inputs.Add(new NodeInput());
            }
        }

        public override void setParameters(Material mat)
        {
            mat.SetFloat("threshold" + getNodeID(),  threshold);
            base.setParameters(mat);
        }

        //Hide the constructor since this is a ScriptableObject and should be instantiated using CreateInstance
        protected ThresholdNode()
        {
        }
    }
}
