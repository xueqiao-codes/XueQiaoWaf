using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thrift.Collections;
using Thrift.Protocol;
using Thrift.Transport;

namespace XueQiaoFoundation.Shared.Helper
{
    public static class ThriftHelper
    {
        /// <summary>
        /// 将 <see cref="TBase"/> 类型对象序列化成 byte[]
        /// </summary>
        /// <param name="tBaseObj"></param>
        /// <returns></returns>
        public static byte[] SerializeTBaseToBytes(TBase tBaseObj)
        {
            if (tBaseObj == null) return null;

            MemoryStream memStream = null;
            TTransport transport = null;
            TProtocol protocol = null;
            BufferedStream bufStream = null;
            byte[] result = null;

            try
            {
                memStream = new MemoryStream();
                transport = new TStreamTransport(null, memStream);
                protocol = new TCompactProtocol(transport);
                tBaseObj.Write(protocol);

                bufStream = new BufferedStream(memStream);
                long len = bufStream.Length;
                byte[] bytes = new byte[len];
                bufStream.Position = 0;
                bufStream.Read(bytes, 0, (int)len);
                result = bytes;
            }
            finally
            {
                memStream?.Close();
                transport?.Close();
                protocol?.Dispose();
                bufStream?.Close();
            }

            return result;
        }


        /// <summary>
        /// 将 byte[] 反序列化为 <see cref="TBase"/> 类型对象
        /// </summary>
        /// <param name="tbaseObj"></param>
        /// <param name="bytes"></param>
        public static void UnserializeBytesToTBase(ref TBase tbaseObj, byte[] bytes)
        {
            MemoryStream memStream = null;
            BufferedStream bufStream = null;
            TTransport transport = null;
            TProtocol protocol = null;

            try
            {
                memStream = new MemoryStream();
                bufStream = new BufferedStream(memStream);
                bufStream.Write(bytes, 0, bytes.Length);

                transport = new TStreamTransport(bufStream, null);
                protocol = new TCompactProtocol(transport);
                bufStream.Position = 0;
                tbaseObj.Read(protocol);
            }
            finally
            {
                memStream?.Close();
                bufStream?.Close();
                transport?.Close();
                protocol?.Dispose();
            }
        }

        /// <summary>
        /// 将 <see cref="TBase"/> 类型对象序列化成 Thrift json string
        /// </summary>
        /// <param name="tBaseObj"></param>
        /// <returns></returns>
        public static string SerializeToThriftJson(TBase tBaseObj)
        {
            if (tBaseObj == null) return null;

            MemoryStream memStream = null;
            TTransport transport = null;
            TProtocol protocol = null;
            BufferedStream bufStream = null;
            string result = null;

            try
            {
                memStream = new MemoryStream();
                transport = new TStreamTransport(null, memStream);
                protocol = new TJSONProtocol(transport);
                tBaseObj.Write(protocol);

                bufStream = new BufferedStream(memStream);
                long len = bufStream.Length;
                byte[] bytes = new byte[len];
                bufStream.Position = 0;
                bufStream.Read(bytes, 0, (int)len);

                result = Encoding.UTF8.GetString(bytes);
            }
            finally
            {
                memStream?.Close();
                transport?.Close();
                protocol?.Dispose();
                bufStream?.Close();
            }

            return result;
        }

        /// <summary>
        /// 将 Thrift json string 反序列化为 <see cref="TBase"/> 类型对象
        /// </summary>
        /// <param name="tBaseObj"></param>
        /// <param name="thriftJson"></param>
        public static void UnserializeThriftJson(ref TBase tBaseObj, string thriftJson)
        {
            MemoryStream memStream = null;
            BufferedStream bufStream = null;
            TTransport transport = null;
            TProtocol protocol = null;

            try
            {
                byte[] msgBytes = Encoding.UTF8.GetBytes(thriftJson);
                memStream = new MemoryStream();
                bufStream = new BufferedStream(memStream);
                bufStream.Write(msgBytes, 0, msgBytes.Length);

                transport = new TStreamTransport(bufStream, null);
                protocol = new TJSONProtocol(transport);
                bufStream.Position = 0;
                tBaseObj.Read(protocol);
            }
            finally
            {
                memStream?.Close();
                bufStream?.Close();
                transport?.Close();
                protocol?.Dispose();
            }
        }

        public static void AddRange<T>(this THashSet<T> set, IEnumerable<T> addItems)
        {
            if (set == null) return;
            if (addItems?.Any() != true) return;
            foreach (var item in addItems)
            {
                set.Add(item);
            }
        }
    }
}
