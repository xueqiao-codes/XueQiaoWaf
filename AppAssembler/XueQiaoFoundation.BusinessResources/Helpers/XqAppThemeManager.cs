using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using XueQiaoFoundation.BusinessResources.Models;
using XueQiaoFoundation.Shared.Helper;

namespace XueQiaoFoundation.BusinessResources.Helpers
{
    public delegate Uri GetThemeResourceDictionaryUri(XqAppThemeType themeType);

    public class XqAppThemeManager
    {
        public readonly GetThemeResourceDictionaryUri ThemeResourceDictionaryUriGetter;

        public XqAppThemeManager(GetThemeResourceDictionaryUri themeRDUriGetter)
        {
            if (themeRDUriGetter == null) throw new ArgumentNullException("themeRDUriGetter");
            ThemeResourceDictionaryUriGetter = themeRDUriGetter;
        }

        public void ApplyTheme(Application application, XqAppThemeType theme)
        {
            EnumHelper.GetAllTypesForEnum(theme.GetType(), out IEnumerable<XqAppThemeType> allThemes);
            var otherThemes = allThemes.Where(i => i != theme);

            Console.WriteLine("Origin application merged resources:");
            foreach (var md in application.Resources.MergedDictionaries)
            {
                Console.WriteLine($"md:{md}, Source uri:{md.Source}");
            }

            var newThemeRDUri = ThemeResourceDictionaryUriGetter(theme);

            Console.WriteLine($"newThemeRDUri:{newThemeRDUri}");

            var existNewTheme = application.Resources.MergedDictionaries.Any(i => AreResourceDictionarySourcesEqual(i.Source, newThemeRDUri));
            if (!existNewTheme)
            {
                // 添加新的主题
                application.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = newThemeRDUri });
            }

            // 删除旧的主题
            foreach (var other in otherThemes)
            {
                var _uri = ThemeResourceDictionaryUriGetter(other);
                var _rmRDs = application.Resources.MergedDictionaries.Where(i => AreResourceDictionarySourcesEqual(i.Source, _uri)).ToArray();
                foreach (var _rmRD in _rmRDs)
                {
                    application.Resources.MergedDictionaries.Remove(_rmRD);
                }
            }
        }
        
        private static bool AreResourceDictionarySourcesEqual(Uri first, Uri second)
        {
            return Uri.Compare(first, second,
                 UriComponents.Host | UriComponents.Path, UriFormat.SafeUnescaped, StringComparison.OrdinalIgnoreCase) == 0;
        }
    }
}
