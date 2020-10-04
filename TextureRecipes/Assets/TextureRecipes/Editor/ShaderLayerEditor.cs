using System;
using UnityEditor;
using UnityEngine;

namespace TextureRecipes
{
    [CustomEditor(typeof(ShaderLayer))]
    public class ShaderLayerEditor : RecipeLayerEditorBase
    {
        Editor subEditor;
  
        //TODO: do we need these statics?
        static public ShaderLayerWindow nodeEditor = null;//we dont own this window, it will outlive the editor itself
        static public TextureRecipe recipe;

        public override void OnInspectorGUI()
        {

            serializedObject.Update();

            EditorGUILayout.Space();
            if (GUILayout.Button("Open Node Editor"))
            {
                nodeEditor = (ShaderLayerWindow)EditorWindow.GetWindow(typeof(ShaderLayerWindow));
                nodeEditor.Show();
            }

            if (null != nodeEditor)
            {
                nodeEditor.recipe = recipe;
                nodeEditor.layerObject = (ShaderLayer) this.target;

                BaseNode n = nodeEditor.SelectedNode;
                if (n != null)
                {
                    EditorGUI.BeginChangeCheck();

                    CreateCachedEditor(n, null, ref subEditor);
#if UNITY_5_6_OR_NEWER
                    subEditor.serializedObject.UpdateIfRequiredOrScript();
#else
                    subEditor.serializedObject.UpdateIfDirtyOrScript();
#endif
                    subEditor.OnInspectorGUI();
                    subEditor.serializedObject.ApplyModifiedProperties();

                    if (EditorGUI.EndChangeCheck())
                    {
                        n.assetsDirty = true;

                        //do we need this if we set dirty on the target every frame?
                        EditorUtility.SetDirty(n);
                    }
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
 