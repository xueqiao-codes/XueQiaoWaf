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

  /// <summary>
  /// 资源结算层面的成交明细（从成交列表中的成交明细转换成适合结算用的成交明细）
  /// </summary>
  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class AssetTradeDetail : TBase, INotifyPropertyChanged
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
    private long _execTradeId;
    private long _subAccountId;
    private long _sledContractId;
    private long _execOrderId;
    private double _tradePrice;
    private int _tradeVolume;
    private xueqiao.trade.hosting.HostingExecTradeDirection _execTradeDirection;
    private long _createTimestampMs;
    private long _lastmodifyTimestampMs;
    private long _sledCommodityId;
    private AssetCalculateConfig _config;
    private int _orderTotalVolume;
    private double _limitPrice;
    private string _source;
    private long _tradeAccountId;
    private long _tradeTimestampMs;
    private long _assetTradeDetailId;
    private int _subUserId;
    private string _sledOrderId;

    public long ExecTradeId
    {
      get
      {
        return _execTradeId;
      }
      set
      {
        __isset.execTradeId = true;
        SetProperty(ref _execTradeId, value);
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

    public long ExecOrderId
    {
      get
      {
        return _execOrderId;
      }
      set
      {
        __isset.execOrderId = true;
        SetProperty(ref _execOrderId, value);
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

    /// <summary>
    /// 
    /// <seealso cref="xueqiao.trade.hosting.HostingExecTradeDirection"/>
    /// </summary>
    public xueqiao.trade.hosting.HostingExecTradeDirection ExecTradeDirection
    {
      get
      {
        return _execTradeDirection;
      }
      set
      {
        __isset.execTradeDirection = true;
        SetProperty(ref _execTradeDirection, value);
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

    public long SledCommodityId
    {
      get
      {
        return _sledCommodityId;
      }
      set
      {
        __isset.sledCommodityId = true;
        SetProperty(ref _sledCommodityId, value);
      }
    }

    public AssetCalculateConfig Config
    {
      get
      {
        return _config;
      }
      set
      {
        __isset.config = true;
        SetProperty(ref _config, value);
      }
    }

    public int OrderTotalVolume
    {
      get
      {
        return _orderTotalVolume;
      }
      set
      {
        __isset.orderTotalVolume = true;
        SetProperty(ref _orderTotalVolume, value);
      }
    }

    public double LimitPrice
    {
      get
      {
        return _limitPrice;
      }
      set
      {
        __isset.limitPrice = true;
        SetProperty(ref _limitPrice, value);
      }
    }

    public string Source
    {
      get
      {
        return _source;
      }
      set
      {
        __isset.source = true;
        SetProperty(ref _source, value);
      }
    }

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

    public long TradeTimestampMs
    {
      get
      {
        return _tradeTimestampMs;
      }
      set
      {
        __isset.tradeTimestampMs = true;
        SetProperty(ref _tradeTimestampMs, value);
      }
    }

    public long AssetTradeDetailId
    {
      get
      {
        return _assetTradeDetailId;
      }
      set
      {
        __isset.assetTradeDetailId = true;
        SetProperty(ref _assetTradeDetailId, value);
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

    public string SledOrderId
    {
      get
      {
        return _sledOrderId;
      }
      set
      {
        __isset.sledOrderId = true;
        SetProperty(ref _sledOrderId, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool execTradeId;
      public bool subAccountId;
      public bool sledContractId;
      public bool execOrderId;
      public bool tradePrice;
      public bool tradeVolume;
      public bool execTradeDirection;
      public bool createTimestampMs;
      public bool lastmodifyTimestampMs;
      public bool sledCommodityId;
      public bool config;
      public bool orderTotalVolume;
      public bool limitPrice;
      public bool source;
      public bool tradeAccountId;
      public bool tradeTimestampMs;
      public bool assetTradeDetailId;
      public bool subUserId;
      public bool sledOrderId;
    }

    public AssetTradeDetail() {
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
              ExecTradeId = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.I64) {
              SubAccountId = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.I64) {
              SledContractId = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.I64) {
              ExecOrderId = iprot.ReadI64();
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
              TradeVolume = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 7:
            if (field.Type == TType.I32) {
              ExecTradeDirection = (xueqiao.trade.hosting.HostingExecTradeDirection)iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 8:
            if (field.Type == TType.I64) {
              CreateTimestampMs = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 9:
            if (field.Type == TType.I64) {
              LastmodifyTimestampMs = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 10:
            if (field.Type == TType.I64) {
              SledCommodityId = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 11:
            if (field.Type == TType.Struct) {
              Config = new AssetCalculateConfig();
              Config.Read(iprot);
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 12:
            if (field.Type == TType.I32) {
              OrderTotalVolume = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 13:
            if (field.Type == TType.Double) {
              LimitPrice = iprot.ReadDouble();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 14:
            if (field.Type == TType.String) {
              Source = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 15:
            if (field.Type == TType.I64) {
              TradeAccountId = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 16:
            if (field.Type == TType.I64) {
              TradeTimestampMs = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 17:
            if (field.Type == TType.I64) {
              AssetTradeDetailId = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 18:
            if (field.Type == TType.I32) {
              SubUserId = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 19:
            if (field.Type == TType.String) {
              SledOrderId = iprot.ReadString();
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
      TStruct struc = new TStruct("AssetTradeDetail");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.execTradeId) {
        field.Name = "execTradeId";
        field.Type = TType.I64;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(ExecTradeId);
        oprot.WriteFieldEnd();
      }
      if (__isset.subAccountId) {
        field.Name = "subAccountId";
        field.Type = TType.I64;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(SubAccountId);
        oprot.WriteFieldEnd();
      }
      if (__isset.sledContractId) {
        field.Name = "sledContractId";
        field.Type = TType.I64;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(SledContractId);
        oprot.WriteFieldEnd();
      }
      if (__isset.execOrderId) {
        field.Name = "execOrderId";
        field.Type = TType.I64;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(ExecOrderId);
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
      if (__isset.tradeVolume) {
        field.Name = "tradeVolume";
        field.Type = TType.I32;
        field.ID = 6;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(TradeVolume);
        oprot.WriteFieldEnd();
      }
      if (__isset.execTradeDirection) {
        field.Name = "execTradeDirection";
        field.Type = TType.I32;
        field.ID = 7;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32((int)ExecTradeDirection);
        oprot.WriteFieldEnd();
      }
      if (__isset.createTimestampMs) {
        field.Name = "createTimestampMs";
        field.Type = TType.I64;
        field.ID = 8;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(CreateTimestampMs);
        oprot.WriteFieldEnd();
      }
      if (__isset.lastmodifyTimestampMs) {
        field.Name = "lastmodifyTimestampMs";
        field.Type = TType.I64;
        field.ID = 9;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(LastmodifyTimestampMs);
        oprot.WriteFieldEnd();
      }
      if (__isset.sledCommodityId) {
        field.Name = "sledCommodityId";
        field.Type = TType.I64;
        field.ID = 10;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(SledCommodityId);
        oprot.WriteFieldEnd();
      }
      if (Config != null && __isset.config) {
        field.Name = "config";
        field.Type = TType.Struct;
        field.ID = 11;
        oprot.WriteFieldBegin(field);
        Config.Write(oprot);
        oprot.WriteFieldEnd();
      }
      if (__isset.orderTotalVolume) {
        field.Name = "orderTotalVolume";
        field.Type = TType.I32;
        field.ID = 12;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(OrderTotalVolume);
        oprot.WriteFieldEnd();
      }
      if (__isset.limitPrice) {
        field.Name = "limitPrice";
        field.Type = TType.Double;
        field.ID = 13;
        oprot.WriteFieldBegin(field);
        oprot.WriteDouble(LimitPrice);
        oprot.WriteFieldEnd();
      }
      if (Source != null && __isset.source) {
        field.Name = "source";
        field.Type = TType.String;
        field.ID = 14;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Source);
        oprot.WriteFieldEnd();
      }
      if (__isset.tradeAccountId) {
        field.Name = "tradeAccountId";
        field.Type = TType.I64;
        field.ID = 15;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(TradeAccountId);
        oprot.WriteFieldEnd();
      }
      if (__isset.tradeTimestampMs) {
        field.Name = "tradeTimestampMs";
        field.Type = TType.I64;
        field.ID = 16;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(TradeTimestampMs);
        oprot.WriteFieldEnd();
      }
      if (__isset.assetTradeDetailId) {
        field.Name = "assetTradeDetailId";
        field.Type = TType.I64;
        field.ID = 17;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(AssetTradeDetailId);
        oprot.WriteFieldEnd();
      }
      if (__isset.subUserId) {
        field.Name = "subUserId";
        field.Type = TType.I32;
        field.ID = 18;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(SubUserId);
        oprot.WriteFieldEnd();
      }
      if (SledOrderId != null && __isset.sledOrderId) {
        field.Name = "sledOrderId";
        field.Type = TType.String;
        field.ID = 19;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(SledOrderId);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("AssetTradeDetail(");
      sb.Append("ExecTradeId: ");
      sb.Append(ExecTradeId);
      sb.Append(",SubAccountId: ");
      sb.Append(SubAccountId);
      sb.Append(",SledContractId: ");
      sb.Append(SledContractId);
      sb.Append(",ExecOrderId: ");
      sb.Append(ExecOrderId);
      sb.Append(",TradePrice: ");
      sb.Append(TradePrice);
      sb.Append(",TradeVolume: ");
      sb.Append(TradeVolume);
      sb.Append(",ExecTradeDirection: ");
      sb.Append(ExecTradeDirection);
      sb.Append(",CreateTimestampMs: ");
      sb.Append(CreateTimestampMs);
      sb.Append(",LastmodifyTimestampMs: ");
      sb.Append(LastmodifyTimestampMs);
      sb.Append(",SledCommodityId: ");
      sb.Append(SledCommodityId);
      sb.Append(",Config: ");
      sb.Append(Config== null ? "<null>" : Config.ToString());
      sb.Append(",OrderTotalVolume: ");
      sb.Append(OrderTotalVolume);
      sb.Append(",LimitPrice: ");
      sb.Append(LimitPrice);
      sb.Append(",Source: ");
      sb.Append(Source);
      sb.Append(",TradeAccountId: ");
      sb.Append(TradeAccountId);
      sb.Append(",TradeTimestampMs: ");
      sb.Append(TradeTimestampMs);
      sb.Append(",AssetTradeDetailId: ");
      sb.Append(AssetTradeDetailId);
      sb.Append(",SubUserId: ");
      sb.Append(SubUserId);
      sb.Append(",SledOrderId: ");
      sb.Append(SledOrderId);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
