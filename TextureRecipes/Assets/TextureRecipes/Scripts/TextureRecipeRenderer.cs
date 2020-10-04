using System.Collections.Generic;
using UnityEngine;

namespace TextureRecipes
{
    public class TextureRecipeRenderer
    {
        List<GameObject> renderObjectList = new List<GameObject>();
        Camera renderCamera;
        public RenderTexture renderTexture;
        Texture2D renderTextureTarget;
        Dictionary<ShaderLayer, Material> renderMaterials = new Dictionary<ShaderLayer, Material>();
        Dictionary<TextLayer, GameObject> textObjects = new Dictionary<TextLayer, GameObject>();

        public TextureRecipe recipe;

        public TextureRecipeRenderer(TextureRecipe _recipe)
        {
            //We are performing a deep copy of the TextureRecipe asset hierarchy. This is so that changes made to the TextureRecipe asset at runtime in the editor
            //don't persist after exiting Play Mode. It also allows us to have multiple renderers using the same TextureRecipe as a source without writing over
            //each others settings. We need to traverse the layer and node hierarchies to clone each object individually and also to remap any references onto the
            //newly-instantiated objects. 
            //TODO: move the layer copying logic into the layer classes to give better layer abstraction
            recipe = GameObject.Instantiate(_recipe);//TODO: better naming here
            recipe.name = _recipe.name;//TODO: necessary?
            recipe.unique_id = _recipe.unique_id;

            int numLayers = recipe.layerList.Count;
            for (int i = 0; i < numLayers; i++)
            {
                var originalLayer = recipe.layerList[i];
                var newLayer = GameObject.Instantiate(originalLayer);
                recipe.layerList[i] = newLayer;

                //TODO: layer abstraction
                if (originalLayer is ShaderLayer)
                {
                    Dictionary<BaseNode, BaseNode> nodeRemap = new Dictionary<BaseNode, BaseNode>();
                    var shaderLayer = (ShaderLayer)recipe.layerList[i];
                    int numNodes = shaderLayer.nodes.Count;
                    for (int j = 0; j < numNodes; j++)
                    {
                        var originalNode = shaderLayer.nodes[j];
                        var newNode = GameObject.Instantiate(originalNode);
                        shaderLayer.nodes[j] = newNode;
                        nodeRemap[originalNode] = newNode;
                    }

                    for (int j = 0; j < numNodes; j++)
                    {
                        var newNode = shaderLayer.nodes[j];
                        for (int k = 0; k < newNode.inputs.Count; k++)
                        {
                            var oldInputNode = newNode.inputs[k];
                            if (null != oldInputNode.inputNode)
                            {

                                // Debug.Assert(nodeRemap.ContainsKey(oldInputNode.inputNode));
                                if (nodeRemap.ContainsKey(oldInputNode.inputNode))
                                {
                                    BaseNode.NodeInput input = newNode.inputs[k];
                                    input.inputNode = nodeRemap[oldInputNode.inputNode];
                                    newNode.inputs[k] = input;
                                    Debug.Assert(newNode.inputs[k].inputNode.name.Contains("Clone"));
                                }
                            }
                        }
                    }

                    shaderLayer.root = (RootNode)nodeRemap[shaderLayer.root];
                }
            }
        }

        public void renderTeardown()
        {
            foreach (var obj in renderObjectList)
            {
                GameObject.DestroyImmediate(obj);
            }
            renderObjectList.Clear();
        }

        public void renderRecipeInternal()
        {
            foreach (var obj in renderObjectList)
            {
                obj.SetActive(true);
            }

            foreach (var layer in recipe.layerList)
            {
                //TODO: layer abstraction
                if (layer is ShaderLayer)
                {
                    ShaderLayer shaderLayer = (ShaderLayer)layer;
                    shaderLayer.root.setParameters(renderMaterials[shaderLayer]);
                }
                else if (layer is TextLayer)
                {
                    TextLayer textLayer = (TextLayer)layer;
                    textLayer.preRender(textObjects[textLayer]);
                }
            }

            RenderTexture.active = renderTexture;

            renderCamera.targetTexture = renderTexture;
            renderCamera.Render();
            renderCamera.targetTexture = null;

            foreach (var obj in renderObjectList)
            {
                obj.SetActive(false);
            }
        }
        public void renderRecipe()
        {
            renderRecipeInternal();
            RenderTexture.active = null;
        }

        static float renderOffsetX = 0.0f;

        public void renderSetup(Texture2D textureTarget = null)
        {
            renderMaterials.Clear();
            textObjects.Clear();

            renderOffsetX += 10.0f;
            renderTextureTarget = textureTarget;
            if (textureTarget != null)
            {
                renderTexture = new RenderTexture(textureTarget.width, textureTarget.height, 0);
            }
            else
            {
                renderTexture = new RenderTexture(recipe.TextureWidth, recipe.TextureHeight, 0);
            }


            GameObject renderCameraObject = new GameObject();
            renderCamera = renderCameraObject.AddComponent<Camera>();
            renderCamera.backgroundColor = Color.black;
            renderCamera.orthographic = true;
            renderCamera.orthographicSize = 5;
            renderCamera.transform.localPosition = new Vector3(renderOffsetX, 0, 0);
            renderCamera.transform.localRotation = Quaternion.Euler(new Vector3(90.0f, 0.0f, 0.0f));
            renderCamera.cullingMask = 1 << recipe.RenderLayer;

            renderCameraObject.layer = recipe.RenderLayer;
            renderObjectList.Add(renderCameraObject);

            foreach (var layer in recipe.layerList)
            {
                //TODO: layer abstraction
                if (layer is ShaderLayer)
                {
                    ShaderLayer shaderLayer = (ShaderLayer)layer;

                    GameObject renderPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
                    renderPlane.transform.localPosition = new Vector3(renderOffsetX, -10, 0);
                    renderPlane.transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
                    renderPlane.layer = recipe.RenderLayer;
                    renderObjectList.Add(renderPlane);

                    string shaderPath = ShaderLayer.getShaderPath(recipe, shaderLayer);
                    Shader shaderAsset = Shader.Find(shaderPath);
                    if (null == shaderAsset)
                    {
                        Debug.LogError("Failed to get shader at " + shaderPath);
                        continue;
                    }
                    Material layerMaterial = new Material(shaderAsset);
                    renderMaterials[shaderLayer] = layerMaterial;
                    renderPlane.GetComponent<MeshRenderer>().material = layerMaterial;
                }
                else if (layer is TextLayer)
                {
                    var textLayer = (TextLayer)layer;
                    GameObject txtObject = new GameObject();
                    txtObject.transform.localPosition = new Vector3(textLayer.position.x + renderOffsetX, -5, textLayer.position.y);
                    txtObject.transform.localRotation = Quaternion.Euler(new Vector3(90, 0, 0));
                    txtObject.layer = recipe.RenderLayer;
                    renderObjectList.Add(txtObject);

                    var textMesh = txtObject.AddComponent<TextMesh>();

                    textMesh.font = textLayer.font;
                    textMesh.text = textLayer.text;
                    textMesh.color = textLayer.textColor;
                    textMesh.fontSize = textLayer.fontSize;
                    textMesh.characterSize = textLayer.characterSize;
                    textMesh.anchor = textLayer.anchor;

                    textObjects[textLayer] = txtObject;

                    //If the material isn't set the text won't appear
                    MeshRenderer rend = txtObject.GetComponentInChildren<MeshRenderer>();
                    rend.material = textMesh.font.material;
                }
            }

        }
    }
}
