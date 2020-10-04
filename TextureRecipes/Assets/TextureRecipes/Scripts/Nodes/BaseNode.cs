using System;
using System.Collections.Generic;
using UnityEngine;

namespace TextureRecipes
{
    [Serializable]
    public abstract class BaseNode : ScriptableObject
    {
        [Serializable]
        public struct NodeInput
        {
            public BaseNode inputNode;
            public int outputIndex;
        }
        [HideInInspector] public Vector2 nodePosition;
        [HideInInspector] public List<NodeInput> inputs;
        [HideInInspector] public int nodeID;
        [HideInInspector] public string nodeName;
        [HideInInspector] public bool assetsDirty;//set on modifications in inspector, clear when regen assets

        [HideInInspector] public int outputCount = 1;

        public void OnEnable()
        {
            hideFlags = HideFlags.HideInHierarchy;
            if (null == inputs)
            {
                inputs = new List<NodeInput>();
            }
            if (string.IsNullOrEmpty(nodeName))
            {
               nodeName = Utilities.MakeNiceName( GetType().Name );
            }
        }

        //Hide the constructor since this is a ScriptableObject and should be instantiated using CreateInstance
        protected BaseNode()
        {
        }

        public virtual void setParameters(Material mat)
        {
            setInputParameters(mat);
        }

        protected void setInputParameters(Material mat)
        {
            int numInputs = inputs.Count;
            for (int i = 0; i < numInputs; i++)
            {
                var input = inputs[i];
                if (null != input.inputNode)
                {
                    input.inputNode.setParameters(mat);
                }
            }
        }

        public string getNodeID()
        {
            return nodeID.ToString();
        }
    }
}