using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;

namespace Manage.Applications.DataModels
{
    /// <summary>
    /// 未分配成交项的相关分配数量概要 data model
    /// </summary>
    public class UATVolumeSummaryDM : Model, IUATVolumeSummaryDM
    {
        private int volume;
        /// <summary>
        /// 总数
        /// </summary>
        public int Volume
        {
            get { return volume; }
            set { SetProperty(ref volume, value); }
        }

        private int previewAssignVolume;
        /// <summary>
        /// 已预分配的数量
        /// </summary>
        public int PreviewAssignVolume
        {
            get { return previewAssignVolume; }
            set
            {
                var _val = Math.Max(0, Math.Min(this.Volume, value));
                SetProperty(ref previewAssignVolume, _val);
            }
        }

        private int unpreviewAssignVolume;
        /// <summary>
        /// 未预分配的数量
        /// </summary>
        public int UnpreviewAssignVolume
        {
            get { return unpreviewAssignVolume; }
            set
            {
                var _val = Math.Max(0, Math.Min(this.Volume, value));
                SetProperty(ref unpreviewAssignVolume, _val);
            }
        }
    }
}
