using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SR = System.Reflection;

namespace UVNF.Editor.Helpers
{
    public static class AssemblyHelpers
    {
        public static T[] GetEnumerableOfType<T>(params object[] constructorArgs) where T : class
        {
            var whatAssembly = SR.Assembly.GetAssembly(typeof(T));
            var types = SR.Assembly.GetAssembly(typeof(T)).GetTypes();

            List<T> objects = new List<T>();
            foreach (Type type in SR.Assembly.GetAssembly(typeof(T)).GetTypes()
                .Where(x =>
                {
                    return x.IsClass && !x.IsAbstract && x.IsSubclassOf(typeof(T));
                }))
            {
                objects.Add((T)Activator.CreateInstance(type, constructorArgs));
            }
            return objects.ToArray();
        }

        public static T[] GetEnumerableOfInterfaceType<T>(params object[] constructorArgs) where T : class
        {
            var interfaceType = typeof(T);
            List<T> objects = new List<T>();
            foreach (Type type in AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                 .Where(x => interfaceType.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract))
            {
                objects.Add((T)Activator.CreateInstance(type, constructorArgs));
            }
            return objects.ToArray();
        }

        public static string[] GetStringsOfType<T>(params object[] constructorArgs) where T : class
        {
            List<string> objects = new List<string>();
            foreach (Type type in SR.Assembly.GetAssembly(typeof(T)).GetTypes()
                .Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(typeof(T))))
            {
                objects.Add(type.Name);
            }
            return objects.ToArray();
        }

#if UNITY_EDITOR
        public static object GetDefaultValue(this Type type)
        {
            // Validate parameters.
            if (type == null)
                throw new ArgumentNullException("type");

            // We want an Func<object> which returns the default.
            // Create that expression here.
            Expression<Func<object>> e = Expression.Lambda<Func<object>>(
                // Have to convert to object.
                Expression.Convert(
                    // The default value, always get what the *code* tells us.
                    Expression.Default(type), typeof(object)
                )
            );

            // Compile and return the value.
            return e.Compile()();
        }

        public static T[] FindAssetsByType<T>() where T : UnityEngine.Object
        {
            List<T> assets = new List<T>();
            string[] guids = UnityEditor.AssetDatabase.FindAssets($"t:{typeof(T)}");

            for (int i = 0; i < guids.Length; i++)
            {
                string assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[i]);
                T asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (asset != null)
                    assets.Add(asset);
            }

            return assets.ToArray();
        }
#endif
    }
}