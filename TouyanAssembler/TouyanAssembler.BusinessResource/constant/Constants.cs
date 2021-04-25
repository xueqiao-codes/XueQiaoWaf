using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TouyanAssembler.BusinessResource.constant
{
    public static class Constants
    {
        /// <summary>
        /// 应用的公司名称
        /// </summary>
        public const string AppEnglishCompanyName = "XueQiao";

        public const string AppEnglishName = "XueQiaoTouyan";

        /// <summary>
        /// 当前应用的非漫游数据存放目录完整路径
        /// </summary>
        public readonly static string AppLocalDataDirectoryFullName
            = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData),
                AppEnglishCompanyName, AppEnglishName);

        /// <summary>
        /// 应用的安装目录
        /// </summary>
        public readonly static string AppSetupDirectoryPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;


        #region 应用的注册表相关

        // 应用的注册表基本路径 
        public static readonly string ApplicationRegistryKeyBasePath = $"SOFTWARE\\{AppEnglishCompanyName}\\{AppEnglishName}";

        // IsDevelopOpen(是否打开开发模式) 注册表 key
        public const string RegistryKey_IsDevelopOpen = "IsDevelopOpen";

        #endregion
    }
}
