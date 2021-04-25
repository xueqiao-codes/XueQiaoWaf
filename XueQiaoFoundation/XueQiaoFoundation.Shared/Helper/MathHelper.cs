using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoFoundation.Shared.Helper
{
    public class MathHelper
    {
        /// <summary>
        /// 精确化double数值。根据 variationInterval(变化间隔) 来精确化
        /// </summary>
        /// <param name="source">原数值</param>
        /// <param name="variationInterval">变化间隔</param>
        /// <returns></returns>
        public static double MakeValuePrecise(double source, double variationInterval)
        {
            if (Math.Abs(variationInterval) > 0)
            {
                var preciseDecimalCount = GetDecimalCount(new Decimal(variationInterval));
                return MakeValuePrecise(Math.Round(source / variationInterval) * variationInterval, preciseDecimalCount);
            }
            return source;
        }

        /// <summary>
        /// 精确化double数值。根据给定小数点位数来精确化
        /// </summary>
        /// <param name="source"></param>
        /// <param name="preciseDecimalCount">小数点位数</param>
        /// <returns></returns>
        public static double MakeValuePrecise(double source, int preciseDecimalCount)
        {
            var resultPrice = Math.Round(source, preciseDecimalCount);
            return resultPrice;
        }

        /// <summary>
        /// 获取小数位数
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static int GetDecimalCount(decimal src)
        {
            var precision = 0;
            while (src * (decimal)Math.Pow(10, precision) !=
                     Math.Round(src * (decimal)Math.Pow(10, precision)))
                precision++;
            return precision;
        }

        /// <summary>
        /// 获取小数的小数点前后片段
        /// </summary>
        /// <param name="decimalNum"></param>
        /// <returns>Tuple, arg0:小数点前的数字片段，arg1:小数点后的数字片段</returns>
        public static Tuple<string, string> GetDecimalPieces(decimal decimalNum)
        {
            string strDecimalNum = $"{decimalNum}";
            var results = strDecimalNum.Split(new char[] { '.' }, 2, StringSplitOptions.RemoveEmptyEntries);
            if (results.Length > 0)
            {
                var beforePiece = results[0];
                var afterPiece = results.Length > 1 ? results[1] : "";
                var tup = new Tuple<string, string>(beforePiece, afterPiece);
                return tup;
            }
            return new Tuple<string, string>("", "");
        }

        /// <summary>
        /// 比较两个 double 类型的数值是否相等
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <param name="accuracyTolerance"></param>
        /// <returns></returns>
        public static bool DoubleAreEqual(double d1, double d2, double accuracyTolerance)
        {
            return Math.Abs(d1 - d2) < accuracyTolerance;
        }

        /// <summary>
        /// 比较两个 float 类型的数值是否相等
        /// </summary>
        /// <param name="f1"></param>
        /// <param name="f2"></param>
        /// <param name="accuracyTolerance"></param>
        /// <returns></returns>
        public static bool FloatAreEqual(float f1, float f2, double accuracyTolerance)
        {
            return Math.Abs(f1 - f2) < accuracyTolerance;
        }
    }
}
