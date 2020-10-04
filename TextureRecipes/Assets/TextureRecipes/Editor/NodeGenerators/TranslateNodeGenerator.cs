using System;

namespace TextureRecipes
{
    public class TranslateNodeGenerator : BaseNodeGenerator
    {
        public override string getPreEvaluation(BaseNode.NodeInput nodeInput)
        {
            var node = nodeInput.inputNode;
            string uvStore = "float2 uv_store" + node.getNodeID() + " = uv;\n";
            uvStore += "float2 translateOffset" + node.getNodeID() + " = float2(translateU" + node.getNodeID() + ", translateV" + node.getNodeID() + ");\n";
            uvStore += "uv -= translateOffset" + node.getNodeID() + ";\n";
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
            string inputStr = "float translateU" + node.getNodeID() + ";\n";
            inputStr += "float translateV" + node.getNodeID() + ";\n";
            return inputStr;
        }

        public override string getProperties(BaseNode.NodeInput nodeInput)
        {
            var node = nodeInput.inputNode;
            string propStr = "translateU" + node.getNodeID() + "(\"translateU\", Float) = 0.0\n";
            propStr += "translateV" + node.getNodeID() + "(\"translateV\", Float) = 0.0\n";
            return propStr;
        }
    }
}
