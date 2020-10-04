using System;

namespace TextureRecipes
{
    public class RotateNodeGenerator : BaseNodeGenerator
    {
        public override string getPreEvaluation(BaseNode.NodeInput nodeInput)
        {
            var node = nodeInput.inputNode;
            string uvStore = "float2 uv_store" + node.getNodeID() + " = uv;\n";
            string rotateStr = "rotateAmount" + node.getNodeID();
            //TODO: all these variables need to be scoped or made unique using the node id
            uvStore += "float sinX = sin(" + rotateStr + ");\n";
            uvStore += "float cosX = cos(" + rotateStr + ");\n";
            uvStore += "float sinY = sin(" + rotateStr + ");\n";
            uvStore += "float2x2 rotationMatrix = float2x2(cosX, -sinX, sinY, cosX);\n";
            uvStore += "float2 rotOffset = float2(rotateCenterU" + node.getNodeID() + ", rotateCenterV" + node.getNodeID() + ");\n";
            uvStore += "uv = mul(uv - rotOffset, rotationMatrix) + rotOffset;\n";
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
            string inputStr = "float rotateAmount" + node.getNodeID() + ";\n";
            inputStr += "float rotateCenterU" + node.getNodeID() + ";\n";
            inputStr += "float rotateCenterV" + node.getNodeID() + ";\n";
            return inputStr;
        }

        public override string getProperties(BaseNode.NodeInput nodeInput)
        {
            var node = nodeInput.inputNode;
            string propStr = "rotateAmount" + node.getNodeID() + "(\"rotateAmount\", Float) = 0.0\n";
            propStr += "rotateCenterU" + node.getNodeID() + "(\"rotateCenterU\", Float) = 0.0\n";
            propStr += "rotateCenterV" + node.getNodeID() + "(\"rotateCenterV\", Float) = 0.0\n";
            return propStr;
        }
    }
}
