using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thrift;
using Thrift.Protocol;

namespace business_foundation_lib.jsbridge
{
    public class ThriftProtocolTransfer
    {
        private int startMaxDepth = int.MaxValue;
        private TProtocol inProtocol;
        private TProtocol outProtocol;

        public ThriftProtocolTransfer(TProtocol inProtocol) : this(inProtocol, null)
        {
            
        }

        /// <summary>
        /// 构造方法。如果不进行输出，可设置 outProtocol 为空
        /// </summary>
        /// <param name="inProtocol">输入。不能为空</param>
        /// <param name="outProtocol">输出。如果为空，则不进行输出</param>
        public ThriftProtocolTransfer(TProtocol inProtocol, TProtocol outProtocol)
        {
            if (inProtocol == null) throw new ArgumentNullException("inProtocol");
            this.inProtocol = inProtocol;
            this.outProtocol = outProtocol;
        }

        public void StartTransfer(TType type)
        {
            Transfer(type, GetStartMaxDepth());
	    }

        private void Transfer(TType type, int maxDepth)
        {
		    if (maxDepth <= 0) {
                throw new TException("Maximum skip depth exceeded", null);
            }
		    switch (type) {
		    case TType.Bool:
                    {
                        var r = inProtocol.ReadBool();
                        outProtocol?.WriteBool(r);
                        break;
                    }
		    case TType.Byte:
                    {
                        var r = inProtocol.ReadByte();
                        outProtocol?.WriteByte(r);
                        break;
                    }
		    case TType.I16:
                    {
                        var r = inProtocol.ReadI16();
                        outProtocol?.WriteI16(r);
                        break;
                    }
		    case TType.I32:
                    {
                        var r = inProtocol.ReadI32();
                        outProtocol?.WriteI32(r);
                        break;
                    }
		    case TType.I64:
                    {
                        var r = inProtocol.ReadI64();
                        outProtocol?.WriteI64(r);
                        break;
                    }
		    case TType.Double:
                    {
                        var r = inProtocol.ReadDouble();
                        outProtocol?.WriteDouble(r);
                        break;
                    }
		    case TType.String:
                    {
                        var r = inProtocol.ReadString();
                        outProtocol?.WriteString(r);
                        break;
                    }
		    case TType.Struct:
                    {
                        TStruct struc = inProtocol.ReadStructBegin();
                        outProtocol?.WriteStructBegin(struc);
                        while (true)
                        {
                            TField field = inProtocol.ReadFieldBegin();
                            if (field.Type == TType.Stop)
                            {
                                outProtocol?.WriteFieldStop();
                                break;
                            }
                            outProtocol?.WriteFieldBegin(field);

                            Transfer(field.Type, maxDepth - 1);
                            inProtocol.ReadFieldEnd();
                            outProtocol?.WriteFieldEnd();
                        }
                        inProtocol.ReadStructEnd();
                        outProtocol?.WriteStructEnd();
                        break;
                    }
		    case TType.Map:
                    {
                        TMap map = inProtocol.ReadMapBegin();
                        outProtocol?.WriteMapBegin(map);
                        for (int i = 0; i < map.Count; i++)
                        {
                            Transfer(map.KeyType, maxDepth - 1);
                            Transfer(map.ValueType, maxDepth - 1);
                        }
                        inProtocol.ReadMapEnd();
                        outProtocol?.WriteMapEnd();
                        break;
                    }
		    case TType.Set:
                    {
                        TSet set = inProtocol.ReadSetBegin();
                        outProtocol?.WriteSetBegin(set);
                        for (int i = 0; i < set.Count; i++)
                        {
                            Transfer(set.ElementType, maxDepth - 1);
                        }
                        inProtocol.ReadSetEnd();
                        outProtocol?.WriteSetEnd();
                        break;
                    }
		    case TType.List:
                    {
                        TList list = inProtocol.ReadListBegin();
                        outProtocol?.WriteListBegin(list);
                        for (int i = 0; i < list.Count; i++)
                        {
                            Transfer(list.ElementType, maxDepth - 1);
                        }
                        inProtocol.ReadListEnd();
                        outProtocol?.WriteListEnd();
                        break;
                    }
		    default:
			    throw new TException("unkown transfer type " + type, null);
		    }
	    }

	    public int GetStartMaxDepth()
        {
            return startMaxDepth;
        }

        public void SetStartMaxDepth(int startMaxDepth)
        {
            this.startMaxDepth = startMaxDepth;
        }
    }
}
