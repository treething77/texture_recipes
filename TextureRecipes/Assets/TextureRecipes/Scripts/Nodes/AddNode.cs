using System;
using System.Collections.Generic;
using UnityEngine;

namespace TextureRecipes
{
    [Serializable]
    public class AddNode : BaseNode, ISerializationCallbackReceiver
    {
        public float InputOneWeight;
        public float InputTwoWeight;

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            if (null == inputs)
            {
                inputs = new List<NodeInput>();
            }

            if (inputs.Count != 2)
            {
                inputs.Clear();
                inputs.Add(new NodeInput());
                inputs.Add(new NodeInput());

                InputOneWeight = 1.0f;
                InputTwoWeight = 1.0f;
            }
        }

        public override void setParameters(Material mat)
        {
            mat.SetFloat("weightOne" + getNodeID(), InputOneWeight);
            mat.SetFloat("weightTwo" + getNodeID(), InputTwoWeight);
            base.setParameters(mat);
        }

        //Hide the constructor since this is a ScriptableObject and should be instantiated using CreateInstance
        protected AddNode()
        {
        }
    }
}
