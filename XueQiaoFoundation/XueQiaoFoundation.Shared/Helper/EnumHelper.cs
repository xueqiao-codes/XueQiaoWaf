using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoFoundation.Shared.Helper
{
    public static class EnumHelper
    {
        /// <summary>
        /// 获取枚举的所有类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumType">枚举 Type</param>
        /// <param name="allTypes"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static void GetAllTypesForEnum<T>(Type enumType, out IEnumerable<T> outAllEnumValues) where T : struct
        {
            try
            {
                List<T> allEnumValues = null;
                var enumNames = Enum.GetNames(enumType);
                if (enumNames?.Any() == true)
                {
                    allEnumValues = new List<T>();
                    foreach (var enumName in enumNames)
                    {
                        if (Enum.TryParse(enumName, out T enumValue))
                        {
                            allEnumValues.Add(enumValue);
                        }
                    }
                }
                outAllEnumValues = allEnumValues?.ToArray();
            }
            catch (ArgumentNullException nullexp)
            {
                throw nullexp;
            }
            catch (ArgumentException arge)
            {
                throw arge;
            }
        }
    }
}
