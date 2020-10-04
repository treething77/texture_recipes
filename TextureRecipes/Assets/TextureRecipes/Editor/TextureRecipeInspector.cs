using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using System.Diagnostics;

namespace TextureRecipes
{
    [CustomEditor(typeof(TextureRecipe))]
    public class TextureRecipeInspector : Editor
    {
        private ReorderableList reorderableList;
        Editor subEditor;
        Type subEditorType;

        static Dictionary<string, Texture2D> previewTextures = new Dictionary<string, Texture2D>();

        bool refreshPreviewTexture = true;
        Texture refreshTex;

        private void DrawListHeader(Rect rect)
        {
            GUI.Label(rect, "Recipe Layers");
        }

        private void DrawListElement(Rect rect, int index, bool active, bool focused)
        {
            serializedObject.Update();

            var layerList = serializedObject.FindProperty("layerList");
            if (index < layerList.arraySize)
            {
                var listItem = layerList.GetArrayElementAtIndex(index);
                if (null == listItem.objectReferenceValue)
                {
                    return;
                }

                RecipeLayerBase layerObj = (RecipeLayerBase)listItem.objectReferenceValue;

                var layerType = listItem.objectReferenceValue.GetType();
                var ft = layerType.GetField("layerColor");
                EditorGUI.DrawRect(rect, (Color)ft.GetValue(null));

                SerializedObject subObject = new SerializedObject(listItem.objectReferenceValue);
                var nameProp = subObject.FindProperty("layerName");

                string layerName = nameProp.stringValue;

                if (layerName == null || layerName == "")
                {
                    layerName = Utilities.MakeNiceName(layerName);
                }

                var textRect = rect;
                var textDimensions = GUI.skin.label.CalcSize(new GUIContent(layerName));
                textRect.xMax = textRect.xMin + textDimensions.x + 3;

                GUI.backgroundColor = new Color(1.0f, 1.0f, 1.0f, 0.5f);

                EditorGUI.BeginChangeCheck();
                string newLayerName = EditorGUI.TextField(textRect, layerName);
                //ensure name value is unique if it changed
                if (EditorGUI.EndChangeCheck())
                {
                    TextureRecipe recipe = (TextureRecipe)target;
                    bool nameAlreadyUsed = false;
                    foreach(var layer in recipe.layerList)
                    {
                        if (layer.layerName == newLayerName)
                        {
                            nameAlreadyUsed = true;
                            break;
                        }
                    }

                    if (!nameAlreadyUsed)
                    {
                        nameProp.stringValue = newLayerName;
                    }
                }

                subObject.ApplyModifiedProperties();
            }
        }

        void CreateLayerCallback(object obj)
        {
            Type objectType = (Type)obj;
            TextureRecipe t = (TextureRecipe)target;

            Undo.RecordObject(t, "Create " + objectType.Name);
            {
                var layerObject = (RecipeLayerBase)CreateInstance(objectType);
                t.layerList.Add(layerObject);

                layerObject.layerName = Utilities.MakeNiceName(objectType.Name);

                AssetDatabase.AddObjectToAsset(layerObject, t);
                AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(layerObject));
            }
            Undo.FlushUndoRecordObjects();

            serializedObject.Update();

            EditorUtility.SetDirty(target);
        }

        private void AddListItem(ReorderableList list)
        {
            GenericMenu menu = new GenericMenu();

            foreach (Type type in Assembly.GetAssembly(typeof(RecipeLayerBase)).GetTypes()
            .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(RecipeLayerBase))))
            {
                menu.AddItem(new GUIContent(Utilities.MakeNiceName(type.Name)), false, CreateLayerCallback, type);
            }

            menu.ShowAsContext();
        }

        private void RemoveListItem(ReorderableList list)
        {
            serializedObject.Update();

            var layerList = serializedObject.FindProperty("layerList");

            //set the element to null first to make sure that DeleteArrayElementAtIndex actually removes the entry and doesn't just null it
            layerList.GetArrayElementAtIndex(list.index).objectReferenceValue = null;
            layerList.DeleteArrayElementAtIndex(list.index);

            serializedObject.ApplyModifiedProperties();

            EditorUtility.SetDirty(target);
        }

        private void OnDisable()
        {
            // Make sure we don't get memory leaks etc.
            reorderableList.drawHeaderCallback -= DrawListHeader;
            reorderableList.drawElementCallback -= DrawListElement;

            reorderableList.onAddCallback -= AddListItem;
            reorderableList.onRemoveCallback -= RemoveListItem;
        }

        private void OnEnable()
        {
            var layerList = serializedObject.FindProperty("layerList");

            reorderableList = new ReorderableList(serializedObject, layerList);

            // Add listeners to draw events
            reorderableList.drawHeaderCallback += DrawListHeader;
            reorderableList.drawElementCallback += DrawListElement;

            reorderableList.onAddCallback += AddListItem;
            reorderableList.onRemoveCallback += RemoveListItem;

            refreshTex = AssetDatabase.LoadAssetAtPath<Texture>("Assets/TextureRecipes/Editor/Icons/refresh.png");
        }

        public override bool RequiresConstantRepaint() { return false; }
        public override bool HasPreviewGUI() { return true; }

        public override Texture2D RenderStaticPreview(string assetPath, UnityEngine.Object[] subAssets, int width, int height)
        {
            Texture2D previewTex = null;
            previewTextures.TryGetValue(assetPath, out previewTex);
            if (null == previewTex)
            {
                TextureRecipe targetRecipe = (TextureRecipe)target;

                previewTex = new Texture2D(128, 128);
                previewTextures[assetPath] = previewTex;

                UpdatePreviewTexture(targetRecipe, previewTex);
            }
            return Instantiate(previewTex);
        }

        void UpdatePreviewTexture(TextureRecipe targetRecipe, Texture2D previewTex)
        {
            if (!refreshPreviewTexture)
            {
                return;
            }

            refreshPreviewTexture = false;
            
            GeneratorFactory.createMappings();
            RecipeBuildPreProcess.bakeSubAssets(targetRecipe);

            TextureRecipeRenderer renderer = new TextureRecipeRenderer(targetRecipe);
            renderer.renderSetup(previewTex);
            renderer.renderRecipeInternal();

            previewTex.ReadPixels(new Rect(0, 0, previewTex.width, previewTex.height), 0, 0);
            previewTex.Apply();

            RenderTexture.active = null;

            renderer.renderTeardown();
        }

        bool drawingHeader = false;
        protected override void OnHeaderGUI()
        {
            //Jumping through hoops since we can't override just the functionality that we want
            drawingHeader = true;
            base.OnHeaderGUI();
            drawingHeader = false;
        }

        public override void OnPreviewGUI(Rect r, GUIStyle s)
        {
            //We don't want any of our preview in the header, just normal header elements
            if (drawingHeader)
            {
                return;
            }

            TextureRecipe targetRecipe = (TextureRecipe)target;

            EditorGUILayout.BeginHorizontal();

            bool autoRefreshWasEnabled = targetRecipe.autoRefresh;
            targetRecipe.autoRefresh = GUILayout.Toggle(targetRecipe.autoRefresh, "auto");
            if (!autoRefreshWasEnabled && targetRecipe.autoRefresh)
            {
                refreshPreviewTexture = true;
            }

            if (GUILayout.Button(refreshTex, GUILayout.Width(30.0f)))
            {
                refreshPreviewTexture = true;
            }

            EditorGUILayout.EndHorizontal();
            Texture2D previewTex = null;
            string key = AssetDatabase.GetAssetPath(target);

            previewTextures.TryGetValue(key, out previewTex);

            if (!previewTex)
            {
                previewTex = new Texture2D(128, 128);
                previewTextures[key] = previewTex;
            }

            UpdatePreviewTexture(targetRecipe, previewTex);

            if (r.width > r.height)
            {
                r.x = (r.width - r.height) / 2.0f;
                r.width = r.height;
            }
            else
            {
                r.y += (r.height - r.width) / 2.0f;
                r.height = r.width;
            }

            EditorGUI.DrawTextureTransparent(r, previewTex);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            TextureRecipe targetRecipe = (TextureRecipe)target;
            var texWidthProp = serializedObject.FindProperty("TextureWidth");
            var texHeightProp = serializedObject.FindProperty("TextureHeight");
            texWidthProp.intValue = EditorGUILayout.IntField("Width", texWidthProp.intValue);
            texHeightProp.intValue = EditorGUILayout.IntField("Height", texHeightProp.intValue);
            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Bake"))
            {
                GeneratorFactory.createMappings();
                RecipeBuildPreProcess.bakeSubAssets(targetRecipe);

                TextureRecipeRenderer renderer = new TextureRecipeRenderer(targetRecipe);
                renderer.renderSetup();
                renderer.renderRecipeInternal();

                TextureWriter.WriteRenderBufferToTextureFile(targetRecipe.TextureWidth, targetRecipe.TextureHeight, targetRecipe);
                RenderTexture.active = null;

                renderer.renderTeardown();
            }

            reorderableList.DoLayoutList();

            int listIndex = reorderableList.index;

            var layerList = serializedObject.FindProperty("layerList");
            if (listIndex >=0 && listIndex < layerList.arraySize)
            {
                var listItem = layerList.GetArrayElementAtIndex(listIndex);
                if (null != listItem.objectReferenceValue)
                {
                    if (subEditorType != listItem.objectReferenceValue.GetType())
                    {
                        subEditor = null;
                    }
                    EditorGUI.BeginChangeCheck();

                    ShaderLayerEditor.recipe = (TextureRecipe)target;
                    CreateCachedEditor(listItem.objectReferenceValue, null, ref subEditor);
                    subEditorType = listItem.objectReferenceValue.GetType();

                    subEditor.serializedObject.Update();
                    subEditor.OnInspectorGUI();
                    subEditor.serializedObject.ApplyModifiedProperties();

                    serializedObject.ApplyModifiedProperties();
                    if (EditorGUI.EndChangeCheck() && targetRecipe.autoRefresh)
                    {
                        //do we need this if we set dirty on the target every frame?
                        EditorUtility.SetDirty(listItem.objectReferenceValue);
                        refreshPreviewTexture = true;
                    }

                    if (ShaderLayerEditor.nodeEditor && ShaderLayerEditor.nodeEditor.refreshShader && targetRecipe.autoRefresh)
                    {
                        refreshPreviewTexture = true;
                        ShaderLayerEditor.nodeEditor.refreshShader = false;
                    }
                }
            }

            //We have to do this every frame otherwise changes to the child objects aren't reflected in the inspector when we Undo them
            //there is perhaps an issue with the order of operations
            EditorUtility.SetDirty(target);
        }
    }
}