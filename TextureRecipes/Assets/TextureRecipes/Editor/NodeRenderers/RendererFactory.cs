using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace TextureRecipes
{
    public class RendererFactory
    {
        static Dictionary<Type, Type> rendererTypeMap = new Dictionary<Type, Type>();
        static Dictionary<Type, BaseNodeRenderer> rendererInstanceMap = new Dictionary<Type, BaseNodeRenderer>();

        internal static BaseNodeRenderer getRenderer(BaseNode node)
        {
            var nodeType = node.GetType();
            Type rendererType = getRendererType(nodeType);
            if (rendererType == null)
            {
                return null;
            }
            BaseNodeRenderer renderer;
            if (!rendererInstanceMap.TryGetValue(nodeType, out renderer))
            {
                renderer = (BaseNodeRenderer)Activator.CreateInstance(rendererType);
                rendererInstanceMap[nodeType] = renderer;
            }
            return renderer;
        }

        public static Type getRendererType(Type nodeType)
        {
            return rendererTypeMap[nodeType];
        }

        public static void createMap()
        {
            rendererTypeMap.Clear();

            var allRendererTypes = Assembly.GetAssembly(typeof(BaseNodeRenderer)).GetTypes().Where(
                                                    myType => myType.IsClass
                                                    && !myType.IsAbstract
                                                    && myType.IsSubclassOf(typeof(BaseNodeRenderer)));

            foreach (Type nodeType in Assembly.GetAssembly(typeof(BaseNode)).GetTypes().Where(
                                                    myType => myType.IsClass
                                                    && !myType.IsAbstract
                                                    && myType.IsSubclassOf(typeof(BaseNode))))
            {
                Type nodeRendererType = typeof(DefaultNodeRenderer);

                foreach (var rendererType in allRendererTypes)
                {
                    if (rendererType.Name == (nodeType.Name + "Renderer"))
                    {
                        nodeRendererType = rendererType;
                        break;
                    }
                }
                rendererTypeMap.Add(nodeType, nodeRendererType);
            }
        }

    }
}
