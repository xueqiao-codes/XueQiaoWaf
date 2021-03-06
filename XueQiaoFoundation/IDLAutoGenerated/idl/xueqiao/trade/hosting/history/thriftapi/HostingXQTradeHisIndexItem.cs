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

namespace xueqiao.trade.hosting.history.thriftapi
{

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class HostingXQTradeHisIndexItem : TBase, INotifyPropertyChanged
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
    private long _subAccountId;
    private int _subUserId;
    private xueqiao.trade.hosting.arbitrage.thriftapi.HostingXQTarget _tradeTarget;
    private long _tradeCreateTimestampMs;
    private long _tradeId;
    private string _orderId;
    private long _createTimestampMs;
    private long _lastmodifyTimestampMs;

    public long SubAccountId
    {
      get
      {
        return _subAccountId;
      }
      set
      {
        __isset.subAccountId = true;
        SetProperty(ref _subAccountId, value);
      }
    }

    public int SubUserId
    {
      get
      {
        return _subUserId;
      }
      set
      {
        __isset.subUserId = true;
        SetProperty(ref _subUserId, value);
      }
    }

    public xueqiao.trade.hosting.arbitrage.thriftapi.HostingXQTarget TradeTarget
    {
      get
      {
        return _tradeTarget;
      }
      set
      {
        __isset.tradeTarget = true;
        SetProperty(ref _tradeTarget, value);
      }
    }

    public long TradeCreateTimestampMs
    {
      get
      {
        return _tradeCreateTimestampMs;
      }
      set
      {
        __isset.tradeCreateTimestampMs = true;
        SetProperty(ref _tradeCreateTimestampMs, value);
      }
    }

    public long TradeId
    {
      get
      {
        return _tradeId;
      }
      set
      {
        __isset.tradeId = true;
        SetProperty(ref _tradeId, value);
      }
    }

    public string OrderId
    {
      get
      {
        return _orderId;
      }
      set
      {
        __isset.orderId = true;
        SetProperty(ref _orderId, value);
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

    public long LastmodifyTimestampMs
    {
      get
      {
        return _lastmodifyTimestampMs;
      }
      set
      {
        __isset.lastmodifyTimestampMs = true;
        SetProperty(ref _lastmodifyTimestampMs, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool subAccountId;
      public bool subUserId;
      public bool tradeTarget;
      public bool tradeCreateTimestampMs;
      public bool tradeId;
      public bool orderId;
      public bool createTimestampMs;
      public bool lastmodifyTimestampMs;
    }

    public HostingXQTradeHisIndexItem() {
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
              SubAccountId = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.I32) {
              SubUserId = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.Struct) {
              TradeTarget = new xueqiao.trade.hosting.arbitrage.thriftapi.HostingXQTarget();
              TradeTarget.Read(iprot);
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.I64) {
              TradeCreateTimestampMs = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 5:
            if (field.Type == TType.I64) {
              TradeId = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 6:
            if (field.Type == TType.String) {
              OrderId = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 9:
            if (field.Type == TType.I64) {
              CreateTimestampMs = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 10:
            if (field.Type == TType.I64) {
              LastmodifyTimestampMs = iprot.ReadI64();
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
      TStruct struc = new TStruct("HostingXQTradeHisIndexItem");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.subAccountId) {
        field.Name = "subAccountId";
        field.Type = TType.I64;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(SubAccountId);
        oprot.WriteFieldEnd();
      }
      if (__isset.subUserId) {
        field.Name = "subUserId";
        field.Type = TType.I32;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(SubUserId);
        oprot.WriteFieldEnd();
      }
      if (TradeTarget != null && __isset.tradeTarget) {
        field.Name = "tradeTarget";
        field.Type = TType.Struct;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        TradeTarget.Write(oprot);
        oprot.WriteFieldEnd();
      }
      if (__isset.tradeCreateTimestampMs) {
        field.Name = "tradeCreateTimestampMs";
        field.Type = TType.I64;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(TradeCreateTimestampMs);
        oprot.WriteFieldEnd();
      }
      if (__isset.tradeId) {
        field.Name = "tradeId";
        field.Type = TType.I64;
        field.ID = 5;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(TradeId);
        oprot.WriteFieldEnd();
      }
      if (OrderId != null && __isset.orderId) {
        field.Name = "orderId";
        field.Type = TType.String;
        field.ID = 6;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(OrderId);
        oprot.WriteFieldEnd();
      }
      if (__isset.createTimestampMs) {
        field.Name = "createTimestampMs";
        field.Type = TType.I64;
        field.ID = 9;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(CreateTimestampMs);
        oprot.WriteFieldEnd();
      }
      if (__isset.lastmodifyTimestampMs) {
        field.Name = "lastmodifyTimestampMs";
        field.Type = TType.I64;
        field.ID = 10;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(LastmodifyTimestampMs);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("HostingXQTradeHisIndexItem(");
      sb.Append("SubAccountId: ");
      sb.Append(SubAccountId);
      sb.Append(",SubUserId: ");
      sb.Append(SubUserId);
      sb.Append(",TradeTarget: ");
      sb.Append(TradeTarget== null ? "<null>" : TradeTarget.ToString());
      sb.Append(",TradeCreateTimestampMs: ");
      sb.Append(TradeCreateTimestampMs);
      sb.Append(",TradeId: ");
      sb.Append(TradeId);
      sb.Append(",OrderId: ");
      sb.Append(OrderId);
      sb.Append(",CreateTimestampMs: ");
      sb.Append(CreateTimestampMs);
      sb.Append(",LastmodifyTimestampMs: ");
      sb.Append(LastmodifyTimestampMs);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
