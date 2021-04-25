using lib.xqclient_base.logger;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouyanAssembler.BusinessResource.constant;

namespace TouyanAssembler.BusinessResource.helper
{
    public static class BusinessHelper
    {
        /// <summary>
        /// 获取应用注册表某个键的值
        /// </summary>
        /// <returns></returns>
        public static object GetApplicationRegistryKey(string keyName)
        {
            object r = null;
            try
            {
                using (var regKey = Registry.CurrentUser.OpenSubKey(Constants.ApplicationRegistryKeyBasePath))
                {
                    if (regKey != null)
                    {
                        r = regKey.GetValue(keyName);
                    }
                }
            }
            catch (Exception e)
            {
                AppLog.Error("Failed to GetApplicationRegistryKey.", e);
            }
            return r;
        }

        /// <summary>
        /// 设置应用注册表某个键的值
        /// </summary>
        /// <param name="keyName">键</param>
        /// <param name="value">值</param>
        public static void SetApplicationRegistryKey(string keyName, object value)
        {
            try
            {
                using (var regKey = Registry.CurrentUser.CreateSubKey(Constants.ApplicationRegistryKeyBasePath))
                {
                    if (regKey != null)
                    {
                        regKey.SetValue(keyName, value);
                    }
                }
            }
            catch (Exception e)
            {
                AppLog.Error("Failed to SetApplicationRegistryKey.", e);
            }
        }
    }
}
