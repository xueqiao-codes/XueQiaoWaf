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

namespace xueqiao.trade.hosting
{

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class HostingExecTrade : TBase, INotifyPropertyChanged
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
    private long _execOrderId;
    private int _subUserId;
    private long _subAccountId;
    private HostingExecOrderContractSummary _contractSummary;
    private HostingExecOrderTradeAccountSummary _accountSummary;
    private List<long> _relatedTradeLegIds;
    private double _tradePrice;
    private int _tradeVolume;
    private long _createTimestampMs;
    private long _lastmodifyTimestampMs;
    private List<double> _relatedTradeLegPrices;
    private HostingExecOrderTradeDirection _orderTradeDirection;
    private List<HostingExecTradeDirection> _relatedTradeLegTradeDirections;
    private List<HostingExecOrderLegContractSummary> _relatedTradeLegContractSummaries;
    private List<int> _relatedTradeLegVolumes;
    private int _relatedTradeLegCount;

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

    public HostingExecOrderContractSummary ContractSummary
    {
      get
      {
        return _contractSummary;
      }
      set
      {
        __isset.contractSummary = true;
        SetProperty(ref _contractSummary, value);
      }
    }

    public HostingExecOrderTradeAccountSummary AccountSummary
    {
      get
      {
        return _accountSummary;
      }
      set
      {
        __isset.accountSummary = true;
        SetProperty(ref _accountSummary, value);
      }
    }

    public List<long> RelatedTradeLegIds
    {
      get
      {
        return _relatedTradeLegIds;
      }
      set
      {
        __isset.relatedTradeLegIds = true;
        SetProperty(ref _relatedTradeLegIds, value);
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

    public List<double> RelatedTradeLegPrices
    {
      get
      {
        return _relatedTradeLegPrices;
      }
      set
      {
        __isset.relatedTradeLegPrices = true;
        SetProperty(ref _relatedTradeLegPrices, value);
      }
    }

    /// <summary>
    /// 
    /// <seealso cref="HostingExecOrderTradeDirection"/>
    /// </summary>
    public HostingExecOrderTradeDirection OrderTradeDirection
    {
      get
      {
        return _orderTradeDirection;
      }
      set
      {
        __isset.orderTradeDirection = true;
        SetProperty(ref _orderTradeDirection, value);
      }
    }

    public List<HostingExecTradeDirection> RelatedTradeLegTradeDirections
    {
      get
      {
        return _relatedTradeLegTradeDirections;
      }
      set
      {
        __isset.relatedTradeLegTradeDirections = true;
        SetProperty(ref _relatedTradeLegTradeDirections, value);
      }
    }

    public List<HostingExecOrderLegContractSummary> RelatedTradeLegContractSummaries
    {
      get
      {
        return _relatedTradeLegContractSummaries;
      }
      set
      {
        __isset.relatedTradeLegContractSummaries = true;
        SetProperty(ref _relatedTradeLegContractSummaries, value);
      }
    }

    public List<int> RelatedTradeLegVolumes
    {
      get
      {
        return _relatedTradeLegVolumes;
      }
      set
      {
        __isset.relatedTradeLegVolumes = true;
        SetProperty(ref _relatedTradeLegVolumes, value);
      }
    }

    public int RelatedTradeLegCount
    {
      get
      {
        return _relatedTradeLegCount;
      }
      set
      {
        __isset.relatedTradeLegCount = true;
        SetProperty(ref _relatedTradeLegCount, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool execTradeId;
      public bool execOrderId;
      public bool subUserId;
      public bool subAccountId;
      public bool contractSummary;
      public bool accountSummary;
      public bool relatedTradeLegIds;
      public bool tradePrice;
      public bool tradeVolume;
      public bool createTimestampMs;
      public bool lastmodifyTimestampMs;
      public bool relatedTradeLegPrices;
      public bool orderTradeDirection;
      public bool relatedTradeLegTradeDirections;
      public bool relatedTradeLegContractSummaries;
      public bool relatedTradeLegVolumes;
      public bool relatedTradeLegCount;
    }

    public HostingExecTrade() {
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
              ExecOrderId = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.I32) {
              SubUserId = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.I64) {
              SubAccountId = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 5:
            if (field.Type == TType.Struct) {
              ContractSummary = new HostingExecOrderContractSummary();
              ContractSummary.Read(iprot);
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 6:
            if (field.Type == TType.Struct) {
              AccountSummary = new HostingExecOrderTradeAccountSummary();
              AccountSummary.Read(iprot);
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 7:
            if (field.Type == TType.List) {
              {
                RelatedTradeLegIds = new List<long>();
                TList _list50 = iprot.ReadListBegin();
                for( int _i51 = 0; _i51 < _list50.Count; ++_i51)
                {
                  long _elem52 = 0;
                  _elem52 = iprot.ReadI64();
                  RelatedTradeLegIds.Add(_elem52);
                }
                iprot.ReadListEnd();
              }
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 8:
            if (field.Type == TType.Double) {
              TradePrice = iprot.ReadDouble();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 9:
            if (field.Type == TType.I32) {
              TradeVolume = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 10:
            if (field.Type == TType.I64) {
              CreateTimestampMs = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 11:
            if (field.Type == TType.I64) {
              LastmodifyTimestampMs = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 12:
            if (field.Type == TType.List) {
              {
                RelatedTradeLegPrices = new List<double>();
                TList _list53 = iprot.ReadListBegin();
                for( int _i54 = 0; _i54 < _list53.Count; ++_i54)
                {
                  double _elem55 = (double)0;
                  _elem55 = iprot.ReadDouble();
                  RelatedTradeLegPrices.Add(_elem55);
                }
                iprot.ReadListEnd();
              }
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 13:
            if (field.Type == TType.I32) {
              OrderTradeDirection = (HostingExecOrderTradeDirection)iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 14:
            if (field.Type == TType.List) {
              {
                RelatedTradeLegTradeDirections = new List<HostingExecTradeDirection>();
                TList _list56 = iprot.ReadListBegin();
                for( int _i57 = 0; _i57 < _list56.Count; ++_i57)
                {
                  HostingExecTradeDirection _elem58 = (HostingExecTradeDirection)0;
                  _elem58 = (HostingExecTradeDirection)iprot.ReadI32();
                  RelatedTradeLegTradeDirections.Add(_elem58);
                }
                iprot.ReadListEnd();
              }
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 15:
            if (field.Type == TType.List) {
              {
                RelatedTradeLegContractSummaries = new List<HostingExecOrderLegContractSummary>();
                TList _list59 = iprot.ReadListBegin();
                for( int _i60 = 0; _i60 < _list59.Count; ++_i60)
                {
                  HostingExecOrderLegContractSummary _elem61 = new HostingExecOrderLegContractSummary();
                  _elem61 = new HostingExecOrderLegContractSummary();
                  _elem61.Read(iprot);
                  RelatedTradeLegContractSummaries.Add(_elem61);
                }
                iprot.ReadListEnd();
              }
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 16:
            if (field.Type == TType.List) {
              {
                RelatedTradeLegVolumes = new List<int>();
                TList _list62 = iprot.ReadListBegin();
                for( int _i63 = 0; _i63 < _list62.Count; ++_i63)
                {
                  int _elem64 = 0;
                  _elem64 = iprot.ReadI32();
                  RelatedTradeLegVolumes.Add(_elem64);
                }
                iprot.ReadListEnd();
              }
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 17:
            if (field.Type == TType.I32) {
              RelatedTradeLegCount = iprot.ReadI32();
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
      TStruct struc = new TStruct("HostingExecTrade");
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
      if (__isset.execOrderId) {
        field.Name = "execOrderId";
        field.Type = TType.I64;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(ExecOrderId);
        oprot.WriteFieldEnd();
      }
      if (__isset.subUserId) {
        field.Name = "subUserId";
        field.Type = TType.I32;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(SubUserId);
        oprot.WriteFieldEnd();
      }
      if (__isset.subAccountId) {
        field.Name = "subAccountId";
        field.Type = TType.I64;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(SubAccountId);
        oprot.WriteFieldEnd();
      }
      if (ContractSummary != null && __isset.contractSummary) {
        field.Name = "contractSummary";
        field.Type = TType.Struct;
        field.ID = 5;
        oprot.WriteFieldBegin(field);
        ContractSummary.Write(oprot);
        oprot.WriteFieldEnd();
      }
      if (AccountSummary != null && __isset.accountSummary) {
        field.Name = "accountSummary";
        field.Type = TType.Struct;
        field.ID = 6;
        oprot.WriteFieldBegin(field);
        AccountSummary.Write(oprot);
        oprot.WriteFieldEnd();
      }
      if (RelatedTradeLegIds != null && __isset.relatedTradeLegIds) {
        field.Name = "relatedTradeLegIds";
        field.Type = TType.List;
        field.ID = 7;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteListBegin(new TList(TType.I64, RelatedTradeLegIds.Count));
          foreach (long _iter65 in RelatedTradeLegIds)
          {
            oprot.WriteI64(_iter65);
          }
          oprot.WriteListEnd();
        }
        oprot.WriteFieldEnd();
      }
      if (__isset.tradePrice) {
        field.Name = "tradePrice";
        field.Type = TType.Double;
        field.ID = 8;
        oprot.WriteFieldBegin(field);
        oprot.WriteDouble(TradePrice);
        oprot.WriteFieldEnd();
      }
      if (__isset.tradeVolume) {
        field.Name = "tradeVolume";
        field.Type = TType.I32;
        field.ID = 9;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(TradeVolume);
        oprot.WriteFieldEnd();
      }
      if (__isset.createTimestampMs) {
        field.Name = "createTimestampMs";
        field.Type = TType.I64;
        field.ID = 10;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(CreateTimestampMs);
        oprot.WriteFieldEnd();
      }
      if (__isset.lastmodifyTimestampMs) {
        field.Name = "lastmodifyTimestampMs";
        field.Type = TType.I64;
        field.ID = 11;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(LastmodifyTimestampMs);
        oprot.WriteFieldEnd();
      }
      if (RelatedTradeLegPrices != null && __isset.relatedTradeLegPrices) {
        field.Name = "relatedTradeLegPrices";
        field.Type = TType.List;
        field.ID = 12;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteListBegin(new TList(TType.Double, RelatedTradeLegPrices.Count));
          foreach (double _iter66 in RelatedTradeLegPrices)
          {
            oprot.WriteDouble(_iter66);
          }
          oprot.WriteListEnd();
        }
        oprot.WriteFieldEnd();
      }
      if (__isset.orderTradeDirection) {
        field.Name = "orderTradeDirection";
        field.Type = TType.I32;
        field.ID = 13;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32((int)OrderTradeDirection);
        oprot.WriteFieldEnd();
      }
      if (RelatedTradeLegTradeDirections != null && __isset.relatedTradeLegTradeDirections) {
        field.Name = "relatedTradeLegTradeDirections";
        field.Type = TType.List;
        field.ID = 14;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteListBegin(new TList(TType.I32, RelatedTradeLegTradeDirections.Count));
          foreach (HostingExecTradeDirection _iter67 in RelatedTradeLegTradeDirections)
          {
            oprot.WriteI32((int)_iter67);
          }
          oprot.WriteListEnd();
        }
        oprot.WriteFieldEnd();
      }
      if (RelatedTradeLegContractSummaries != null && __isset.relatedTradeLegContractSummaries) {
        field.Name = "relatedTradeLegContractSummaries";
        field.Type = TType.List;
        field.ID = 15;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteListBegin(new TList(TType.Struct, RelatedTradeLegContractSummaries.Count));
          foreach (HostingExecOrderLegContractSummary _iter68 in RelatedTradeLegContractSummaries)
          {
            _iter68.Write(oprot);
          }
          oprot.WriteListEnd();
        }
        oprot.WriteFieldEnd();
      }
      if (RelatedTradeLegVolumes != null && __isset.relatedTradeLegVolumes) {
        field.Name = "relatedTradeLegVolumes";
        field.Type = TType.List;
        field.ID = 16;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteListBegin(new TList(TType.I32, RelatedTradeLegVolumes.Count));
          foreach (int _iter69 in RelatedTradeLegVolumes)
          {
            oprot.WriteI32(_iter69);
          }
          oprot.WriteListEnd();
        }
        oprot.WriteFieldEnd();
      }
      if (__isset.relatedTradeLegCount) {
        field.Name = "relatedTradeLegCount";
        field.Type = TType.I32;
        field.ID = 17;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(RelatedTradeLegCount);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("HostingExecTrade(");
      sb.Append("ExecTradeId: ");
      sb.Append(ExecTradeId);
      sb.Append(",ExecOrderId: ");
      sb.Append(ExecOrderId);
      sb.Append(",SubUserId: ");
      sb.Append(SubUserId);
      sb.Append(",SubAccountId: ");
      sb.Append(SubAccountId);
      sb.Append(",ContractSummary: ");
      sb.Append(ContractSummary== null ? "<null>" : ContractSummary.ToString());
      sb.Append(",AccountSummary: ");
      sb.Append(AccountSummary== null ? "<null>" : AccountSummary.ToString());
      sb.Append(",RelatedTradeLegIds: ");
      if (RelatedTradeLegIds == null)
      {
        sb.Append("<null>");
      }
      else
      {
        sb.Append("[");
        foreach (long _iter70 in RelatedTradeLegIds)
        {
          sb.Append(_iter70.ToString());
          sb.Append(", ");
        }
        sb.Append("]");
      }
      sb.Append(",TradePrice: ");
      sb.Append(TradePrice);
      sb.Append(",TradeVolume: ");
      sb.Append(TradeVolume);
      sb.Append(",CreateTimestampMs: ");
      sb.Append(CreateTimestampMs);
      sb.Append(",LastmodifyTimestampMs: ");
      sb.Append(LastmodifyTimestampMs);
      sb.Append(",RelatedTradeLegPrices: ");
      if (RelatedTradeLegPrices == null)
      {
        sb.Append("<null>");
      }
      else
      {
        sb.Append("[");
        foreach (double _iter71 in RelatedTradeLegPrices)
        {
          sb.Append(_iter71.ToString());
          sb.Append(", ");
        }
        sb.Append("]");
      }
      sb.Append(",OrderTradeDirection: ");
      sb.Append(OrderTradeDirection);
      sb.Append(",RelatedTradeLegTradeDirections: ");
      if (RelatedTradeLegTradeDirections == null)
      {
        sb.Append("<null>");
      }
      else
      {
        sb.Append("[");
        foreach (HostingExecTradeDirection _iter72 in RelatedTradeLegTradeDirections)
        {
          sb.Append(_iter72.ToString());
          sb.Append(", ");
        }
        sb.Append("]");
      }
      sb.Append(",RelatedTradeLegContractSummaries: ");
      if (RelatedTradeLegContractSummaries == null)
      {
        sb.Append("<null>");
      }
      else
      {
        sb.Append("[");
        foreach (HostingExecOrderLegContractSummary _iter73 in RelatedTradeLegContractSummaries)
        {
          sb.Append(_iter73.ToString());
          sb.Append(", ");
        }
        sb.Append("]");
      }
      sb.Append(",RelatedTradeLegVolumes: ");
      if (RelatedTradeLegVolumes == null)
      {
        sb.Append("<null>");
      }
      else
      {
        sb.Append("[");
        foreach (int _iter74 in RelatedTradeLegVolumes)
        {
          sb.Append(_iter74.ToString());
          sb.Append(", ");
        }
        sb.Append("]");
      }
      sb.Append(",RelatedTradeLegCount: ");
      sb.Append(RelatedTradeLegCount);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
