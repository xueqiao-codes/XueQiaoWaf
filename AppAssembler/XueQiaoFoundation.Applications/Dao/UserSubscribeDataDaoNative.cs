using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Xml.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xueqiao.trade.hosting.terminal.ao;
using XueQiaoFoundation.Interfaces.Applications;
using XueQiaoFoundation.BusinessResources.Helpers;
using XueQiaoFoundation.BusinessResources.Models;
using lib.xqclient_base.logger;
using business_foundation_lib.xq_thriftlib_config;

namespace XueQiaoFoundation.Applications.Dao
{
    //[Export(typeof(IUserSubscribeDao)), PartCreationPolicy(CreationPolicy.Shared)]
    internal class UserSubscribeDataDaoNative : IUserSubscribeDataDao
    {
        private const string UserSubscribeDataXmlFileName = "subscribe_data.xml";
        
        //[ImportingConstructor]
        public UserSubscribeDataDaoNative()
        {

        }
        
        public UserSubscribeDataTree GetUserSubscribeData(LandingInfo landingInfo, out int? dataVersion)
        {
            dataVersion = 0;
            if (landingInfo == null) return null;
            var xmlPath = UserSubscribeDataFilePath(landingInfo);
            if (xmlPath == null) return null;
            try
            {
                // To read the file, create a FileStream.  
                var fileStream = new FileStream(xmlPath, FileMode.Open);
                var xmlSerializer = new XmlSerializer(typeof(UserSubscribeDataTree));

                var dataRoot = xmlSerializer.Deserialize(fileStream);
                return dataRoot as UserSubscribeDataTree;
            }
            catch (Exception e)
            {
                AppLog.Error($"Fail to GetUserSubscribeData, e:{e}");
                return null;
            }
        }

        public int? SaveSubscribeData(LandingInfo landingInfo, UserSubscribeDataTree subscribeTree, int dataVersion)
        {
            if (landingInfo == null) return null;
            if (subscribeTree == null) return null;
            var xmlPath = UserSubscribeDataFilePath(landingInfo);
            if (xmlPath == null) return null;
            try
            {
                // Insert code to set properties and fields of the object.  
                var xmlSerializer = new XmlSerializer(typeof(UserSubscribeDataTree));
                // To write to a file, create a StreamWriter object.  
                var myWriter = new StreamWriter(xmlPath);
                xmlSerializer.Serialize(myWriter, subscribeTree);
                myWriter.Close();
                return 0;
            }
            catch (Exception e)
            {
                AppLog.Error($"Fail to SaveSubscribeData, e: {e}");
                return null;
            }
        }

        private string UserSubscribeDataFilePath(LandingInfo landingInfo)
        {
            var dir = XueQiaoBusinessHelper.CreateApplicationUserDataLocalDirectory(landingInfo.MachineId, 
                landingInfo.SubUserId,
                XqThriftLibConfigurationManager.SharedInstance.ThriftHttpLibEnvironment.ToString());
            if (dir == null) return null;
            return Path.Combine(dir, UserSubscribeDataXmlFileName);
        }
    }
}
