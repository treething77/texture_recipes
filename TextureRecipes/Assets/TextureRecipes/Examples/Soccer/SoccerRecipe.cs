using TextureRecipes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoccerRecipe : MonoBehaviour
{
    public List<Texture2D> TeamLogos = new List<Texture2D>();
    public List<Color> ColorList1 = new List<Color>();
    public List<Color> ColorList2 = new List<Color>();
    public List<string> PlayerNames = new List<string>();

    public List<GameObject> TeamAPlayers = new List<GameObject>();
    public List<GameObject> TeamBPlayers = new List<GameObject>();

    void Shuffle<T>(IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    void Awake()
    {
        //Choose team logos
        Shuffle(TeamLogos);
        Shuffle(ColorList1);
        Shuffle(ColorList2);

        var logo_a = TeamLogos[0];
        var logo_b = TeamLogos[1];

        var color1_a = ColorList1[0];
        var color2_a = ColorList2[0];

        var color1_b = ColorList1[1];
        var color2_b = ColorList2[1];

        List<string> names = new List<string>(PlayerNames);
        Shuffle(names);

        SetupTeam(TeamAPlayers, logo_a, color1_a, color2_a, names);
        SetupTeam(TeamBPlayers, logo_b, color1_b, color2_b, names);
    }

    private void SetupTeam(List<GameObject> teamAPlayers, Texture2D logo, Color color1, Color color2, List<string> names)
    {
        foreach (var go in teamAPlayers)
        {
            var textureRecipe = go.GetComponent<TextureRecipeMesh>();

            var layer = (ShaderLayer)textureRecipe.RecipeRender.recipe.getLayer("base");
            var logoNode = (TextureNode)layer.getNode("logo");
            logoNode.Texture = logo;

            var color1Node = (ColorNode)layer.getNode("color1");
            color1Node.color = color1;

            var color2Node = (ColorNode)layer.getNode("color2");
            color2Node.color = color2;

            string name = names[0];
            names.RemoveAt(0);

            var nameLayer = (TextLayer)textureRecipe.RecipeRender.recipe.getLayer("name");
            nameLayer.text = name;

            var numberLayer = (TextLayer)textureRecipe.RecipeRender.recipe.getLayer("number");
            numberLayer.text = UnityEngine.Random.Range(1,100).ToString();
        }
    }
}
