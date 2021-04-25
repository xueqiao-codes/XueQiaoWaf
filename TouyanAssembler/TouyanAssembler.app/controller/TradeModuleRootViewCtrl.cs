using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouyanAssembler.app.view;
using TouyanAssembler.app.viewmodel;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoFoundation.Shared.Model;

namespace TouyanAssembler.app.controller
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class TradeModuleRootViewCtrl : IController
    {
        private readonly ModuleRootSimpleVM rootVM;
        private readonly TradeFeatureAdView tradeFeatureAdView;

        [ImportingConstructor]
        public TradeModuleRootViewCtrl(
            ModuleRootSimpleVM rootVM,
            TradeFeatureAdView tradeFeatureAdView)
        {
            this.rootVM = rootVM;
            this.tradeFeatureAdView = tradeFeatureAdView;
        }

        /// <summary>
        /// 当嵌入到窗体头部时，窗体头部数据的 holder
        /// </summary>
        public ChromeWindowCaptionDataHolder EmbedInWindowCaptionDataHolder { get; set; }

        public object ContentView => rootVM.View;

        public void Initialize()
        {
            rootVM.ContentView = this.tradeFeatureAdView;
            rootVM.EmbedInWindowCaptionDataHolder = this.EmbedInWindowCaptionDataHolder;
        }

        public void Run()
        {

        }

        public void Shutdown()
        {

        }
    }
}
