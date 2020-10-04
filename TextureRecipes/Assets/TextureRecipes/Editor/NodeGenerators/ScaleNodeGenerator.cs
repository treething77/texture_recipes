using System;

namespace TextureRecipes
{
    public class ScaleNodeGenerator : BaseNodeGenerator
    {
        public override string getPreEvaluation(BaseNode.NodeInput nodeInput)
        {
            var node = nodeInput.inputNode;
            string uvStore = "float2 uv_store" + node.getNodeID() + " = uv;\n";
            uvStore += "float2 scaleFactor" + node.getNodeID() + " = float2(scaleU" + node.getNodeID() + ", scaleV" + node.getNodeID() + ");\n";
            uvStore += "uv -= float2(0.5, 0.5);\n";
            uvStore += "uv /= scaleFactor" + node.getNodeID() + ";\n";
            uvStore += "uv += float2(0.5, 0.5);\n";

            return uvStore;
        }

        public override string getPostEvaluation(BaseNode.NodeInput nodeInput)
        {
            var node = nodeInput.inputNode;
            string uvRestore = "uv = uv_store" + node.getNodeID() + ";\n";
            return uvRestore;
        }

        public override string getFunctionBody(BaseNode.NodeInput nodeInput)
        {
            return "   return input1;\n";
        }

        public override string getInputs(BaseNode.NodeInput nodeInput)
        {
            var node = nodeInput.inputNode;
            string inputStr = "float scaleU" + node.getNodeID() + ";\n";
            inputStr += "float scaleV" + node.getNodeID() + ";\n";
            return inputStr;
        }

        public override string getProperties(BaseNode.NodeInput nodeInput)
        {
            var node = nodeInput.inputNode;
            string propStr = "scaleU" + node.getNodeID() + "(\"scaleU\", Float) = 0.0\n";
            propStr += "scaleV" + node.getNodeID() + "(\"scaleV\", Float) = 0.0\n";
            return propStr;
        }
    }
}
