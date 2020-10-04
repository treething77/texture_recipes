using System;
using System.Collections.Generic;
using UnityEngine;

namespace TextureRecipes
{
    [Serializable]
    public class SplitNode : BaseNode, ISerializationCallbackReceiver
    {
        public bool ReplicateToAllChannels;

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

            outputCount = 4;
        }

        public override void setParameters(Material mat)
        {
            base.setParameters(mat);
        }

        //Hide the constructor since this is a ScriptableObject and should be instantiated using CreateInstance
        protected SplitNode()
        {
        }
    }
}
