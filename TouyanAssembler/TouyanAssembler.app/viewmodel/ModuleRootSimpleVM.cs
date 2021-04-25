using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using TouyanAssembler.app.view;
using XueQiaoFoundation.Shared.Model;

namespace TouyanAssembler.app.viewmodel
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class ModuleRootSimpleVM : ViewModel<ModuleRootSimpleView>
    {
        [ImportingConstructor]
        public ModuleRootSimpleVM(ModuleRootSimpleView view) : base(view)
        {
        }
        
        private ChromeWindowCaptionDataHolder embedInWindowCaptionDataHolder;
        public ChromeWindowCaptionDataHolder EmbedInWindowCaptionDataHolder
        {
            get { return embedInWindowCaptionDataHolder; }
            set { SetProperty(ref embedInWindowCaptionDataHolder, value); }
        }

        private object contentView;
        public object ContentView
        {
            get { return contentView; }
            set { SetProperty(ref contentView, value); }
        }
    }
}
