using System;
using System.Text;

namespace TextureRecipes
{
    public class SplitNodeGenerator : BaseNodeGenerator
    {
        public override string getFunctionName(BaseNode.NodeInput nodeInput)
        {
            var node = nodeInput.inputNode;
            StringBuilder signature = new StringBuilder();
            signature.Append("get" + node.GetType().Name + node.getNodeID());

            switch (nodeInput.outputIndex)
            {
                case 0:
                    signature.Append("_R");
                    break;
                case 1:
                    signature.Append("_G");
                    break;
                case 2:
                    signature.Append("_B");
                    break;
                case 3:
                    signature.Append("_A");
                    break;
            }
            return signature.ToString();
        }

        public override string getFunctionBody(BaseNode.NodeInput nodeInput)
        {
            string funcStr = "";
            SplitNode split = (SplitNode)nodeInput.inputNode;

            if (split.ReplicateToAllChannels)
            {
                switch (nodeInput.outputIndex)
                {
                    case 0:
                        funcStr = "   return input1.r;\n";
                        break;
                    case 1:
                        funcStr = "   return input1.g;\n";
                        break;
                    case 2:
                        funcStr = "   return input1.b;\n";
                        break;
                    case 3:
                        funcStr = "   return input1.a;\n";
                        break;
                }
            }
            else
            {
                switch (nodeInput.outputIndex)
                {
                    case 0:
                        funcStr = "   return float4(input1.r,0,0,1);\n";
                        break;
                    case 1:
                        funcStr = "   return float4(0,input1.g,0,1);\n";
                        break;
                    case 2:
                        funcStr = "   return float4(0,0,input1.b,1);\n";
                        break;
                    case 3:
                        funcStr = "   return float4(0,0,0,input1.a);\n";
                        break;
                }
            }

            return funcStr;
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
