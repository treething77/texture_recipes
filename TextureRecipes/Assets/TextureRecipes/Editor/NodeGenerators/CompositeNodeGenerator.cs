using System;
using System.Collections.Generic;
using UnityEngine;

namespace TextureRecipes
{
    public class CompositeNodeGenerator : BaseNodeGenerator
    {
        public override string getFunctionBody(BaseNode.NodeInput nodeInput)
        {
            string funcBody = "   float t = input2.a;\n";
            funcBody += "   return (input1 * (1.0-t)) + (input2 * t);\n";
            return funcBody;
        }

        public override string getInputs(BaseNode.NodeInput nodeInput)
        {
            string inputStr = "";
            return inputStr;
        }

        public override string getProperties(BaseNode.NodeInput nodeInput)
        {
            string propStr = "";
            return propStr;
        }
    }
}
