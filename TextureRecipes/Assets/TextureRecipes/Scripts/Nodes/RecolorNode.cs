using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace TextureRecipes
{
    [Serializable]
    public class RecolorNode : BaseNode, ISerializationCallbackReceiver
    {
        [Serializable]
        public struct ColorMap
        {
            public Color InColor;
            public Color OutColor;
            public AnimationCurve AlphaCurve;
        }
        public bool dirty;
        public List<ColorMap> colorRemapping;
        public bool InterpolateSliceSamples;
        [HideInInspector] public Texture2D mapColorTexture;

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

                colorRemapping = new List<ColorMap>();
            }

            //ensure all colors have alpha of 1.0
            for(int i=0;i<colorRemapping.Count;i++)
            {
                var mapping = colorRemapping[i];
                mapping.InColor.a = 1.0f;
                mapping.OutColor.a = 1.0f;
                colorRemapping[i] = mapping;
            }
        }

        public override void setParameters(Material mat)
        {
            mat.SetTexture("mapTexture" + getNodeID(), mapColorTexture);
            base.setParameters(mat);
        }

        //Hide the constructor since this is a ScriptableObject and should be instantiated using CreateInstance
        protected RecolorNode()
        {
        }
    }
}
