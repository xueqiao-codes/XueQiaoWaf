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
  public partial class HostingXQComposeLimitOrderLegChaseParam : TBase, INotifyPropertyChanged
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
    private int _ticks;
    private int _times;
    private double _protectPriceRatio;

    public int Ticks
    {
      get
      {
        return _ticks;
      }
      set
      {
        __isset.ticks = true;
        SetProperty(ref _ticks, value);
      }
    }

    public int Times
    {
      get
      {
        return _times;
      }
      set
      {
        __isset.times = true;
        SetProperty(ref _times, value);
      }
    }

    public double ProtectPriceRatio
    {
      get
      {
        return _protectPriceRatio;
      }
      set
      {
        __isset.protectPriceRatio = true;
        SetProperty(ref _protectPriceRatio, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool ticks;
      public bool times;
      public bool protectPriceRatio;
    }

    public HostingXQComposeLimitOrderLegChaseParam() {
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
              Ticks = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.I32) {
              Times = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.Double) {
              ProtectPriceRatio = iprot.ReadDouble();
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
      TStruct struc = new TStruct("HostingXQComposeLimitOrderLegChaseParam");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.ticks) {
        field.Name = "ticks";
        field.Type = TType.I32;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(Ticks);
        oprot.WriteFieldEnd();
      }
      if (__isset.times) {
        field.Name = "times";
        field.Type = TType.I32;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(Times);
        oprot.WriteFieldEnd();
      }
      if (__isset.protectPriceRatio) {
        field.Name = "protectPriceRatio";
        field.Type = TType.Double;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteDouble(ProtectPriceRatio);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("HostingXQComposeLimitOrderLegChaseParam(");
      sb.Append("Ticks: ");
      sb.Append(Ticks);
      sb.Append(",Times: ");
      sb.Append(Times);
      sb.Append(",ProtectPriceRatio: ");
      sb.Append(ProtectPriceRatio);
      sb.Append(")");
      return sb.ToString();
    }

  }

}