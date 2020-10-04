using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace TextureRecipes
{
    public static class GeneratorFactory
    {
        static Dictionary<Type, Type> shaderGeneratorMap = new Dictionary<Type, Type>();
        static Dictionary<Type, Type> assetGeneratorMap = new Dictionary<Type, Type>();

        internal static BaseNodeGenerator getShaderGenerator(BaseNode node)
        {
            return (BaseNodeGenerator) getNodeGenerator(node, shaderGeneratorMap);
        }

        internal static BaseNodeAssetGenerator getAssetGenerator(BaseNode node)
        {
            return (BaseNodeAssetGenerator) getNodeGenerator(node, assetGeneratorMap);
        }

        public static void createMappings()
        {
            generateTypeMapping(shaderGeneratorMap, typeof(BaseNodeGenerator), "Generator");
            generateTypeMapping(assetGeneratorMap, typeof(BaseNodeAssetGenerator), "AssetGenerator");
        }

        private static object getNodeGenerator(BaseNode node, Dictionary<Type, Type> generatorMap)
        {
            Type generatorType;
            if (!generatorMap.TryGetValue(node.GetType(), out generatorType))
            { 
                return null;
            }

            var generator = Activator.CreateInstance(generatorType);
            return generator;
        }

        private static void generateTypeMapping(Dictionary<Type, Type> typeMap, Type baseType, string nameSuffix)
        {
            typeMap.Clear();

            var allGeneratorTypes = Assembly.GetAssembly(baseType).GetTypes().Where(
                                                    myType => myType.IsClass
                                                    && !myType.IsAbstract
                                                    && myType.IsSubclassOf(baseType));

            foreach (Type nodeType in Assembly.GetAssembly(typeof(RecipeLayerBase)).GetTypes().Where(
                                                    myType => myType.IsClass
                                                    && !myType.IsAbstract
                                                    && myType.IsSubclassOf(typeof(BaseNode))))
            {
                if (nodeType == typeof(RootNode))
                {
                    continue;
                }

                //is there a corresponding generator class?
                Type nodeGeneratorType = null;

                foreach (var generatorType in allGeneratorTypes)
                {
                    if (generatorType.Name == (nodeType.Name + nameSuffix))
                    {
                        nodeGeneratorType = generatorType;
                        break;
                    }
                }
                if (nodeGeneratorType == null)
                {
                    //Debug.Log("No generator found for " + nodeType.Name);
                }
                else
                {
                    typeMap.Add(nodeType, nodeGeneratorType);
                }
            }
        }
    }
}
