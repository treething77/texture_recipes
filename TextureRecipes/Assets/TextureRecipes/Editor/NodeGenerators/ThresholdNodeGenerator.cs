using System;

namespace TextureRecipes
{
    public class ThresholdNodeGenerator : BaseNodeGenerator
    {
        public override string getFunctionBody(BaseNode.NodeInput nodeInput)
        {
            var node = nodeInput.inputNode;
            return "   float g = dot(input1, float4(1,1,1,1));\n" +
                   "   if (g > threshold" + node.getNodeID() + ")\n" +
                   "      return input1;\n" +
                   "   else\n" +
                   "      return float4(0,0,0,1);\n";
        }

        public override string getInputs(BaseNode.NodeInput nodeInput)
        {
            var node = nodeInput.inputNode;
            string inputStr = "float threshold" + node.getNodeID() + ";\n";
            return inputStr;
        }

        public override string getProperties(BaseNode.NodeInput nodeInput)
        {
            var node = nodeInput.inputNode;
            string propStr = "threshold" + node.getNodeID() + "(\"threshold\", Float) = 0.0\n";
            return propStr;
        }
    }
}
