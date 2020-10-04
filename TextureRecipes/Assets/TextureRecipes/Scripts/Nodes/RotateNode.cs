using System;
using System.Collections.Generic;
using UnityEngine;

namespace TextureRecipes
{
    [Serializable]
    public class RotateNode : BaseNode, ISerializationCallbackReceiver
    {
        public float rotation;
        public float centerU;
        public float centerV;

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

                centerU = 0.5f;
                centerV = 0.5f;
            }
        }

        public override void setParameters(Material mat)
        {
            mat.SetFloat("rotateAmount" + getNodeID(), Mathf.Deg2Rad * rotation);
            mat.SetFloat("rotateCenterU" + getNodeID(), centerU);
            mat.SetFloat("rotateCenterV" + getNodeID(), centerV);
            base.setParameters(mat);
        }

        //Hide the constructor since this is a ScriptableObject and should be instantiated using CreateInstance
        protected RotateNode()
        {
        }
    }
}
