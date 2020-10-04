using System;

namespace TextureRecipes
{
    public class MergeNodeGenerator : BaseNodeGenerator
    {
        public override string getFunctionBody(BaseNode.NodeInput nodeInput)
        {
            string funcStr = "   return float4(";

            MergeNode merge = (MergeNode)nodeInput.inputNode;
            var inputR = merge.inputs[0].inputNode;
            var inputG = merge.inputs[1].inputNode;
            var inputB = merge.inputs[2].inputNode;
            var inputA = merge.inputs[3].inputNode;

            if (null != inputR)
                funcStr += "input1.r, ";
            else
                funcStr += "0.0f, ";

            if (null != inputG)
                funcStr += "input2.g, ";
            else
                funcStr += "0.0f, ";

            if (null != inputB)
                funcStr += "input3.b, ";
            else
                funcStr += "0.0f, ";

            if (null != inputA)
                funcStr += "input4.a";
            else
                funcStr += "1.0f";

            funcStr += ");\n";
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
