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
  public partial class HostingExecOrderTradeSummary : TBase, INotifyPropertyChanged
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
    private int _upsideTradeTotalVolume;
    private double _upsideTradeAveragePrice;
    private int _upsideTradeRestingVolume;
    private int _tradeListTotalVolume;
    private double _tradeListAveragePrice;

    public int UpsideTradeTotalVolume
    {
      get
      {
        return _upsideTradeTotalVolume;
      }
      set
      {
        __isset.upsideTradeTotalVolume = true;
        SetProperty(ref _upsideTradeTotalVolume, value);
      }
    }

    public double UpsideTradeAveragePrice
    {
      get
      {
        return _upsideTradeAveragePrice;
      }
      set
      {
        __isset.upsideTradeAveragePrice = true;
        SetProperty(ref _upsideTradeAveragePrice, value);
      }
    }

    public int UpsideTradeRestingVolume
    {
      get
      {
        return _upsideTradeRestingVolume;
      }
      set
      {
        __isset.upsideTradeRestingVolume = true;
        SetProperty(ref _upsideTradeRestingVolume, value);
      }
    }

    public int TradeListTotalVolume
    {
      get
      {
        return _tradeListTotalVolume;
      }
      set
      {
        __isset.tradeListTotalVolume = true;
        SetProperty(ref _tradeListTotalVolume, value);
      }
    }

    public double TradeListAveragePrice
    {
      get
      {
        return _tradeListAveragePrice;
      }
      set
      {
        __isset.tradeListAveragePrice = true;
        SetProperty(ref _tradeListAveragePrice, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool upsideTradeTotalVolume;
      public bool upsideTradeAveragePrice;
      public bool upsideTradeRestingVolume;
      public bool tradeListTotalVolume;
      public bool tradeListAveragePrice;
    }

    public HostingExecOrderTradeSummary() {
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
            if (field.Type == TType.I32) {
              UpsideTradeTotalVolume = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.Double) {
              UpsideTradeAveragePrice = iprot.ReadDouble();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.I32) {
              UpsideTradeRestingVolume = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 6:
            if (field.Type == TType.I32) {
              TradeListTotalVolume = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 7:
            if (field.Type == TType.Double) {
              TradeListAveragePrice = iprot.ReadDouble();
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
      TStruct struc = new TStruct("HostingExecOrderTradeSummary");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.upsideTradeTotalVolume) {
        field.Name = "upsideTradeTotalVolume";
        field.Type = TType.I32;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(UpsideTradeTotalVolume);
        oprot.WriteFieldEnd();
      }
      if (__isset.upsideTradeAveragePrice) {
        field.Name = "upsideTradeAveragePrice";
        field.Type = TType.Double;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteDouble(UpsideTradeAveragePrice);
        oprot.WriteFieldEnd();
      }
      if (__isset.upsideTradeRestingVolume) {
        field.Name = "upsideTradeRestingVolume";
        field.Type = TType.I32;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(UpsideTradeRestingVolume);
        oprot.WriteFieldEnd();
      }
      if (__isset.tradeListTotalVolume) {
        field.Name = "tradeListTotalVolume";
        field.Type = TType.I32;
        field.ID = 6;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(TradeListTotalVolume);
        oprot.WriteFieldEnd();
      }
      if (__isset.tradeListAveragePrice) {
        field.Name = "tradeListAveragePrice";
        field.Type = TType.Double;
        field.ID = 7;
        oprot.WriteFieldBegin(field);
        oprot.WriteDouble(TradeListAveragePrice);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("HostingExecOrderTradeSummary(");
      sb.Append("UpsideTradeTotalVolume: ");
      sb.Append(UpsideTradeTotalVolume);
      sb.Append(",UpsideTradeAveragePrice: ");
      sb.Append(UpsideTradeAveragePrice);
      sb.Append(",UpsideTradeRestingVolume: ");
      sb.Append(UpsideTradeRestingVolume);
      sb.Append(",TradeListTotalVolume: ");
      sb.Append(TradeListTotalVolume);
      sb.Append(",TradeListAveragePrice: ");
      sb.Append(TradeListAveragePrice);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
