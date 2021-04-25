using System;
using System.IO;
using System.Xml.Serialization;
using xueqiao.trade.hosting.terminal.ao;
using XueQiaoFoundation.Interfaces.Applications;
using XueQiaoFoundation.BusinessResources.Helpers;
using XueQiaoFoundation.BusinessResources.Models;
using lib.xqclient_base.logger;
using business_foundation_lib.xq_thriftlib_config;

namespace XueQiaoFoundation.Applications.Dao
{
    //[Export(typeof(IUserWorkspaceDao)), PartCreationPolicy(CreationPolicy.Shared)]
    internal class UserWorkspaceDaoNative : IUserWorkspaceDataDao
    {
        private const string UserWorkspaceDataXmlFileName = "workspaces.xml";
        

        //[ImportingConstructor]
        public UserWorkspaceDaoNative()
        {
            
        }

        public WorkspaceWindowTree GetUserWorkspaceData(LandingInfo landingInfo, out int? dataVersion)
        {
            dataVersion = 0;
            if (landingInfo == null) return null;
            var xmlPath = UserWorkspaceDataFilePath(landingInfo);
            if (xmlPath == null) return null;
            try
            {
                // To read the file, create a FileStream.  
                var fileStream = new FileStream(xmlPath, FileMode.Open);
                var xmlSerializer = new XmlSerializer(typeof(WorkspaceWindowTree));

                var dataRoot = xmlSerializer.Deserialize(fileStream);
                return dataRoot as WorkspaceWindowTree;
            }
            catch (Exception e)
            {
                AppLog.Error($"Fail to GetUserWorkspaceData, e:{e}");
                return null;
            }
        }

        public int? SaveUserWorkspaceData(LandingInfo landingInfo, WorkspaceWindowTree userWorkspaceDataTree, int dataVersion)
        {
            if (landingInfo == null) return null;
            if (userWorkspaceDataTree == null) return null;
            var xmlPath = UserWorkspaceDataFilePath(landingInfo);
            if (xmlPath == null) return null;
            try
            {
                // Insert code to set properties and fields of the object.  
                var xmlSerializer = new XmlSerializer(typeof(WorkspaceWindowTree));
                // To write to a file, create a StreamWriter object.  
                var myWriter = new StreamWriter(xmlPath);
                xmlSerializer.Serialize(myWriter, userWorkspaceDataTree);
                myWriter.Close();
                return 0;
            }
            catch (Exception e)
            {
                AppLog.Error($"Fail to SaveUserWorkspaceData, e: {e}");
                return null;
            }
        }

        private string UserWorkspaceDataFilePath(LandingInfo landingInfo)
        {
            var dir = XueQiaoBusinessHelper.CreateApplicationUserDataLocalDirectory(landingInfo.MachineId, 
                landingInfo.SubUserId, XqThriftLibConfigurationManager.SharedInstance.ThriftHttpLibEnvironment.ToString());
            if (dir == null) return null;
            return Path.Combine(dir, UserWorkspaceDataXmlFileName);
        }
    }
}
