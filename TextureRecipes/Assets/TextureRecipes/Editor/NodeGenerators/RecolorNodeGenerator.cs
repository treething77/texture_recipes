using System;

namespace TextureRecipes
{
    public class RecolorNodeGenerator : BaseNodeGenerator
    {
        public override string getFunctionBody(BaseNode.NodeInput nodeInput)
        {
            var node = nodeInput.inputNode;
            RecolorNode recolor = (RecolorNode)node;

            string shaderStr =
                    "   float3 inC = input1.rgb;\n" +
                    "   float numSlices = 62.0;\n" +
                    "       float slicePixels = 64.0;\n" +
                    "       float sliceSize = 1.0 / numSlices;              // space of 1 slice\n" +
                    "       float slicePixelSize = sliceSize / slicePixels; // space of 1 pixel\n" +
                    "       float sliceInnerSize = slicePixelSize * (slicePixels - 1.0); //space of size pixels\n" +
                    "       float zSlice0 = min(floor(inC.b * numSlices), numSlices - 1.0);\n" +
                    "       float xOffset = slicePixelSize * 0.5 + inC.r * sliceInnerSize;\n" +
                    "       float s0 = xOffset + (zSlice0 * sliceSize);\n" +

                    "   float2 mapUV;\n" +
                    "   mapUV.x = s0;\n" +
                    "   mapUV.y = inC.g;// (inC.g * (62.0/64.0)) + (1.0/64.0);\n" +

                    "   float4 col1 = tex2D(mapTexture" + node.getNodeID() + ", mapUV);\n";

            if (recolor.InterpolateSliceSamples)
            {
                shaderStr += 
                    "   float zSlice1 = min(zSlice0 + 1.0, numSlices - 1.0);\n" +
                    "   float s1 = xOffset + (zSlice1 * sliceSize);\n" +
                    "   float fr = frac(inC.b * numSlices);\n" +

                    "   mapUV.x = s1;\n" +
                    "   float4 col2 = tex2D(mapTexture" + node.getNodeID() + ", mapUV);\n" +
                    "   float4 c = lerp(col1, col2, fr);\n" +
                    "   return c;\n";
            }
            else
            {
                shaderStr += "   return col1;\n";
            }

            return shaderStr;
        }

        public override string getInputs(BaseNode.NodeInput nodeInput)
        {
            var node = nodeInput.inputNode;
            string inputStr = "sampler2D mapTexture" + node.getNodeID() + ";\n";
            return inputStr;
        }

        public override string getProperties(BaseNode.NodeInput nodeInput)
        {
            var node = nodeInput.inputNode;
            string propStr = "mapTexture" + node.getNodeID() + "(\"texture\", 2D) = \"\" {}";
            return propStr;
        }
    }
}
