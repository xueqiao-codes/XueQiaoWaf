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
  public partial class HostingXQComposeLimitOrderLegByFirstLegExtraParam : TBase, INotifyPropertyChanged
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
    private int _revokeDeviatePriceTicks;

    public int RevokeDeviatePriceTicks
    {
      get
      {
        return _revokeDeviatePriceTicks;
      }
      set
      {
        __isset.revokeDeviatePriceTicks = true;
        SetProperty(ref _revokeDeviatePriceTicks, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool revokeDeviatePriceTicks;
    }

    public HostingXQComposeLimitOrderLegByFirstLegExtraParam() {
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
              RevokeDeviatePriceTicks = iprot.ReadI32();
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
      TStruct struc = new TStruct("HostingXQComposeLimitOrderLegByFirstLegExtraParam");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.revokeDeviatePriceTicks) {
        field.Name = "revokeDeviatePriceTicks";
        field.Type = TType.I32;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(RevokeDeviatePriceTicks);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("HostingXQComposeLimitOrderLegByFirstLegExtraParam(");
      sb.Append("RevokeDeviatePriceTicks: ");
      sb.Append(RevokeDeviatePriceTicks);
      sb.Append(")");
      return sb.ToString();
    }

  }

}