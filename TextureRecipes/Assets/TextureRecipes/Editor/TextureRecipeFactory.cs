using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace TextureRecipes
{
    public class TextureRecipeFactory
    {
        [MenuItem("Assets/Create/Texture Recipe")]
        public static void CreateTextureRecipe()
        {
            var recipe = ScriptableObject.CreateInstance<TextureRecipe>();
            recipe.unique_id = Guid.NewGuid().ToString();
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (path.Length == 0)
            {
                path = "Assets";
            }
            else if (Path.GetExtension(path) != "")
            {
                path = path.Replace(Path.GetFileName(path), "");
            }
            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New Texture Recipe.asset");
            AssetDatabase.CreateAsset(recipe, assetPathAndName);
            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = recipe;
        }
    }
}