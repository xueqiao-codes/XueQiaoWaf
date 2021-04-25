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
  public partial class HostingExecTradeLegInfo : TBase, INotifyPropertyChanged
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
    private double _legTradePrice;
    private int _legTradeVolume;
    private string _legTradeDateTime;
    private HostingExecUpsideTradeID _legUpsideTradeId;
    private HostingExecTradeDirection _legUpsideTradeDirection;

    public double LegTradePrice
    {
      get
      {
        return _legTradePrice;
      }
      set
      {
        __isset.legTradePrice = true;
        SetProperty(ref _legTradePrice, value);
      }
    }

    public int LegTradeVolume
    {
      get
      {
        return _legTradeVolume;
      }
      set
      {
        __isset.legTradeVolume = true;
        SetProperty(ref _legTradeVolume, value);
      }
    }

    public string LegTradeDateTime
    {
      get
      {
        return _legTradeDateTime;
      }
      set
      {
        __isset.legTradeDateTime = true;
        SetProperty(ref _legTradeDateTime, value);
      }
    }

    public HostingExecUpsideTradeID LegUpsideTradeId
    {
      get
      {
        return _legUpsideTradeId;
      }
      set
      {
        __isset.legUpsideTradeId = true;
        SetProperty(ref _legUpsideTradeId, value);
      }
    }

    /// <summary>
    /// 
    /// <seealso cref="HostingExecTradeDirection"/>
    /// </summary>
    public HostingExecTradeDirection LegUpsideTradeDirection
    {
      get
      {
        return _legUpsideTradeDirection;
      }
      set
      {
        __isset.legUpsideTradeDirection = true;
        SetProperty(ref _legUpsideTradeDirection, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool legTradePrice;
      public bool legTradeVolume;
      public bool legTradeDateTime;
      public bool legUpsideTradeId;
      public bool legUpsideTradeDirection;
    }

    public HostingExecTradeLegInfo() {
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
              LegTradePrice = iprot.ReadDouble();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.I32) {
              LegTradeVolume = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.String) {
              LegTradeDateTime = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.Struct) {
              LegUpsideTradeId = new HostingExecUpsideTradeID();
              LegUpsideTradeId.Read(iprot);
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 5:
            if (field.Type == TType.I32) {
              LegUpsideTradeDirection = (HostingExecTradeDirection)iprot.ReadI32();
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
      TStruct struc = new TStruct("HostingExecTradeLegInfo");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.legTradePrice) {
        field.Name = "legTradePrice";
        field.Type = TType.Double;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteDouble(LegTradePrice);
        oprot.WriteFieldEnd();
      }
      if (__isset.legTradeVolume) {
        field.Name = "legTradeVolume";
        field.Type = TType.I32;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(LegTradeVolume);
        oprot.WriteFieldEnd();
      }
      if (LegTradeDateTime != null && __isset.legTradeDateTime) {
        field.Name = "legTradeDateTime";
        field.Type = TType.String;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(LegTradeDateTime);
        oprot.WriteFieldEnd();
      }
      if (LegUpsideTradeId != null && __isset.legUpsideTradeId) {
        field.Name = "legUpsideTradeId";
        field.Type = TType.Struct;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        LegUpsideTradeId.Write(oprot);
        oprot.WriteFieldEnd();
      }
      if (__isset.legUpsideTradeDirection) {
        field.Name = "legUpsideTradeDirection";
        field.Type = TType.I32;
        field.ID = 5;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32((int)LegUpsideTradeDirection);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("HostingExecTradeLegInfo(");
      sb.Append("LegTradePrice: ");
      sb.Append(LegTradePrice);
      sb.Append(",LegTradeVolume: ");
      sb.Append(LegTradeVolume);
      sb.Append(",LegTradeDateTime: ");
      sb.Append(LegTradeDateTime);
      sb.Append(",LegUpsideTradeId: ");
      sb.Append(LegUpsideTradeId== null ? "<null>" : LegUpsideTradeId.ToString());
      sb.Append(",LegUpsideTradeDirection: ");
      sb.Append(LegUpsideTradeDirection);
      sb.Append(")");
      return sb.ToString();
    }

  }

}