using System.ComponentModel.Composition;
using System.Globalization;
using System.Reflection;
using System.Waf.Presentation;
using System.Windows;
using System.Windows.Markup;
using XueQiaoFoundation.Shared.Interface;

namespace Manage.Presentations.Services
{
    [Export(typeof(IPresentationService))]
    internal class PresentationService : IPresentationService
    {
        public void Initialize()
        {
            ResourceHelper.AddToApplicationResources(Assembly.GetExecutingAssembly(), "Resources/ModuleResources.xaml");
        }
    }
}
