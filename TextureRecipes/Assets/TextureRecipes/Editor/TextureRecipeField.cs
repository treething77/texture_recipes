using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TextureRecipes;

#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class TextureRecipeField
{
    [SerializeField]
    public TextureRecipe recipeAsset;
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(TextureRecipeField))]
public class TextureRecipeFieldPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
    {
        EditorGUI.BeginProperty(_position, GUIContent.none, _property);
        SerializedProperty recipeAsset = _property.FindPropertyRelative("recipeAsset");
        _position = EditorGUI.PrefixLabel(_position, GUIUtility.GetControlID(FocusType.Passive), _label);
        if (recipeAsset != null)
        {
            recipeAsset.objectReferenceValue = EditorGUI.ObjectField(_position, recipeAsset.objectReferenceValue, typeof(TextureRecipe), false);
            if (recipeAsset.objectReferenceValue != null)
            {
                recipeAsset.stringValue = (recipeAsset.objectReferenceValue as TextureRecipe).name;
            }
        }
        EditorGUI.EndProperty();
    }
}
#endif
