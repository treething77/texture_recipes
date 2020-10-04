using System;
using UnityEngine;

namespace TextureRecipes
{
    public class TextureNodeGenerator : BaseNodeGenerator
    {
        public override string getFunctionBody(BaseNode.NodeInput nodeInput)
        {
            var node = nodeInput.inputNode;
            return "   float4 s = tex2D(texture" + node.getNodeID() + ",uv);\n" +
                   "   return s;\n";
        }

        public override string getInputs(BaseNode.NodeInput nodeInput)
        {
            var node = nodeInput.inputNode;
            string inputStr = "sampler2D texture" + node.getNodeID() + ";\n";
            return inputStr;
        }

        public override string getProperties(BaseNode.NodeInput nodeInput)
        {
            var node = nodeInput.inputNode;
            string propStr = "texture" + node.getNodeID() + "(\"texture\", 2D) = \"\" {}";
            return propStr;
        }
    }
}
