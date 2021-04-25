using lib.xqclient_base.thriftapi_mediation.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using xueqiao.trade.hosting.proxy;

namespace business_foundation_lib.helper
{
    public static class FoundationHelper
    {
        /// <summary>
        /// 格式化接口返回错误信息
        /// </summary>
        /// <param name="resp"></param>
        /// <param name="prefixSummaryText">信息前缀</param>
        /// <param name="showBusinessErrCodeIfExist">当存在业务错误码时是否显示业务员错误码</param>
        /// <returns></returns>
        public static string FormatResponseDisplayErrorMsg(IInterfaceInteractResponse resp, string prefixSummaryText = null, bool showBusinessErrCodeIfExist = false)
        {
            var sb = new StringBuilder();

            string statusCodeStr = null;
            string statusMsgStr = null;
            if (resp != null)
            {
                statusMsgStr = resp.CustomParsedExceptionResult?.BusinessErrorMessage;
                if (resp.CustomParsedExceptionResult?.BusinessErrorCode != null && showBusinessErrCodeIfExist)
                {
                    statusCodeStr = $"b_err:{resp.CustomParsedExceptionResult?.BusinessErrorCode}";
                }
                else if (resp.HttpResponseStatusCode != null)
                {
                    statusCodeStr = $"http_sta:{resp.HttpResponseStatusCode}";
                }
            }

            if (!string.IsNullOrEmpty(statusMsgStr))
                sb.Append(statusMsgStr);

            if (!string.IsNullOrEmpty(statusCodeStr))
            {
                if (sb.Length > 0)
                    sb.Append("\n");
                sb.Append($"({statusCodeStr})");
            }

            if (!string.IsNullOrEmpty(prefixSummaryText))
            {
                sb.Insert(0, prefixSummaryText);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 组合应用更新描述
        /// </summary>
        /// <param name="updateNotes"></param>
        /// <returns></returns>
        public static string JoinAppVersionUpdateNotes(IEnumerable<string> updateNotes)
        {
            if (updateNotes == null) return null;
            var notes = updateNotes.ToArray();
            var notesCount = notes.Count();

            var descSb = new StringBuilder();
            for (var i = 0; i < notesCount; i++)
            {
                descSb.Append($"{i + 1}.{notes[i]}");
                if (i != notesCount - 1)
                    descSb.Append("\n");
            }

            return descSb.ToString();
        }

        /// <summary>
        /// 启动应用的安装程序
        /// </summary>
        /// <param name="appInstallFilePath">安装包路径</param>
        public static void StartupAppPackageInstallProcess(string appInstallPackagePath)
        {
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = appInstallPackagePath;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
        }

        /// <summary>
        /// 根据当前进程位数获取应用安装包的下载地址
        /// </summary>
        /// <param name="updateInfo">应用更新信息</param>
        /// <returns></returns>
        public static string GetPackageDownloadUrlDependOnCurrentProcessBitMode(AppVersion updateInfo)
        {
            string downloadUrl = null;
            if (Environment.Is64BitProcess)
            {
                downloadUrl = updateInfo.DownloadUrlX64;
            }

            if (string.IsNullOrEmpty(downloadUrl))
            {
                downloadUrl = updateInfo.DownloadUrlX32;
            }

            return downloadUrl;
        }

        public static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        public static string ConvertToUnsecureString(SecureString secureStr)
        {
            if (secureStr == null)
            {
                return string.Empty;
            }

            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureStr);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
    }
}
