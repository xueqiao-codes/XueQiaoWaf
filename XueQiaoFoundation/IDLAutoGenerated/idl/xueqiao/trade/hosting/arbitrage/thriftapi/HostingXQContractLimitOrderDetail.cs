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
  public partial class HostingXQContractLimitOrderDetail : TBase, INotifyPropertyChanged
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
    private HostingXQTradeDirection _direction;
    private double _limitPrice;
    private int _quantity;
    private HostingXQEffectDate _effectDate;

    /// <summary>
    /// 
    /// <seealso cref="HostingXQTradeDirection"/>
    /// </summary>
    public HostingXQTradeDirection Direction
    {
      get
      {
        return _direction;
      }
      set
      {
        __isset.direction = true;
        SetProperty(ref _direction, value);
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

    public int Quantity
    {
      get
      {
        return _quantity;
      }
      set
      {
        __isset.quantity = true;
        SetProperty(ref _quantity, value);
      }
    }

    public HostingXQEffectDate EffectDate
    {
      get
      {
        return _effectDate;
      }
      set
      {
        __isset.effectDate = true;
        SetProperty(ref _effectDate, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool direction;
      public bool limitPrice;
      public bool quantity;
      public bool effectDate;
    }

    public HostingXQContractLimitOrderDetail() {
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
              Direction = (HostingXQTradeDirection)iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.Double) {
              LimitPrice = iprot.ReadDouble();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.I32) {
              Quantity = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.Struct) {
              EffectDate = new HostingXQEffectDate();
              EffectDate.Read(iprot);
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
      TStruct struc = new TStruct("HostingXQContractLimitOrderDetail");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.direction) {
        field.Name = "direction";
        field.Type = TType.I32;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32((int)Direction);
        oprot.WriteFieldEnd();
      }
      if (__isset.limitPrice) {
        field.Name = "limitPrice";
        field.Type = TType.Double;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteDouble(LimitPrice);
        oprot.WriteFieldEnd();
      }
      if (__isset.quantity) {
        field.Name = "quantity";
        field.Type = TType.I32;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(Quantity);
        oprot.WriteFieldEnd();
      }
      if (EffectDate != null && __isset.effectDate) {
        field.Name = "effectDate";
        field.Type = TType.Struct;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        EffectDate.Write(oprot);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("HostingXQContractLimitOrderDetail(");
      sb.Append("Direction: ");
      sb.Append(Direction);
      sb.Append(",LimitPrice: ");
      sb.Append(LimitPrice);
      sb.Append(",Quantity: ");
      sb.Append(Quantity);
      sb.Append(",EffectDate: ");
      sb.Append(EffectDate== null ? "<null>" : EffectDate.ToString());
      sb.Append(")");
      return sb.ToString();
    }

  }

}
