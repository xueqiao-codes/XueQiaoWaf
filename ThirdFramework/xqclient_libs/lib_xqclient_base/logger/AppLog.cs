using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace lib.xqclient_base.logger
{
    /// <summary>
    /// 应用日志记录类
    /// </summary>
    public class AppLog
    {
        private const string GlobalContext_Prop_LogFileDir = "LogFileDir";
        private const string GlobalContext_Prop_AppInstanceId = "AppInstanceId";
        private const string GlobalContext_Prop_InfoLogFileName = "InfoLogFileName";
        private const string GlobalContext_Prop_ErrorLogFileName = "ErrorLogFileName";

        private static bool _hasInit = false;
        private static readonly object appLogSharedInstanceLock = new object();
        private static readonly string appInstanceId = $"{(long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds}";
        private static string _logFileDir;
        
        /// <summary>
        /// 日志文件的存放目录
        /// </summary>
        public static string LogFileDir => _logFileDir;

        /// <summary>
        /// Info 级别的日志文件名称（不包含路径）
        /// </summary>
        public const string InfoLogFileName = "Info.log";

        /// <summary>
        /// Error 级别的日志文件名称（不包含路径）
        /// </summary>
        public const string ErrorLogFileName = "Error.log";

        /// <summary>
        /// 初始化方法
        /// </summary>
        /// <param name="logFileDir">日志文件存放目录</param>
        public static void Init(string logFileDir)
        {
            lock (appLogSharedInstanceLock)
            {
                if (_hasInit == true) return;
                if (string.IsNullOrWhiteSpace(logFileDir))
                {
                    throw new ArgumentException("`logFileDir` can't be whitespace or null.");
                }
                // configure context properties
                log4net.GlobalContext.Properties[GlobalContext_Prop_LogFileDir] = logFileDir;
                log4net.GlobalContext.Properties[GlobalContext_Prop_AppInstanceId] = appInstanceId;
                log4net.GlobalContext.Properties[GlobalContext_Prop_InfoLogFileName] = InfoLogFileName;
                log4net.GlobalContext.Properties[GlobalContext_Prop_ErrorLogFileName] = ErrorLogFileName;

                var executingAssembly = Assembly.GetExecutingAssembly();
                using (var configFileStream = executingAssembly.GetManifestResourceStream(typeof(AppLog), "AppLog.config"))
                {
                    log4net.Config.XmlConfigurator.Configure(configFileStream);
                }

                _logFileDir = logFileDir;
                _hasInit = true;
            }
        }
        
        public static void Fatal(object message, Exception exception = null)
        {
            GetLogger()?.Fatal(message, exception);
        }

        public static void Error(object message, Exception exception = null)
        {
            GetLogger()?.Error(message, exception);
        }

        public static void Warn(object message, Exception exception = null)
        {
            GetLogger()?.Warn(message, exception);
        }

        public static void Info(object message, Exception exception = null)
        {
            GetLogger()?.Info(message, exception);
        }

        public static void Debug(object message, Exception exception = null)
        {
            GetLogger()?.Debug(message, exception);
        }

        /// <summary>
        /// 获取日志器。在未 init 时，返回 null
        /// </summary>
        /// <returns></returns>
        private static ILog GetLogger()
        {
            ILog logger = null;
            lock (appLogSharedInstanceLock)
            {
                if (_hasInit)
                {
                    logger = LogManager.GetLogger("");
                }
            }
            return logger;
        }
    }
}
