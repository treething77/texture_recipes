using System;
using System.Collections.Generic;
using UnityEngine;

namespace TextureRecipes
{
    public class AddNodeGenerator : BaseNodeGenerator
    {
        public override string getFunctionBody(BaseNode.NodeInput nodeInput)
        {
            var node = nodeInput.inputNode;
            return "   return (input1 * weightOne" + node.getNodeID() + ") + (input2 * weightTwo" + node.getNodeID() + ");\n";
        }

        public override string getInputs(BaseNode.NodeInput nodeInput)
        {
            var node = nodeInput.inputNode;
            string inputStr = "float weightOne" + node.getNodeID() + ";\n";
            inputStr += "float weightTwo" + node.getNodeID() + ";\n";
            return inputStr;
        }

        public override string getProperties(BaseNode.NodeInput nodeInput)
        {
            var node = nodeInput.inputNode;
            string propStr = "weightOne" + node.getNodeID() + "(\"weightOne\", Float) = 0.0\n";
            propStr += "weightTwo" + node.getNodeID() + "(\"weightTwo\", Float) = 0.0\n";
            return propStr;
        }
    }
}
