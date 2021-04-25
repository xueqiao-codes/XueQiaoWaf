using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Applications.DataModels
{
    public interface IUATVolumeSummaryDM : INotifyPropertyChanged
    {
        /// <summary>
        /// 总数
        /// </summary>
        int Volume { get; set; }

        /// <summary>
        /// 已预分配的数量
        /// </summary>
        int PreviewAssignVolume { get; set; }

        /// <summary>
        /// 未预分配的数量
        /// </summary>
        int UnpreviewAssignVolume { get; set; }
    }
}
