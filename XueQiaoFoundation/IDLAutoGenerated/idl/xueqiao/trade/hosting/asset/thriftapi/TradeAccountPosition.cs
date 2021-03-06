/**
 * Autogenerated by Thrift Compiler (0.9.1)
 *
 * DO NOT EDIT UNLESS YOU ARE SURE THAT YOU KNOW WHAT YOU ARE DOING
 *  @generated
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Thrift;
using Thrift.Collections;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Runtime.CompilerServices; 
using Thrift.Protocol;
using Thrift.Transport;

namespace xueqiao.trade.hosting.asset.thriftapi
{

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class TradeAccountPosition : TBase, INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;
    protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
      if (Equals(field, value)) { return false; }
      field = value;
      RaisePropertyChanged(propertyName);
      return true;
    }

    protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    private long _tradeAccountId;
    private Dictionary<long, int> _sledContractNetPositionMap;
    private long _createTimestampMs;
    private long _lastModifyTimestampMs;

    public long TradeAccountId
    {
      get
      {
        return _tradeAccountId;
      }
      set
      {
        __isset.tradeAccountId = true;
        SetProperty(ref _tradeAccountId, value);
      }
    }

    public Dictionary<long, int> SledContractNetPositionMap
    {
      get
      {
        return _sledContractNetPositionMap;
      }
      set
      {
        __isset.sledContractNetPositionMap = true;
        SetProperty(ref _sledContractNetPositionMap, value);
      }
    }

    public long CreateTimestampMs
    {
      get
      {
        return _createTimestampMs;
      }
      set
      {
        __isset.createTimestampMs = true;
        SetProperty(ref _createTimestampMs, value);
      }
    }

    public long LastModifyTimestampMs
    {
      get
      {
        return _lastModifyTimestampMs;
      }
      set
      {
        __isset.lastModifyTimestampMs = true;
        SetProperty(ref _lastModifyTimestampMs, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool tradeAccountId;
      public bool sledContractNetPositionMap;
      public bool createTimestampMs;
      public bool lastModifyTimestampMs;
    }

    public TradeAccountPosition() {
    }

    public void Read (TProtocol iprot)
    {
      TField field;
      iprot.ReadStructBegin();
      while (true)
      {
        field = iprot.ReadFieldBegin();
        if (field.Type == TType.Stop) { 
          break;
        }
        switch (field.ID)
        {
          case 1:
            if (field.Type == TType.I64) {
              TradeAccountId = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.Map) {
              {
                SledContractNetPositionMap = new Dictionary<long, int>();
                TMap _map60 = iprot.ReadMapBegin();
                for( int _i61 = 0; _i61 < _map60.Count; ++_i61)
                {
                  long _key62;
                  int _val63;
                  _key62 = iprot.ReadI64();
                  _val63 = iprot.ReadI32();
                  SledContractNetPositionMap[_key62] = _val63;
                }
                iprot.ReadMapEnd();
              }
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 20:
            if (field.Type == TType.I64) {
              CreateTimestampMs = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 21:
            if (field.Type == TType.I64) {
              LastModifyTimestampMs = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          default: 
            TProtocolUtil.Skip(iprot, field.Type);
            break;
        }
        iprot.ReadFieldEnd();
      }
      iprot.ReadStructEnd();
    }

    public void Write(TProtocol oprot) {
      TStruct struc = new TStruct("TradeAccountPosition");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.tradeAccountId) {
        field.Name = "tradeAccountId";
        field.Type = TType.I64;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(TradeAccountId);
        oprot.WriteFieldEnd();
      }
      if (SledContractNetPositionMap != null && __isset.sledContractNetPositionMap) {
        field.Name = "sledContractNetPositionMap";
        field.Type = TType.Map;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteMapBegin(new TMap(TType.I64, TType.I32, SledContractNetPositionMap.Count));
          foreach (long _iter64 in SledContractNetPositionMap.Keys)
          {
            oprot.WriteI64(_iter64);
            oprot.WriteI32(SledContractNetPositionMap[_iter64]);
          }
          oprot.WriteMapEnd();
        }
        oprot.WriteFieldEnd();
      }
      if (__isset.createTimestampMs) {
        field.Name = "createTimestampMs";
        field.Type = TType.I64;
        field.ID = 20;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(CreateTimestampMs);
        oprot.WriteFieldEnd();
      }
      if (__isset.lastModifyTimestampMs) {
        field.Name = "lastModifyTimestampMs";
        field.Type = TType.I64;
        field.ID = 21;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(LastModifyTimestampMs);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("TradeAccountPosition(");
      sb.Append("TradeAccountId: ");
      sb.Append(TradeAccountId);
      sb.Append(",SledContractNetPositionMap: ");
      if (SledContractNetPositionMap == null)
      {
        sb.Append("<null>");
      }
      else
      {
        sb.Append("{");
        foreach (long _iter65 in SledContractNetPositionMap.Keys)
        {
          sb.Append(_iter65.ToString());
          sb.Append(":");
          sb.Append(SledContractNetPositionMap[_iter65].ToString());
          sb.Append(", ");
        }
        sb.Append("}");
      }
      sb.Append(",CreateTimestampMs: ");
      sb.Append(CreateTimestampMs);
      sb.Append(",LastModifyTimestampMs: ");
      sb.Append(LastModifyTimestampMs);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
