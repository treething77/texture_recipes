using System;

namespace TextureRecipes
{
    public class Noise3DNodeGenerator : BaseNodeGenerator
    {
        public override string getInputs(BaseNode.NodeInput nodeInput)
        {
            var node = nodeInput.inputNode;
            string inputs = "float noiseScale" + node.getNodeID() + ";\n";
            inputs += "float noiseZ" + node.getNodeID() + ";\n";
            return inputs;
        }

        public override string getFunctionBody(BaseNode.NodeInput nodeInput)
        {
            var node = nodeInput.inputNode;
            return "   float s = snoise(float3(uv * noiseScale" + node.getNodeID() + ", noiseZ" + node.getNodeID() + "));\n" +
                   "   return float4(s,s,s,s);\n";
        }

        public override string getProperties(BaseNode.NodeInput nodeInput)
        {
            var node = nodeInput.inputNode;
            string properties = "noiseScale" + node.getNodeID() + "(\"noise scale\", Float) = 1.0";
            properties += "noiseZ" + node.getNodeID() + "(\"noise z\", Float) = 0.0";
            return properties;
        }
    }
}
