using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoWaf.Trade.Interfaces.Services;

namespace XueQiaoWaf.Trade.Modules.Applications.Services
{
    [Export, Export(typeof(IXQTradeLameTaskNoteService)), PartCreationPolicy(CreationPolicy.Shared)]
    public class XQTradeLameTaskNoteService : IXQTradeLameTaskNoteService
    {
        public XQTradeLameTaskNoteService()
        {
            TaskNoteItems = new ObservableCollection<XQTradeLameTaskNote>();
        }

        public ObservableCollection<XQTradeLameTaskNote> TaskNoteItems { get; private set; }
    }
}
