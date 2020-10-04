using System;
using UnityEngine;

namespace TextureRecipes
{
    public class ColorNodeGenerator : BaseNodeGenerator
    {
        public override string getFunctionBody(BaseNode.NodeInput nodeInput)
        {
            var node = nodeInput.inputNode;
            return "   return color" + node.getNodeID() + ";\n";
        }

        public override string getInputs(BaseNode.NodeInput nodeInput)
        {
            var node = nodeInput.inputNode;
            string inputStr = "fixed4 color" + node.getNodeID() + ";\n";
            return inputStr;
        }

        public override string getProperties(BaseNode.NodeInput nodeInput)
        {
            var node = nodeInput.inputNode;
            string propStr = "color" + node.getNodeID() + "(\"color\", Color) = (0,0,0,0)";
            return propStr;
        }
    }
}
