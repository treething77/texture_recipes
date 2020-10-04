using TextureRecipes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipRecipe : MonoBehaviour
{
    public List<Texture2D> Decals = new List<Texture2D>();
    public List<Texture2D> Damage = new List<Texture2D>();

    void Awake()
    {
        var textureRecipe = GetComponent<TextureRecipeMesh>();

        //We need to go through the copy of the recipe that we use for rendering, otherwise changes are applied to all instances
        //TODO: maybe textureRecipeMesh.getLayerInstance? getLayerShared? don't expose recipes directly
        var layer = (ShaderLayer)textureRecipe.RecipeRender.recipe.getLayer("shader");

        var node = (TextureNode)layer.getNode("decal");
        int decalIndex = Random.Range(0, Decals.Count);
        node.Texture = Decals[decalIndex];
        
        var color_node = (ColorNode)layer.getNode("team color");
        color_node.color = Random.ColorHSV(0.0f, 1.0f, 0.5f, 1.0f, 0.0f, 1.0f, 1.0f, 1.0f);
       
        node = (TextureNode)layer.getNode("damage1");
        decalIndex = Random.Range(0, Damage.Count);
        node.Texture = Damage[decalIndex];

        var node_translate = (TranslateNode)layer.getNode("damage1_translate");
        node_translate.translateU = Random.Range(-0.4f, 0.8f);
        node_translate.translateV = Random.Range(0.9f, 2.8f);

        var node_scale = (ScaleNode)layer.getNode("damage1_scale");
        float decal_scale = Random.Range(0.05f, 0.2f);
        node_scale.scaleU = decal_scale;
        node_scale.scaleV = decal_scale;

        node = (TextureNode)layer.getNode("damage2");
        decalIndex = Random.Range(0, Damage.Count);
        node.Texture = Damage[decalIndex];

        node_translate = (TranslateNode)layer.getNode("damage2_translate");
        node_translate.translateU = Random.Range(-0.4f, 0.8f);
        node_translate.translateV = Random.Range(0.9f, 2.8f);

        node_scale = (ScaleNode)layer.getNode("damage2_scale");
        decal_scale = Random.Range(0.05f, 0.2f);
        node_scale.scaleU = decal_scale;
        node_scale.scaleV = decal_scale;

        float dirt_amount = Random.Range(0.0f, 0.5f);
        var dirt_color_node = (ColorNode)layer.getNode("dirt color");
        dirt_color_node.color.a = dirt_amount;
    }
}
