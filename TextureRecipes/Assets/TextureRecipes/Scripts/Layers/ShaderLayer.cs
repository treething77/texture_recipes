using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace TextureRecipes
{
    [Serializable]
    public class ShaderLayer : RecipeLayerBase
    {
        public RootNode root;
        public List<BaseNode> nodes;

        [NonSerialized]
        public int nodeGenID;

        public static readonly Color layerColor = new Color(0.97f,0.87f,0.87f);

        //Hide the constructor since this is a ScriptableObject and should be instantiated using CreateInstance
        protected ShaderLayer()
        {
        }

        public new void OnEnable()
        {
            base.OnEnable();
            if (null == nodes)
            {
                //first time initialization
                nodes = new List<BaseNode>();
            }

            foreach(var node in nodes)
            {
                if (node.nodeID >= nodeGenID)
                {
                    nodeGenID = node.nodeID + 1;
                }
            }
        }

        public BaseNode getNode(string name)
        {
            foreach (var node in nodes)
            {
                if (node.nodeName == name)
                {
                    return node;
                }
            }
            return null;
        }

        public RootNode getRoot()
        {
            if (null == root)
            {
                //Can't do this in OnEnable because the ShaderLayer isn't registered yet
                //we get an error "AddAssetToSameFile failed because the other asset is not persistent"
                root = (RootNode)CreateInstance(typeof(RootNode));
                nodes.Add(root);
#if UNITY_EDITOR
                AssetDatabase.AddObjectToAsset(root, this);
                AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(root));
#endif
            }
            return root;
        }

        public static string getShaderPath(TextureRecipe recipe, ShaderLayer shaderLayer)
        {
            return "TextureKit/" + getShaderName(recipe, shaderLayer);
        }

        public static string getShaderName(TextureRecipe recipe, ShaderLayer shaderLayer)
        {
            return recipe.unique_id + "-" + recipe.name + "-" + shaderLayer.layerName;
        }
    }
}
