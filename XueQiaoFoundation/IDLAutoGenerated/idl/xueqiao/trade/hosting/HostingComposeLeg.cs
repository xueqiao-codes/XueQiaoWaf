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
  public partial class HostingComposeLeg : TBase, INotifyPropertyChanged
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
    private string _variableName;
    private int _quantity;
    private HostingComposeLegTradeDirection _legTradeDirection;
    private string _sledContractCode;
    private long _sledCommodityId;
    private short _sledCommodityType;
    private string _sledCommodityCode;
    private string _sledExchangeMic;

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

    public string VariableName
    {
      get
      {
        return _variableName;
      }
      set
      {
        __isset.variableName = true;
        SetProperty(ref _variableName, value);
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

    /// <summary>
    /// 
    /// <seealso cref="HostingComposeLegTradeDirection"/>
    /// </summary>
    public HostingComposeLegTradeDirection LegTradeDirection
    {
      get
      {
        return _legTradeDirection;
      }
      set
      {
        __isset.legTradeDirection = true;
        SetProperty(ref _legTradeDirection, value);
      }
    }

    public string SledContractCode
    {
      get
      {
        return _sledContractCode;
      }
      set
      {
        __isset.sledContractCode = true;
        SetProperty(ref _sledContractCode, value);
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

    public short SledCommodityType
    {
      get
      {
        return _sledCommodityType;
      }
      set
      {
        __isset.sledCommodityType = true;
        SetProperty(ref _sledCommodityType, value);
      }
    }

    public string SledCommodityCode
    {
      get
      {
        return _sledCommodityCode;
      }
      set
      {
        __isset.sledCommodityCode = true;
        SetProperty(ref _sledCommodityCode, value);
      }
    }

    public string SledExchangeMic
    {
      get
      {
        return _sledExchangeMic;
      }
      set
      {
        __isset.sledExchangeMic = true;
        SetProperty(ref _sledExchangeMic, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool sledContractId;
      public bool variableName;
      public bool quantity;
      public bool legTradeDirection;
      public bool sledContractCode;
      public bool sledCommodityId;
      public bool sledCommodityType;
      public bool sledCommodityCode;
      public bool sledExchangeMic;
    }

    public HostingComposeLeg() {
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
            if (field.Type == TType.String) {
              VariableName = iprot.ReadString();
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
          case 5:
            if (field.Type == TType.I32) {
              LegTradeDirection = (HostingComposeLegTradeDirection)iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 6:
            if (field.Type == TType.String) {
              SledContractCode = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 7:
            if (field.Type == TType.I64) {
              SledCommodityId = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 8:
            if (field.Type == TType.I16) {
              SledCommodityType = iprot.ReadI16();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 9:
            if (field.Type == TType.String) {
              SledCommodityCode = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 10:
            if (field.Type == TType.String) {
              SledExchangeMic = iprot.ReadString();
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
      TStruct struc = new TStruct("HostingComposeLeg");
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
      if (VariableName != null && __isset.variableName) {
        field.Name = "variableName";
        field.Type = TType.String;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(VariableName);
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
      if (__isset.legTradeDirection) {
        field.Name = "legTradeDirection";
        field.Type = TType.I32;
        field.ID = 5;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32((int)LegTradeDirection);
        oprot.WriteFieldEnd();
      }
      if (SledContractCode != null && __isset.sledContractCode) {
        field.Name = "sledContractCode";
        field.Type = TType.String;
        field.ID = 6;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(SledContractCode);
        oprot.WriteFieldEnd();
      }
      if (__isset.sledCommodityId) {
        field.Name = "sledCommodityId";
        field.Type = TType.I64;
        field.ID = 7;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(SledCommodityId);
        oprot.WriteFieldEnd();
      }
      if (__isset.sledCommodityType) {
        field.Name = "sledCommodityType";
        field.Type = TType.I16;
        field.ID = 8;
        oprot.WriteFieldBegin(field);
        oprot.WriteI16(SledCommodityType);
        oprot.WriteFieldEnd();
      }
      if (SledCommodityCode != null && __isset.sledCommodityCode) {
        field.Name = "sledCommodityCode";
        field.Type = TType.String;
        field.ID = 9;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(SledCommodityCode);
        oprot.WriteFieldEnd();
      }
      if (SledExchangeMic != null && __isset.sledExchangeMic) {
        field.Name = "sledExchangeMic";
        field.Type = TType.String;
        field.ID = 10;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(SledExchangeMic);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("HostingComposeLeg(");
      sb.Append("SledContractId: ");
      sb.Append(SledContractId);
      sb.Append(",VariableName: ");
      sb.Append(VariableName);
      sb.Append(",Quantity: ");
      sb.Append(Quantity);
      sb.Append(",LegTradeDirection: ");
      sb.Append(LegTradeDirection);
      sb.Append(",SledContractCode: ");
      sb.Append(SledContractCode);
      sb.Append(",SledCommodityId: ");
      sb.Append(SledCommodityId);
      sb.Append(",SledCommodityType: ");
      sb.Append(SledCommodityType);
      sb.Append(",SledCommodityCode: ");
      sb.Append(SledCommodityCode);
      sb.Append(",SledExchangeMic: ");
      sb.Append(SledExchangeMic);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
