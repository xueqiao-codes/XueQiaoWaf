using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoFoundation.Shared.Helper
{
    public class CommonConvert
    {
        /// <summary>
        /// GB2312转换成UTF8
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string GB2312_TO_UTF8(string text)
        {
            //声明字符集   
            System.Text.Encoding utf8, gb2312;
            //gb2312   
            gb2312 = System.Text.Encoding.GetEncoding("gb2312");
            //utf8   
            utf8 = System.Text.Encoding.GetEncoding("utf-8");
            byte[] gb;
            gb = gb2312.GetBytes(text);
            gb = System.Text.Encoding.Convert(gb2312, utf8, gb);
            //返回转换后的字符   
            return utf8.GetString(gb);
        }

        /// <summary>
        /// UTF8转换成GB2312
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string UTF8_TO_GB2312(string text)
        {
            //声明字符集   
            System.Text.Encoding utf8, gb2312;
            //utf8   
            utf8 = System.Text.Encoding.GetEncoding("utf-8");
            //gb2312   
            gb2312 = System.Text.Encoding.GetEncoding("gb2312");
            byte[] utf;
            utf = utf8.GetBytes(text);
            utf = System.Text.Encoding.Convert(utf8, gb2312, utf);
            //返回转换后的字符   
            return gb2312.GetString(utf);
        }

        // 转换接收到的字符串
        public static string UTF8_To_Unicode(string recvStr)
        {
            byte[] tempStr = Encoding.UTF8.GetBytes(recvStr);
            byte[] tempDef = Encoding.Convert(Encoding.UTF8, Encoding.Default, tempStr);
            string msgBody = Encoding.Default.GetString(tempDef);
            return msgBody;
        }

        // 转换要发送的字符数组
        /*
        public static byte[] UnicodeToUTF8(string sendStr)
        {
            string tempStr = Encoding.UTF8.GetString(sendStr.ToCharArray());
            byte[] msgBody = Encoding.UTF8.GetBytes(tempUTF8);
            return msgBody;
        }
        */

        public static byte[] ObjectToByteArray(Object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }
        

        private static string[] Letters = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        public static string IndexUnder23ToLetter(int index, bool letterToUpperCase, string prefix, string suffix)
        {
            if (index < Letters.Length)
            {
                var letter = Letters[index];
                if (!letterToUpperCase) { letter = letter.ToLower(); }

                var sb = new StringBuilder();
                if (!string.IsNullOrEmpty(prefix)) sb.Append(prefix);
                sb.Append(letter);
                if (!string.IsNullOrEmpty(suffix)) sb.Append(suffix);

                return sb.ToString();
            }
            return null;
        }

    }
}
