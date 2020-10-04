using System;
using System.Collections.Generic;
using UnityEngine;

namespace TextureRecipes
{
    public class MultiplyNodeGenerator : BaseNodeGenerator
    {
        public override string getFunctionBody(BaseNode.NodeInput nodeInput)
        {
            var node = nodeInput.inputNode;
            return "   return (input1 * input2);\n";
        }

        public override string getInputs(BaseNode.NodeInput nodeInput)
        {
            return "";
        }

        public override string getProperties(BaseNode.NodeInput nodeInput)
        {
            return "";
        }
    }
}
