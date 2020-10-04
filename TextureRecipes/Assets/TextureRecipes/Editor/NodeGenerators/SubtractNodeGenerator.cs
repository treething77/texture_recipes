using System;
using System.Collections.Generic;
using UnityEngine;

namespace TextureRecipes
{
    public class SubtractNodeGenerator : BaseNodeGenerator
    {
        public override string getFunctionBody(BaseNode.NodeInput nodeInput)
        {
            return "   return input1 - input2;\n";
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
