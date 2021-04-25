using lib.xqclient_base.logger;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using XueQiaoFoundation.Applications.Resources;
using XueQiaoFoundation.BusinessResources.Models;
using XueQiaoFoundation.Interfaces.Applications;

namespace XueQiaoFoundation.Applications.Controllers
{
    [Export(typeof(ICurrencyUnitsService)), Export(typeof(IXueQiaoFoundationSingletonController)), PartCreationPolicy(CreationPolicy.Shared)]
    internal class CurrencyUnitsController : ICurrencyUnitsService, IXueQiaoFoundationSingletonController
    {
        private CurrencyUnitInfo[] currencyUnitInfos;

        [ImportingConstructor]
        public CurrencyUnitsController()
        {
            InitCurrencyUnitInfosFromFile();
        }

        public void Shutdown()
        {
            currencyUnitInfos = null;
        }

        #region ICurrencyUnitsService

        public IEnumerable<CurrencyUnitInfo> SupportCurrencyUnitInfos => currencyUnitInfos?.ToArray();

        public CurrencyUnitInfo GetCurrencyUnitInfo(string currency)
        {
            return currencyUnitInfos?.FirstOrDefault(i => i.Currency == currency);
        }

        public string GetCurrencyUnitName(string currency, double currencyChargeUnit)
        {
            var unitInfo = GetCurrencyUnitInfo(currency);
            if (unitInfo == null) return null;
            if (currencyChargeUnit == 1)
                return unitInfo.UnitName;
            else if (currencyChargeUnit == 0.1)
                return unitInfo.UnitName0_1;
            else if (currencyChargeUnit == 0.01)
                return unitInfo.UnitName0_0_1;
            else if (currencyChargeUnit == 0.001)
                return unitInfo.UnitName0_0_0_1;
            else
                return null;
        }

        #endregion

        private void InitCurrencyUnitInfosFromFile()
        {
            var assembly = typeof(CurrencyUnitsController).Assembly;
            var fileName = assembly.GetManifestResourceNames().FirstOrDefault(i => i.EndsWith(".CurrencyUnits.xml"));
            if (fileName == null)
            {
                AppLog.Error($"Failed to get `CurrencyUnits.xml` from Assembly:{assembly.FullName}");
                return;
            }
            var fileStream = assembly.GetManifestResourceStream(fileName);

            try
            {
                var serializer = new XmlSerializer(typeof(CurrencyUnitsTree));
                var currencyUnitsTree = (CurrencyUnitsTree)serializer.Deserialize(fileStream);
                this.currencyUnitInfos = currencyUnitsTree?.CurrencyUnitItems;
            }
            catch (Exception e)
            {
                AppLog.Error($"Failed to deserialize from file:{fileName}, e:{e}");
                return;
            }
        }
    }
}
