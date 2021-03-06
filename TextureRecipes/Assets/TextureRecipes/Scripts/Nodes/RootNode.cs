﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace TextureRecipes
{
    [Serializable]
    public class RootNode : BaseNode, ISerializationCallbackReceiver
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

            if (inputs.Count == 0)
            { 
                inputs.Add(new NodeInput());
            }

            nodeName = "Root";
        }

        //Hide the constructor since this is a ScriptableObject and should be instantiated using CreateInstance
        protected RootNode()
        {
        }

        public override void setParameters(Material mat)
        {
            setInputParameters(mat);
        }
    }
}
