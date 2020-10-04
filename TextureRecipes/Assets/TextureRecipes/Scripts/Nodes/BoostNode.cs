using System;
using System.Collections.Generic;
using UnityEngine;

namespace TextureRecipes
{
    [Serializable]
    public class BoostNode : BaseNode, ISerializationCallbackReceiver
    {
        public float Multiplier;

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

                Multiplier = 1.0f;
            }
        }

        public override void setParameters(Material mat)
        {
            mat.SetFloat("multiplier" + getNodeID(), Multiplier);
            base.setParameters(mat);
        }

        //Hide the constructor since this is a ScriptableObject and should be instantiated using CreateInstance
        protected BoostNode()
        {
        }
    }
}
