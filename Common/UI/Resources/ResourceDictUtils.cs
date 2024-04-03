using System;
using System.Reflection;
using System.Windows;

namespace Common.UI.Resources
{
    public class ResourceDictUtils
    {
        private static ResourceDictionary dict = null;

        public static T GetResourceByName<T>(string name)
        {
            T resource = default;

            if (dict != null)
            {
                resource = (T)dict[name];
            }

            return resource;
        }
    }
}
