using System;
using System.Collections.Generic;
using UnityEngine;

namespace TextureRecipes
{
    public class BoostNodeGenerator : BaseNodeGenerator
    {
        public override string getFunctionBody(BaseNode.NodeInput nodeInput)
        {
            var node = nodeInput.inputNode;
            return "   return input1 * multiplier" + node.getNodeID() + ";\n";
        }

        public override string getInputs(BaseNode.NodeInput nodeInput)
        {
            var node = nodeInput.inputNode;
            string inputStr = "float multiplier" + node.getNodeID() + ";\n";
            return inputStr;
        }

        public override string getProperties(BaseNode.NodeInput nodeInput)
        {
            var node = nodeInput.inputNode;
            string propStr = "multiplier" + node.getNodeID() + "(\"multiplier\", Float) = 1.0";
            return propStr;
        }
    }
}
