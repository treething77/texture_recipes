using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace TextureRecipes
{
    public class RecolorNodeAssetGenerator : BaseNodeAssetGenerator
    {
        public override List<string> generateAssets(BaseNode node)
        {
            RecolorNode recolor = (RecolorNode)node;

            int dim3D = 62;
            int dim2D = dim3D+2;
            float oneOverDim = 1.0f / (float)(dim3D - 1);

            //when unrolling to the 2d array we will pad 1 pixel around each 3d dim3D*dim3D "layer"
            // .. so the 2d texture is (dim3D+2)*((dim3D+2)*(dim3D+2))

            var colorArray3D = new Color[dim3D, dim3D, dim3D];

            //precalculate the x,y,z position of each input color
            List<Vector3> mapColorPositions = new List<Vector3>();
            foreach (var mapping in recolor.colorRemapping)
            {
                var inC = mapping.InColor;
                var pos = new Vector3();
                pos.x = inC.r / oneOverDim;
                pos.y = inC.g / oneOverDim;
                pos.z = inC.b / oneOverDim;
                mapColorPositions.Add(pos);
            }

            for (int x = 0; x < dim3D; x++)
            {
                for (int y = 0; y < dim3D; y++)
                {
                    for (int z = 0; z < dim3D; z++)
                    {
                        Vector3 pixelPos = new Vector3(x, y, z);

                        //calculate RGB at this position
                        //use Vector3 so we can interpolate without Color clamping us to 0..255 or anything
                        Vector3 cVec = new Vector3(
                            x * oneOverDim,
                            y * oneOverDim,
                            z * oneOverDim
                        );

                        //calculate alpha value at this position for each color mapping
                        for(int mapIndex = 0;mapIndex < recolor.colorRemapping.Count;mapIndex++)
                        {
                            var mapping = recolor.colorRemapping[mapIndex];
                            var mapColor = mapping.OutColor;
                            var mapColorVec = new Vector3(mapColor.r, mapColor.g, mapColor.b);

                            var colorPos = mapColorPositions[mapIndex];
                            //"alpha" in the mapping is a measure of how wide an effect this mapping has
                            //0 - replace this exact mapping and nothing else
                            //1 - has an influence over the entire color cube

                            //at each pixel calculate an offset
                            Vector3 colorOffset = colorPos - pixelPos;

                            //multiply offset by alpha
                            float alphaT = Mathf.Clamp01(colorOffset.magnitude * oneOverDim);
                            float alpha = mapping.AlphaCurve.Evaluate(alphaT);

                            if (alpha > 0.0f)
                            {
                                //use to lerp the colors
                                cVec = Vector3.Lerp(cVec, mapColorVec, alpha);
                            }
                        }

                        //write out to 3d array
                        Color c = new Color(cVec.x, cVec.y, cVec.z, 1.0f);
                        colorArray3D[x, y, z] = c;
                    }//z
                }//y
            }//x

            //make sure even for mappings with alpha of 0 we at least poke a single value in at full strength at the closest position
            for (int mapIndex = 0; mapIndex < recolor.colorRemapping.Count; mapIndex++)
            {
                var mapping = recolor.colorRemapping[mapIndex];
                var mapColor = mapping.OutColor;
                var mapColorPos = mapColorPositions[mapIndex];

                colorArray3D[Mathf.FloorToInt(mapColorPos.x), 
                             Mathf.FloorToInt(mapColorPos.y), 
                             Mathf.FloorToInt(mapColorPos.z)] = mapColor;
            }

            int width2D = dim3D * dim2D;
            int height2D = dim2D;
            var colorArray2D = new Color[width2D * height2D];
            //flattened 2D array, where pixels are laid out left to right, bottom to top

            for (int x = 0; x < dim3D; x++)
            {
                for (int y = 0; y < dim3D; y++)
                {
                    for (int z = 0; z < dim3D; z++)
                    {
                        var c = colorArray3D[x, y, z];

                        int index2D = (x+1) + ((y+1) * width2D) + (z * dim2D);
                        colorArray2D[index2D] = c;
                    }
                }
            }

            for (int slice = 0; slice < dim3D; slice++)
            {
                //copy 1st column
                {
                    int x = 0;

                    for (int y = 0; y < dim3D; y++)
                    {
                        int dstIndex2D = (x) + ((y + 1) * width2D) + (slice * dim2D);
                        int srcIndex2D = (x + 1) + ((y + 1) * width2D) + (slice * dim2D);
                        colorArray2D[dstIndex2D] = colorArray2D[srcIndex2D];
                    }
                }
                //copy 2nd column
                {
                    int x = dim2D - 1;

                    for (int y = 0; y < dim3D; y++)
                    {
                        int dstIndex2D = (x) + ((y + 1) * width2D) + (slice * dim2D);
                        int srcIndex2D = (x - 1) + ((y + 1) * width2D) + (slice * dim2D);
                        colorArray2D[dstIndex2D] = colorArray2D[srcIndex2D];
                    }
                }

                //copy 1st row
                {
                    int y = 0;

                    for (int x = 0; x < dim3D; x++)
                    {
                        int dstIndex2D = (x + 1) + ((y) * width2D) + (slice * dim2D);
                        int srcIndex2D = (x + 1) + ((y + 1) * width2D) + (slice * dim2D);
                        colorArray2D[dstIndex2D] = colorArray2D[srcIndex2D];
                    }
                }

                //copy 1st row
                {
                    int y = dim2D - 1;

                    for (int x = 0; x < dim3D; x++)
                    {
                        int dstIndex2D = (x + 1) + ((y) * width2D) + (slice * dim2D);
                        int srcIndex2D = (x + 1) + ((y - 1) * width2D) + (slice * dim2D);
                        colorArray2D[dstIndex2D] = colorArray2D[srcIndex2D];
                    }
                }

                //4 corners
                {
                    int x = 0;
                    int y = 0;
                    int dstIndex2D = (x) + ((y) * width2D) + (slice * dim2D);
                    int srcIndex2D = (x + 1) + ((y) * width2D) + (slice * dim2D);
                    colorArray2D[dstIndex2D] = colorArray2D[srcIndex2D];

                    y = dim2D - 1;
                    dstIndex2D = (x) + ((y) * width2D) + (slice * dim2D);
                    srcIndex2D = (x + 1) + ((y) * width2D) + (slice * dim2D);
                    colorArray2D[dstIndex2D] = colorArray2D[srcIndex2D];

                    x = dim2D - 1;
                    y = dim2D - 1;
                    dstIndex2D = (x) + ((y) * width2D) + (slice * dim2D);
                    srcIndex2D = (x - 1) + ((y) * width2D) + (slice * dim2D);
                    colorArray2D[dstIndex2D] = colorArray2D[srcIndex2D];

                    y = 0;
                    dstIndex2D = (x) + ((y) * width2D) + (slice * dim2D);
                    srcIndex2D = (x - 1) + ((y) * width2D) + (slice * dim2D);
                    colorArray2D[dstIndex2D] = colorArray2D[srcIndex2D];
                }
            }

            var tex2D = new Texture2D(width2D, dim2D, TextureFormat.ARGB32, false);
     
            tex2D.SetPixels(colorArray2D);
            tex2D.Apply();

            string assetPathAndName = "/test.png";
            byte[] bytes = tex2D.EncodeToPNG();
            File.WriteAllBytes("Assets" + assetPathAndName, bytes);

            // set the texture reference in the RecolorNode
            AssetDatabase.ImportAsset("Assets" + assetPathAndName);
            Texture2D mapTexture = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets" + assetPathAndName, typeof(Texture2D));
            recolor.mapColorTexture = mapTexture;

            List<string> textureAssetPaths = new List<string>();
            return textureAssetPaths;
        }
    }
}
