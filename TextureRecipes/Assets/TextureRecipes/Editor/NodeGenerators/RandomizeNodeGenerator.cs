using System;
using UnityEngine;

namespace TextureRecipes
{
    public class RandomizeNodeGenerator : BaseNodeGenerator
    {
        public override string getFunctionBody(BaseNode.NodeInput nodeInput)
        {
            string funcStr = "";

            var node = nodeInput.inputNode;
            string selectName = "select" + node.getNodeID();
            funcStr += "   if (" + selectName + " == 0)\n";
            funcStr += "      return input1;\n";
            funcStr += "   else if (" + selectName + " == 1)\n";
            funcStr += "      return input2;\n";
            funcStr += "   else if (" + selectName + " == 2)\n";
            funcStr += "      return input3;\n";
            funcStr += "   return input4;\n";

            return funcStr;
        }

        public override string getInputs(BaseNode.NodeInput nodeInput)
        {
            var node = nodeInput.inputNode;
            string inputStr = "int select" + node.getNodeID() + ";\n";
            return inputStr;
        }

        public override string getProperties(BaseNode.NodeInput nodeInput)
        {
            return "";
        }
    }
}
