using System;
using UnityEngine;

namespace TextureRecipes
{
    [Serializable]
    public class RecipeLayerBase : ScriptableObject
    {
        [HideInInspector]
        public string layerName;

        public void OnEnable()
        {
            //Don't show layer sub-assets in the Project window
            hideFlags = HideFlags.HideInHierarchy;
        }

        //Hide the constructor since this is a ScriptableObject and should be instantiated using CreateInstance
        protected RecipeLayerBase()
        {
        }
    }
}
