using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace TextureRecipes
{
    public static class ShaderGenerator
    {
        delegate string NodeGenDelegate(BaseNode.NodeInput nodeInput);

        static Dictionary<string, string[]> existingShaderLines = new Dictionary<string, string[]>();

        public static Shader bakeShader(ShaderLayer shaderLayer, TextureRecipe recipe)
        {
            TextAsset shaderBaseText = (TextAsset)Resources.Load("template.shader");
            if (shaderBaseText == null)
            {
                Debug.LogError("Failed to template shader");
                return null;
            }

            string shaderName = ShaderLayer.getShaderName(recipe, shaderLayer);
            string shaderPath = ShaderLayer.getShaderPath(recipe, shaderLayer);
            string shaderText = shaderBaseText.text;
            List<string> lines = shaderText.Split('\n').ToList<string>();

            int numLines = lines.Count;
            for (int i = 0; i < numLines; i++)
            {
                string line = lines[i];
                if (line.Contains("$$"))
                {
                    if (line.Contains("$$ASSETNAME"))
                    {
                        line = line.Replace("$$ASSETNAME", shaderPath);
                        lines[i] = line;
                    }
                    else if (line.Contains("$$STARTFUNCS"))
                    {
                        var funcs = getFunctions(shaderLayer);
                        lines.InsertRange(i + 1, funcs);
                        i += funcs.Count;
                        numLines = lines.Count;
                    }
                    else if (line.Contains("$$STARTPROPS"))
                    {
                        var props = getProperties(shaderLayer);
                        lines.InsertRange(i + 1, props);
                        i += props.Count;
                        numLines = lines.Count;
                    }
                    else if (line.Contains("$$STARTINPUTS"))
                    {
                        var inputs = getInputs(shaderLayer);
                        lines.InsertRange(i + 1, inputs);
                        i += inputs.Count;
                        numLines = lines.Count;
                    }
                    else if (line.Contains("$$STARTSHADER"))
                    {
                        lines.Insert(i + 1, getCalls(shaderLayer));
                        numLines = lines.Count;
                        i++;
                    }
                }
            }

            string baseAssetPath = "Assets/TextureRecipes/Resources/";
            string shaderAssetPath = baseAssetPath + shaderName + ".shader";
            //Can't do this because we want to overwrite an existing asset! shaderPath = AssetDatabase.GenerateUniqueAssetPath(shaderPath);

            //Generating the text for the shader is a very fast operation. Rather than trying to figure out which changes
            //would have altered the shader, we simply generate the shader as a byproduct of every change, and then see
            //if it has changed since the last time we output it. The really expensive part is outputting the shader and 
            //triggering the Unity shader post-process and compilation steps (we're talking >1.5s vs 3ms).
            string[] existingLines = null;
            existingShaderLines.TryGetValue(shaderPath, out existingLines);

            string[] newLines = lines.ToArray();
            if (existingLines != null)
            {
                if (existingLines.Length == newLines.Length)
                {
                    bool allMatch = true;
                    for (int i = 0; allMatch && (i < newLines.Length); i++)
                    {
                        if (existingLines[i] != newLines[i])
                            allMatch = false;
                    }

                    if (allMatch)
                    {
                        return Shader.Find(shaderPath);
                    }
                }
            }

            //File.WriteAllLines(shaderAssetPath, newLines);
            using (StreamWriter writer = new StreamWriter(shaderAssetPath, false))
            {
                writer.NewLine = "\n";
                foreach (string str in newLines)
                {
                    writer.WriteLine(str);
                }
            }

            existingShaderLines[shaderPath] = newLines;

            AssetDatabase.Refresh();
            return Shader.Find( shaderPath );
        }

        private static List<string> getNodeStrings(ShaderLayer shaderLayer, NodeGenDelegate genDelegate)
        {
            List<string> nodeInputs = new List<string>();
            var rootInputs = shaderLayer.getRoot().inputs;
            if ((rootInputs.Count == 0) || (rootInputs[0].inputNode == null))
            {
                return nodeInputs;
            }

            List<BaseNode.NodeInput> nodeList = new List<BaseNode.NodeInput>();
            Queue<BaseNode.NodeInput> nodeStack = new Queue<BaseNode.NodeInput>();
            nodeStack.Enqueue(shaderLayer.getRoot().inputs[0]);
            while (nodeStack.Count > 0)
            {
                BaseNode.NodeInput node = nodeStack.Dequeue();

                if (!nodeList.Contains(node))
                {
                    nodeList.Add(node);

                    int numInputs = node.inputNode.inputs.Count;
                    for (int i = 0; i < numInputs; i++)
                    {
                        var nodeInput = node.inputNode.inputs[i];
                        if (nodeInput.inputNode != null)
                        {
                            nodeStack.Enqueue(nodeInput);
                        }
                    }

                    nodeInputs.Add(genDelegate(node));
                }
            }
            return nodeInputs;
        }

        private static string getNodeFunctionStr(BaseNode.NodeInput nodeInput)
        {
            var node = nodeInput.inputNode;
            return GeneratorFactory.getShaderGenerator(node).getFunctionSignature(nodeInput) + GeneratorFactory.getShaderGenerator(node).getFunctionBody(nodeInput) + "}\n";
        }

        private static List<string> getFunctions(ShaderLayer shaderLayer)
        {
            return getNodeStrings(shaderLayer, getNodeFunctionStr);
        }

        private static string getNodePropertyStr(BaseNode.NodeInput nodeInput)
        {
            var node = nodeInput.inputNode;
            return GeneratorFactory.getShaderGenerator(node).getProperties(nodeInput);
        }

        private static List<string> getProperties(ShaderLayer shaderLayer)
        {
            return getNodeStrings(shaderLayer, getNodePropertyStr);
        }

        private static string getNodeInputStr(BaseNode.NodeInput nodeInput)
        {
            return GeneratorFactory.getShaderGenerator(nodeInput.inputNode).getInputs(nodeInput);
        }

        private static List<string> getInputs(ShaderLayer shaderLayer)
        {
            return getNodeStrings(shaderLayer, getNodeInputStr);
        }

        private static string getCalls(ShaderLayer shaderLayer)
        {
            Dictionary<string, string> outputVariableNames = new Dictionary<string, string>();

            string nodeCalls = "";
            var rootInput = shaderLayer.getRoot().inputs[0];
            if (rootInput.inputNode != null)
            {
                nodeCalls += GeneratorFactory.getShaderGenerator(rootInput.inputNode).getCallStr(rootInput, outputVariableNames);

                nodeCalls += "col = out" + rootInput.inputNode.GetType().Name + rootInput.inputNode.getNodeID() + "_" + rootInput.outputIndex + ";";
            }

            return nodeCalls;
        }
    }
}
