using System;
using System.Collections.Generic;
using UnityEngine;

namespace TextureRecipes
{
    [Serializable]
    public class CompositeNode : BaseNode, ISerializationCallbackReceiver
    {
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
            }
        }

        //Hide the constructor since this is a ScriptableObject and should be instantiated using CreateInstance
        protected CompositeNode()
        {
        }
    }
}
