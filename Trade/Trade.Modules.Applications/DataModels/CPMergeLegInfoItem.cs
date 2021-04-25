using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;
using XueQiaoFoundation.BusinessResources.DataModels;

namespace XueQiaoWaf.Trade.Modules.Applications.DataModels
{
    /// <summary>
    /// 组合持仓合并的腿信息项 data model
    /// </summary>
    public class CPMergeLegInfoItem : Model
    {
        public CPMergeLegInfoItem(ComposeLegDetail legDetail, ClientTradeDirection? legPositionDirForMerge)
        {
            this.LegDetail = legDetail;
            this.LegPositionDirForMerge = legPositionDirForMerge;
        }

        /// <summary>
        /// 腿详情
        /// </summary>
        public ComposeLegDetail LegDetail { get; private set; }

        /// <summary>
        /// 用于合并的腿持仓方向
        /// </summary>
        public ClientTradeDirection? LegPositionDirForMerge { get; private set; }

        private int demandVolume;
        /// <summary>
        /// 总需求量
        /// </summary>
        public int DemandVolume
        {
            get { return demandVolume; }
            set { SetProperty(ref demandVolume, value); }
        }

        private int currentVolume;
        /// <summary>
        /// 当前选择的量
        /// </summary>
        public int CurrentVolume
        {
            get { return currentVolume; }
            set { SetProperty(ref currentVolume, value); }
        }

        private double? currentAvgPrice;
        /// <summary>
        /// 当前选择项的均价
        /// </summary>
        public double? CurrentAvgPrice
        {
            get { return currentAvgPrice; }
            set { SetProperty(ref currentAvgPrice, value); }
        }
    }
}
