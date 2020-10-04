using System;

namespace TextureRecipes
{
    public class NoiseNodeGenerator : BaseNodeGenerator
    {
        public override string getInputs(BaseNode.NodeInput nodeInput)
        {
            var node = nodeInput.inputNode;
            return "float noiseScale" + node.getNodeID() + ";\n";
        }

        public override string getFunctionBody(BaseNode.NodeInput nodeInput)
        {
            var node = nodeInput.inputNode;
            return "   float s = snoise(uv * noiseScale" + node.getNodeID() + ");\n" +
                   "   return float4(s,s,s,s);\n";
        }

        public override string getProperties(BaseNode.NodeInput nodeInput)
        {
            var node = nodeInput.inputNode;
            return "noiseScale" + node.getNodeID() + "(\"noise scale\", Float) = 1.0";
        }
    }
}
