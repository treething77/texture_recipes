using TextureRecipes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureRecipeMesh : MonoBehaviour
{
    public TextureRecipe Recipe;
    public TextureRecipeRenderer _recipeRenderer;

    public TextureRecipeRenderer RecipeRender
    {
        get
        {
            if (null == _recipeRenderer)
            {
                _recipeRenderer = new TextureRecipeRenderer(Recipe);
                _recipeRenderer.renderSetup();
            }
            return _recipeRenderer;
        }
    }

    void Start()
    {
        var meshRender = GetComponent<MeshRenderer>();
        RecipeRender.renderRecipe();
        meshRender.material.mainTexture = RecipeRender.renderTexture;
        RecipeRender.renderTeardown();
    }
}
