using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;

namespace XueQiaoFoundation.BusinessResources.DataModels
{
    /// <summary>
    /// 组合标的类型数据的组合信息容器
    /// </summary>
    public class TargetCompose_ComposeDetail : Model, IComposeDetailDataModel
    {
        public TargetCompose_ComposeDetail(long composeId)
        {
            this.ComposeId = composeId;
        }

        public long ComposeId { get; private set; }
        
        private NativeComposeGraph basicComposeGraph;
        public NativeComposeGraph BasicComposeGraph
        {
            get { return basicComposeGraph; }
            set { SetProperty(ref basicComposeGraph, value); }
        }
        
        private ObservableCollection<ComposeLegDetail> detailLegs;
        public ObservableCollection<ComposeLegDetail> DetailLegs
        {
            get { return detailLegs; }
            set { SetProperty(ref detailLegs, value); }
        }
    }
}
