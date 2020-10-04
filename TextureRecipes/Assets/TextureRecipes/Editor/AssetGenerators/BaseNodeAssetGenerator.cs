using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace TextureRecipes
{
    public abstract class BaseNodeAssetGenerator
    {
        public abstract List<string> generateAssets(BaseNode node);
    }
}