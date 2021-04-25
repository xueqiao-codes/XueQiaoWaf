using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoFoundation.BusinessResources.Models
{
    /// <summary>
    /// 列表列信息
    /// </summary>
    public class ListColumnInfo : IEquatable<ListColumnInfo>, ICloneable
    {
        /// <summary>
        /// 内容对齐方式
        /// </summary>
        public int ContentAlignment { get; set; }

        /// <summary>
        /// 列 code
        /// </summary>
        public int ColumnCode { get; set; }

        /// <summary>
        /// 列宽度
        /// </summary>
        public double? Width { get; set; }

        public bool Equals(ListColumnInfo other)
        {
            if (other == null) return false;
            if (ContentAlignment != other.ContentAlignment) return false;
            if (ColumnCode != other.ColumnCode) return false;
            if (Width != other.Width) return false;
            return true;
        }

        public object Clone()
        {
            return new ListColumnInfo
            {
                ContentAlignment = this.ContentAlignment,
                ColumnCode = this.ColumnCode,
                Width = this.Width,
            };
        }
    }
}
