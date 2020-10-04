using System;
using UnityEngine;

namespace TextureRecipes
{
    [Serializable]
    public class TextLayer : RecipeLayerBase
    {
        public string text;
        public Font font;
        public Color textColor = Color.white;
        public int fontSize = 20;
        public float characterSize = 1;
        public TextAnchor anchor;
        public Vector2 position;

        public static readonly Color layerColor = new Color(0.914f, 0.97f, 0.97f);

        //Hide the constructor since this is a ScriptableObject and should be instantiated using CreateInstance
        protected TextLayer()
        {
        }

        internal void preRender(GameObject gameObject)
        {
            var textMesh = gameObject.GetComponent<TextMesh>();

            textMesh.font = font;
            textMesh.text = text;
            textMesh.color = textColor;
            textMesh.fontSize = fontSize;
            textMesh.characterSize = characterSize;
            textMesh.anchor = anchor;
        }
    }
}
