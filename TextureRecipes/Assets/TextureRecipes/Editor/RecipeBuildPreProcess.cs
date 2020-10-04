using System.Collections.Generic;
using UnityEditor;

namespace TextureRecipes
{
    [InitializeOnLoad]
    public class RecipeBuildPreProcess : UnityEditor.AssetModificationProcessor
    {
        static RecipeBuildPreProcess()
        {
            EditorApplication.playmodeStateChanged += buildModifiedShaders;
        }

        static void buildModifiedShaders()
        {
            if (!EditorApplication.isPlaying && EditorApplication.isPlayingOrWillChangePlaymode)
            {
                //TODO: is there a way to figure out which shaders need building?
                var recipeGUIDs = AssetDatabase.FindAssets("t:TextureRecipe");
                foreach (var recipeID in recipeGUIDs)
                {
                    string assetPath = AssetDatabase.GUIDToAssetPath(recipeID);
                    TextureRecipe recipe = AssetDatabase.LoadAssetAtPath<TextureRecipe>(assetPath);
                    GeneratorFactory.createMappings();
                    bakeSubAssets(recipe);
                }
            }
        }

        public static List<string> bakeSubAssets(TextureRecipe recipe)
        {
            List<string> subAssetPaths = new List<string>();

            foreach (var layer in recipe.layerList)
            {
                if (layer is ShaderLayer)
                {
                    bakeShaderLayerSubAssets((ShaderLayer)layer, recipe, subAssetPaths);
                }
            }

            if (subAssetPaths.Count > 0)
            {
                AssetDatabase.Refresh();
            }
            return subAssetPaths;
        }

        static void bakeShaderLayerSubAssets(ShaderLayer shaderLayer, TextureRecipe recipe, List<string> subAssetPaths)
        {
            var shader = ShaderGenerator.bakeShader(shaderLayer, recipe);
            if (null == shader)
            {
                UnityEngine.Debug.LogError("Failed to load shader " + shaderLayer.name);
                return;
            }
            subAssetPaths.Add(shader.name);

            foreach (var node in shaderLayer.nodes)
            {
                if (!node.assetsDirty)
                {
                    continue;
                }

                var nodeAssetGenerator = GeneratorFactory.getAssetGenerator(node);
                if (null != nodeAssetGenerator)
                {
                    var newAssetPaths = nodeAssetGenerator.generateAssets(node);
                    if (newAssetPaths.Count > 0)
                    {
                        subAssetPaths.AddRange(newAssetPaths);
                    }
                    node.assetsDirty = false;
                }
            }
        }

        static string[] OnWillSaveAssets(string[] assetPaths)
        {
            //find all TextureRecipe assets, then check if any are being saved
            List<string> recipeAssetPaths = new List<string>();

            var recipeGUIDs = AssetDatabase.FindAssets("t:TextureRecipe");
            foreach(var recipeID in recipeGUIDs)
            {
                recipeAssetPaths.Add(AssetDatabase.GUIDToAssetPath(recipeID));
            }

            foreach (string assetPath in assetPaths)
            {
                if (recipeAssetPaths.Contains(assetPath))
                {
                    //We are saving this recipe so generate any other assets that it requires
                    TextureRecipe recipe = AssetDatabase.LoadAssetAtPath<TextureRecipe>(assetPath);
                    GeneratorFactory.createMappings();

                    var builtSubAssetPaths = bakeSubAssets(recipe);

                    var oldAssetPaths = new List<string>(assetPaths);
                    oldAssetPaths.AddRange(builtSubAssetPaths);
                    assetPaths = oldAssetPaths.ToArray();
                }                       
            }

            return assetPaths;
        }
    }
}
