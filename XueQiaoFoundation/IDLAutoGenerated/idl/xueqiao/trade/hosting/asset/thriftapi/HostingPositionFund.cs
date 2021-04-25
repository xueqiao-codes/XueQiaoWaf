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
  /// 雪橇合约资金根据不同价格计算变动信息
  /// </summary>
  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class HostingPositionFund : TBase, INotifyPropertyChanged
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
    private long _sledContractId;
    private long _subAccountId;
    private double _positionProfit;
    private double _calculatePrice;
    private double _useMargin;
    private double _frozenMargin;
    private double _frozenCommission;
    private string _currency;
    private double _goodsValue;
    private double _leverage;

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

    public double PositionProfit
    {
      get
      {
        return _positionProfit;
      }
      set
      {
        __isset.positionProfit = true;
        SetProperty(ref _positionProfit, value);
      }
    }

    public double CalculatePrice
    {
      get
      {
        return _calculatePrice;
      }
      set
      {
        __isset.calculatePrice = true;
        SetProperty(ref _calculatePrice, value);
      }
    }

    public double UseMargin
    {
      get
      {
        return _useMargin;
      }
      set
      {
        __isset.useMargin = true;
        SetProperty(ref _useMargin, value);
      }
    }

    public double FrozenMargin
    {
      get
      {
        return _frozenMargin;
      }
      set
      {
        __isset.frozenMargin = true;
        SetProperty(ref _frozenMargin, value);
      }
    }

    public double FrozenCommission
    {
      get
      {
        return _frozenCommission;
      }
      set
      {
        __isset.frozenCommission = true;
        SetProperty(ref _frozenCommission, value);
      }
    }

    public string Currency
    {
      get
      {
        return _currency;
      }
      set
      {
        __isset.currency = true;
        SetProperty(ref _currency, value);
      }
    }

    public double GoodsValue
    {
      get
      {
        return _goodsValue;
      }
      set
      {
        __isset.goodsValue = true;
        SetProperty(ref _goodsValue, value);
      }
    }

    public double Leverage
    {
      get
      {
        return _leverage;
      }
      set
      {
        __isset.leverage = true;
        SetProperty(ref _leverage, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool sledContractId;
      public bool subAccountId;
      public bool positionProfit;
      public bool calculatePrice;
      public bool useMargin;
      public bool frozenMargin;
      public bool frozenCommission;
      public bool currency;
      public bool goodsValue;
      public bool leverage;
    }

    public HostingPositionFund() {
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
              SledContractId = iprot.ReadI64();
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
          case 9:
            if (field.Type == TType.Double) {
              PositionProfit = iprot.ReadDouble();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 11:
            if (field.Type == TType.Double) {
              CalculatePrice = iprot.ReadDouble();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 12:
            if (field.Type == TType.Double) {
              UseMargin = iprot.ReadDouble();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 13:
            if (field.Type == TType.Double) {
              FrozenMargin = iprot.ReadDouble();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 15:
            if (field.Type == TType.Double) {
              FrozenCommission = iprot.ReadDouble();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 16:
            if (field.Type == TType.String) {
              Currency = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 17:
            if (field.Type == TType.Double) {
              GoodsValue = iprot.ReadDouble();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 18:
            if (field.Type == TType.Double) {
              Leverage = iprot.ReadDouble();
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
      TStruct struc = new TStruct("HostingPositionFund");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.sledContractId) {
        field.Name = "sledContractId";
        field.Type = TType.I64;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(SledContractId);
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
      if (__isset.positionProfit) {
        field.Name = "positionProfit";
        field.Type = TType.Double;
        field.ID = 9;
        oprot.WriteFieldBegin(field);
        oprot.WriteDouble(PositionProfit);
        oprot.WriteFieldEnd();
      }
      if (__isset.calculatePrice) {
        field.Name = "calculatePrice";
        field.Type = TType.Double;
        field.ID = 11;
        oprot.WriteFieldBegin(field);
        oprot.WriteDouble(CalculatePrice);
        oprot.WriteFieldEnd();
      }
      if (__isset.useMargin) {
        field.Name = "useMargin";
        field.Type = TType.Double;
        field.ID = 12;
        oprot.WriteFieldBegin(field);
        oprot.WriteDouble(UseMargin);
        oprot.WriteFieldEnd();
      }
      if (__isset.frozenMargin) {
        field.Name = "frozenMargin";
        field.Type = TType.Double;
        field.ID = 13;
        oprot.WriteFieldBegin(field);
        oprot.WriteDouble(FrozenMargin);
        oprot.WriteFieldEnd();
      }
      if (__isset.frozenCommission) {
        field.Name = "frozenCommission";
        field.Type = TType.Double;
        field.ID = 15;
        oprot.WriteFieldBegin(field);
        oprot.WriteDouble(FrozenCommission);
        oprot.WriteFieldEnd();
      }
      if (Currency != null && __isset.currency) {
        field.Name = "currency";
        field.Type = TType.String;
        field.ID = 16;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Currency);
        oprot.WriteFieldEnd();
      }
      if (__isset.goodsValue) {
        field.Name = "goodsValue";
        field.Type = TType.Double;
        field.ID = 17;
        oprot.WriteFieldBegin(field);
        oprot.WriteDouble(GoodsValue);
        oprot.WriteFieldEnd();
      }
      if (__isset.leverage) {
        field.Name = "leverage";
        field.Type = TType.Double;
        field.ID = 18;
        oprot.WriteFieldBegin(field);
        oprot.WriteDouble(Leverage);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("HostingPositionFund(");
      sb.Append("SledContractId: ");
      sb.Append(SledContractId);
      sb.Append(",SubAccountId: ");
      sb.Append(SubAccountId);
      sb.Append(",PositionProfit: ");
      sb.Append(PositionProfit);
      sb.Append(",CalculatePrice: ");
      sb.Append(CalculatePrice);
      sb.Append(",UseMargin: ");
      sb.Append(UseMargin);
      sb.Append(",FrozenMargin: ");
      sb.Append(FrozenMargin);
      sb.Append(",FrozenCommission: ");
      sb.Append(FrozenCommission);
      sb.Append(",Currency: ");
      sb.Append(Currency);
      sb.Append(",GoodsValue: ");
      sb.Append(GoodsValue);
      sb.Append(",Leverage: ");
      sb.Append(Leverage);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
