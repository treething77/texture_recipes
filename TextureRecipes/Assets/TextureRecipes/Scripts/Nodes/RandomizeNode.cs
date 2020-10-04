using System;
using System.Collections.Generic;
using UnityEngine;

namespace TextureRecipes
{
    [Serializable]
    public class RandomizeNode : BaseNode, ISerializationCallbackReceiver
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

            if (inputs.Count != 4)
            {
                inputs.Clear();
                inputs.Add(new NodeInput());
                inputs.Add(new NodeInput());
                inputs.Add(new NodeInput());
                inputs.Add(new NodeInput());
            }
        }
        public override void setParameters(Material mat)
        {
            int inputCount = 0;
            for (int i = 0; i < inputs.Count; i++)
            {
                if (null != inputs[i].inputNode)
                {
                    inputCount++;
                }
            }

            int select = UnityEngine.Random.Range(0, inputCount);
            mat.SetInt("select" + getNodeID(), select);
            base.setParameters(mat);
        }

        //Hide the constructor since this is a ScriptableObject and should be instantiated using CreateInstance
        protected RandomizeNode()
        {
        }
    }
}
