using System.ComponentModel.Composition;
using System.Globalization;
using System.Reflection;
using System.Waf.Presentation;
using System.Windows;
using System.Windows.Markup;
using XueQiaoFoundation.Shared.Interface;

namespace Touyan.app.service
{
    [Export(typeof(IPresentationService))]
    internal class PresentationService : IPresentationService
    {
        public void Initialize()
        {
            ResourceHelper.AddToApplicationResources(Assembly.GetExecutingAssembly(), "resource/module_res.xaml");
        }
    }
}
