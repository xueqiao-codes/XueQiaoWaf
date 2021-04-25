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

namespace xueqiao.trade.hosting.arbitrage.thriftapi
{

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class HostingXQTrade : TBase, INotifyPropertyChanged
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
    private string _orderId;
    private long _tradeId;
    private HostingXQTarget _tradeTarget;
    private int _tradeVolume;
    private double _tradePrice;
    private int _subUserId;
    private long _subAccountId;
    private HostingXQTradeDirection _tradeDiretion;
    private HostingXQTarget _sourceOrderTarget;
    private long _createTimestampMs;
    private long _lastmodifyTimestampMs;

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

    public HostingXQTarget TradeTarget
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

    public int TradeVolume
    {
      get
      {
        return _tradeVolume;
      }
      set
      {
        __isset.tradeVolume = true;
        SetProperty(ref _tradeVolume, value);
      }
    }

    public double TradePrice
    {
      get
      {
        return _tradePrice;
      }
      set
      {
        __isset.tradePrice = true;
        SetProperty(ref _tradePrice, value);
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

    /// <summary>
    /// 
    /// <seealso cref="HostingXQTradeDirection"/>
    /// </summary>
    public HostingXQTradeDirection TradeDiretion
    {
      get
      {
        return _tradeDiretion;
      }
      set
      {
        __isset.tradeDiretion = true;
        SetProperty(ref _tradeDiretion, value);
      }
    }

    public HostingXQTarget SourceOrderTarget
    {
      get
      {
        return _sourceOrderTarget;
      }
      set
      {
        __isset.sourceOrderTarget = true;
        SetProperty(ref _sourceOrderTarget, value);
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
      public bool orderId;
      public bool tradeId;
      public bool tradeTarget;
      public bool tradeVolume;
      public bool tradePrice;
      public bool subUserId;
      public bool subAccountId;
      public bool tradeDiretion;
      public bool sourceOrderTarget;
      public bool createTimestampMs;
      public bool lastmodifyTimestampMs;
    }

    public HostingXQTrade() {
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
            if (field.Type == TType.String) {
              OrderId = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.I64) {
              TradeId = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.Struct) {
              TradeTarget = new HostingXQTarget();
              TradeTarget.Read(iprot);
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.I32) {
              TradeVolume = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 5:
            if (field.Type == TType.Double) {
              TradePrice = iprot.ReadDouble();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 6:
            if (field.Type == TType.I32) {
              SubUserId = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 7:
            if (field.Type == TType.I64) {
              SubAccountId = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 9:
            if (field.Type == TType.I32) {
              TradeDiretion = (HostingXQTradeDirection)iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 10:
            if (field.Type == TType.Struct) {
              SourceOrderTarget = new HostingXQTarget();
              SourceOrderTarget.Read(iprot);
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 12:
            if (field.Type == TType.I64) {
              CreateTimestampMs = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 13:
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
      TStruct struc = new TStruct("HostingXQTrade");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (OrderId != null && __isset.orderId) {
        field.Name = "orderId";
        field.Type = TType.String;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(OrderId);
        oprot.WriteFieldEnd();
      }
      if (__isset.tradeId) {
        field.Name = "tradeId";
        field.Type = TType.I64;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(TradeId);
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
      if (__isset.tradeVolume) {
        field.Name = "tradeVolume";
        field.Type = TType.I32;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(TradeVolume);
        oprot.WriteFieldEnd();
      }
      if (__isset.tradePrice) {
        field.Name = "tradePrice";
        field.Type = TType.Double;
        field.ID = 5;
        oprot.WriteFieldBegin(field);
        oprot.WriteDouble(TradePrice);
        oprot.WriteFieldEnd();
      }
      if (__isset.subUserId) {
        field.Name = "subUserId";
        field.Type = TType.I32;
        field.ID = 6;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(SubUserId);
        oprot.WriteFieldEnd();
      }
      if (__isset.subAccountId) {
        field.Name = "subAccountId";
        field.Type = TType.I64;
        field.ID = 7;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(SubAccountId);
        oprot.WriteFieldEnd();
      }
      if (__isset.tradeDiretion) {
        field.Name = "tradeDiretion";
        field.Type = TType.I32;
        field.ID = 9;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32((int)TradeDiretion);
        oprot.WriteFieldEnd();
      }
      if (SourceOrderTarget != null && __isset.sourceOrderTarget) {
        field.Name = "sourceOrderTarget";
        field.Type = TType.Struct;
        field.ID = 10;
        oprot.WriteFieldBegin(field);
        SourceOrderTarget.Write(oprot);
        oprot.WriteFieldEnd();
      }
      if (__isset.createTimestampMs) {
        field.Name = "createTimestampMs";
        field.Type = TType.I64;
        field.ID = 12;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(CreateTimestampMs);
        oprot.WriteFieldEnd();
      }
      if (__isset.lastmodifyTimestampMs) {
        field.Name = "lastmodifyTimestampMs";
        field.Type = TType.I64;
        field.ID = 13;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(LastmodifyTimestampMs);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("HostingXQTrade(");
      sb.Append("OrderId: ");
      sb.Append(OrderId);
      sb.Append(",TradeId: ");
      sb.Append(TradeId);
      sb.Append(",TradeTarget: ");
      sb.Append(TradeTarget== null ? "<null>" : TradeTarget.ToString());
      sb.Append(",TradeVolume: ");
      sb.Append(TradeVolume);
      sb.Append(",TradePrice: ");
      sb.Append(TradePrice);
      sb.Append(",SubUserId: ");
      sb.Append(SubUserId);
      sb.Append(",SubAccountId: ");
      sb.Append(SubAccountId);
      sb.Append(",TradeDiretion: ");
      sb.Append(TradeDiretion);
      sb.Append(",SourceOrderTarget: ");
      sb.Append(SourceOrderTarget== null ? "<null>" : SourceOrderTarget.ToString());
      sb.Append(",CreateTimestampMs: ");
      sb.Append(CreateTimestampMs);
      sb.Append(",LastmodifyTimestampMs: ");
      sb.Append(LastmodifyTimestampMs);
      sb.Append(")");
      return sb.ToString();
    }

  }

}