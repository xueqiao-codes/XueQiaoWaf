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
  public partial class TradeAccountPositionTable : TBase, INotifyPropertyChanged
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
    private long _tradeAccount;
    private long _sledContractId;
    private int _netPosition;
    private long _createTimestampMs;
    private long _lastModifyTimestampMs;

    public long TradeAccount
    {
      get
      {
        return _tradeAccount;
      }
      set
      {
        __isset.tradeAccount = true;
        SetProperty(ref _tradeAccount, value);
      }
    }

    public long SledContractId
    {
      get
      {
        return _sledContractId;
      }
      set
      {
        __isset.sledContractId = true;
        SetProperty(ref _sledContractId, value);
      }
    }

    public int NetPosition
    {
      get
      {
        return _netPosition;
      }
      set
      {
        __isset.netPosition = true;
        SetProperty(ref _netPosition, value);
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
      public bool tradeAccount;
      public bool sledContractId;
      public bool netPosition;
      public bool createTimestampMs;
      public bool lastModifyTimestampMs;
    }

    public TradeAccountPositionTable() {
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
              TradeAccount = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.I64) {
              SledContractId = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.I32) {
              NetPosition = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.I64) {
              CreateTimestampMs = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 5:
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
      TStruct struc = new TStruct("TradeAccountPositionTable");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.tradeAccount) {
        field.Name = "tradeAccount";
        field.Type = TType.I64;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(TradeAccount);
        oprot.WriteFieldEnd();
      }
      if (__isset.sledContractId) {
        field.Name = "sledContractId";
        field.Type = TType.I64;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(SledContractId);
        oprot.WriteFieldEnd();
      }
      if (__isset.netPosition) {
        field.Name = "netPosition";
        field.Type = TType.I32;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(NetPosition);
        oprot.WriteFieldEnd();
      }
      if (__isset.createTimestampMs) {
        field.Name = "createTimestampMs";
        field.Type = TType.I64;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(CreateTimestampMs);
        oprot.WriteFieldEnd();
      }
      if (__isset.lastModifyTimestampMs) {
        field.Name = "lastModifyTimestampMs";
        field.Type = TType.I64;
        field.ID = 5;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(LastModifyTimestampMs);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("TradeAccountPositionTable(");
      sb.Append("TradeAccount: ");
      sb.Append(TradeAccount);
      sb.Append(",SledContractId: ");
      sb.Append(SledContractId);
      sb.Append(",NetPosition: ");
      sb.Append(NetPosition);
      sb.Append(",CreateTimestampMs: ");
      sb.Append(CreateTimestampMs);
      sb.Append(",LastModifyTimestampMs: ");
      sb.Append(LastModifyTimestampMs);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
