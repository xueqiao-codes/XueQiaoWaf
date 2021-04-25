using Manage.Applications.DataModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Applications.Services
{
    [Export, PartCreationPolicy(CreationPolicy.Shared)]
    public class PositionVerifyTradeInputItemsService
    {
        public PositionVerifyTradeInputItemsService()
        {
            XqPreviewTradeItems = new ObservableCollection<PositionVerifyTradeInputDM>();
        }

        public ObservableCollection<PositionVerifyTradeInputDM> XqPreviewTradeItems { get; private set; }
    }
}
