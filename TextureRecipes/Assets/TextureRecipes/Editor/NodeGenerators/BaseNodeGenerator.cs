using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace TextureRecipes
{
    public abstract class BaseNodeGenerator
    {
        public abstract string getFunctionBody(BaseNode.NodeInput nodeInput);
        public abstract string getInputs(BaseNode.NodeInput nodeInput);
        public abstract string getProperties(BaseNode.NodeInput nodeInput);
        
        public virtual string getPreEvaluation(BaseNode.NodeInput nodeInput) { return ""; }
        public virtual string getPostEvaluation(BaseNode.NodeInput nodeInput) { return ""; }

        public virtual string getFunctionName(BaseNode.NodeInput nodeInput)
        {
            var node = nodeInput.inputNode;
            StringBuilder signature = new StringBuilder();
            signature.Append("get" + node.GetType().Name + node.getNodeID());
            return signature.ToString();
        }

        public virtual string getFunctionSignature(BaseNode.NodeInput nodeInput)
        {
            var node = nodeInput.inputNode;
            StringBuilder signature = new StringBuilder();
            signature.Append("float4 " + getFunctionName(nodeInput) + "(float2 uv, float w, float h");

            int numInputs = node.inputs.Count;
            for (int i = 0; i < numInputs; i++)
            {
                signature.Append(", float4 input" + (i + 1).ToString());
            }

            signature.Append(")\n{\n");

            return signature.ToString();
        }

        public string getCallStr(BaseNode.NodeInput nodeInput, Dictionary<string,string> outputVariableNames)
        {
            var node = nodeInput.inputNode;
            int numInputs = node.inputs.Count;
            string callStr = "";

            //pre evaluation
            callStr += getPreEvaluation(nodeInput);

            for (int i = 0; i < numInputs; i++)
            {
                var input = node.inputs[i];
                if (null != input.inputNode)
                {
                    callStr += GeneratorFactory.getShaderGenerator(input.inputNode).getCallStr(input, outputVariableNames);
                }
            }

            string outputVariableName = "out" + node.GetType().Name + node.getNodeID() + "_" + nodeInput.outputIndex;
            bool declareOutputVariable = !outputVariableNames.ContainsKey(outputVariableName);
            if (declareOutputVariable)
            {
                outputVariableNames.Add(outputVariableName, outputVariableName);
                callStr += "fixed4 ";
            }

            callStr += outputVariableName + "= " + getFunctionName(nodeInput) + "( uv,0,0";

            for (int i = 0; i < numInputs; i++)
            {
                callStr += ", ";
                var input = node.inputs[i];
                if (null != input.inputNode)
                {
                    callStr += "out" + input.inputNode.GetType().Name + input.inputNode.getNodeID() + "_" + input.outputIndex;
                }
                else
                {
                    callStr += "float4(0,0,0,0)";
                }
            }

            callStr += " );\n";

            //post-evaluation
            callStr += getPostEvaluation(nodeInput);

            return callStr;
        }

    }
}