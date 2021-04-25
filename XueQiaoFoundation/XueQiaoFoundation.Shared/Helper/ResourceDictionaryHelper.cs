using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace XueQiaoFoundation.Shared.Helper
{
    /// <summary>
    /// 资源字典 helper
    /// </summary>
    public static class ResourceDictionaryHelper
    {
        /// <summary>
        /// Find a resource define in resource dictionary
        /// </summary>
        /// <param name="resourceKey">The key of resource.</param>
        /// <param name="parentDictionary">The root dictionary to start searching at. Null means using Application.Current.Resources</param>
        /// <returns></returns>
        public static object FindResource(string resourceKey, ResourceDictionary parentDictionary = null)
        {
            if (parentDictionary == null)
                parentDictionary = Application.Current.Resources;

            return parentDictionary[resourceKey];
        }

        /// <summary>
        /// Check if ResourceDictionary's Uri are equal.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static bool AreResourceDictionarySourcesEqual(Uri first, Uri second)
        {
            return Uri.Compare(first, second,
                 UriComponents.Host | UriComponents.Path, UriFormat.SafeUnescaped, StringComparison.OrdinalIgnoreCase) == 0;
        }
    }
}
