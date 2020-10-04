using System;
using System.Collections.Generic;
using UnityEngine;

namespace TextureRecipes
{
    [Serializable] 
    public class TextureRecipe : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField]
        public List<RecipeLayerBase> layerList;

        public bool autoRefresh = true;

        public int TextureWidth;
        public int TextureHeight;

        //We set the layer for everything we render, then set the culling mask on the camera so we should only see our own objects
        public int RenderLayer = 30;

        [SerializeField]
        public string unique_id;

        public void OnEnable()
        {
            if (null == layerList)
            {
                layerList = new List<RecipeLayerBase>();
                TextureWidth = 256;
                TextureHeight = 256;
            }
            
        }

        public void OnBeforeSerialize()
        {
            if (unique_id == "")
            {
                unique_id = Guid.NewGuid().ToString();
            }
        }

        public void OnAfterDeserialize()
        {
            if (unique_id == "")
            {
                unique_id = Guid.NewGuid().ToString();
            }
        }

        internal RecipeLayerBase getLayer(string name)
        {
            foreach(var l in layerList)
            {
                if (l.layerName == name)
                    return l;
            }
            return null;
        }

        //Hide the constructor since this is a ScriptableObject and should be instantiated using CreateInstance
        protected TextureRecipe()
        {
        }
    }
}
