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
  public partial class HostingXQComposeOrderLimitLegSendOrderExtraParam : TBase, INotifyPropertyChanged
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
    private double _quantityRatio;
    private double _impactCost;

    public double QuantityRatio
    {
      get
      {
        return _quantityRatio;
      }
      set
      {
        __isset.quantityRatio = true;
        SetProperty(ref _quantityRatio, value);
      }
    }

    public double ImpactCost
    {
      get
      {
        return _impactCost;
      }
      set
      {
        __isset.impactCost = true;
        SetProperty(ref _impactCost, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool quantityRatio;
      public bool impactCost;
    }

    public HostingXQComposeOrderLimitLegSendOrderExtraParam() {
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
            if (field.Type == TType.Double) {
              QuantityRatio = iprot.ReadDouble();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.Double) {
              ImpactCost = iprot.ReadDouble();
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
      TStruct struc = new TStruct("HostingXQComposeOrderLimitLegSendOrderExtraParam");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.quantityRatio) {
        field.Name = "quantityRatio";
        field.Type = TType.Double;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteDouble(QuantityRatio);
        oprot.WriteFieldEnd();
      }
      if (__isset.impactCost) {
        field.Name = "impactCost";
        field.Type = TType.Double;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteDouble(ImpactCost);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("HostingXQComposeOrderLimitLegSendOrderExtraParam(");
      sb.Append("QuantityRatio: ");
      sb.Append(QuantityRatio);
      sb.Append(",ImpactCost: ");
      sb.Append(ImpactCost);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
