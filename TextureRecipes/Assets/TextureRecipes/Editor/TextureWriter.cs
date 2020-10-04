using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace TextureRecipes
{
    public static class TextureWriter
    {
        public static void WriteRenderBufferToTextureFile(int texW, int texH, Object baseAsset)
        {
            Texture2D outTex = new Texture2D(texW, texH);
            outTex.ReadPixels(new Rect(0, 0, texW, texH), 0, 0);

            string basePath = AssetDatabase.GetAssetPath(baseAsset);
            if (basePath.Length == 0)
            {
                basePath = "Assets";
            }
            else if (Path.GetExtension(basePath) != "")
            {
                basePath = basePath.Replace(Path.GetFileName(basePath), "");
            }

            //TODO: unique name?
            string assetPathAndName = basePath + "/New Texture.png";
            byte[] bytes = outTex.EncodeToPNG();
            File.WriteAllBytes(Application.dataPath + "/../" + assetPathAndName, bytes);

            AssetDatabase.Refresh();
        }
    }
}
