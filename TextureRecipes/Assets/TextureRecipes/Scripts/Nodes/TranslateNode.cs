using System;
using System.Collections.Generic;
using UnityEngine;

namespace TextureRecipes
{
    [Serializable]
    public class TranslateNode : BaseNode, ISerializationCallbackReceiver
    {
        public float translateU;
        public float translateV;

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
            mat.SetFloat("translateU" + getNodeID(), translateU);
            mat.SetFloat("translateV" + getNodeID(), translateV);
            base.setParameters(mat);
        }

        //Hide the constructor since this is a ScriptableObject and should be instantiated using CreateInstance
        protected TranslateNode()
        {
        }
    }
}
