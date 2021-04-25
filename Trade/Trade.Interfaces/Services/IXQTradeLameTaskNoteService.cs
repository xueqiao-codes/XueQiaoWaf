using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoFoundation.BusinessResources.DataModels;

namespace XueQiaoWaf.Trade.Interfaces.Services
{
    /// <summary>
    /// 雪橇成交瘸腿任务笔记 service 协议
    /// </summary>
    public interface IXQTradeLameTaskNoteService
    {
        ObservableCollection<XQTradeLameTaskNote> TaskNoteItems { get; }
    }
}
