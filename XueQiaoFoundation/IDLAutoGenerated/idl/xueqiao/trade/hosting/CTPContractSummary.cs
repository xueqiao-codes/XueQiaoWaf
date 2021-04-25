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
  public partial class CTPContractSummary : TBase, INotifyPropertyChanged
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
    private string _ctpExchangeCode;
    private string _ctpCommodityCode;
    private short _ctpCommodityType;
    private string _ctpContractCode;

    public string CtpExchangeCode
    {
      get
      {
        return _ctpExchangeCode;
      }
      set
      {
        __isset.ctpExchangeCode = true;
        SetProperty(ref _ctpExchangeCode, value);
      }
    }

    public string CtpCommodityCode
    {
      get
      {
        return _ctpCommodityCode;
      }
      set
      {
        __isset.ctpCommodityCode = true;
        SetProperty(ref _ctpCommodityCode, value);
      }
    }

    public short CtpCommodityType
    {
      get
      {
        return _ctpCommodityType;
      }
      set
      {
        __isset.ctpCommodityType = true;
        SetProperty(ref _ctpCommodityType, value);
      }
    }

    public string CtpContractCode
    {
      get
      {
        return _ctpContractCode;
      }
      set
      {
        __isset.ctpContractCode = true;
        SetProperty(ref _ctpContractCode, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool ctpExchangeCode;
      public bool ctpCommodityCode;
      public bool ctpCommodityType;
      public bool ctpContractCode;
    }

    public CTPContractSummary() {
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
            if (field.Type == TType.String) {
              CtpExchangeCode = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.String) {
              CtpCommodityCode = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.I16) {
              CtpCommodityType = iprot.ReadI16();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.String) {
              CtpContractCode = iprot.ReadString();
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
      TStruct struc = new TStruct("CTPContractSummary");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (CtpExchangeCode != null && __isset.ctpExchangeCode) {
        field.Name = "ctpExchangeCode";
        field.Type = TType.String;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(CtpExchangeCode);
        oprot.WriteFieldEnd();
      }
      if (CtpCommodityCode != null && __isset.ctpCommodityCode) {
        field.Name = "ctpCommodityCode";
        field.Type = TType.String;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(CtpCommodityCode);
        oprot.WriteFieldEnd();
      }
      if (__isset.ctpCommodityType) {
        field.Name = "ctpCommodityType";
        field.Type = TType.I16;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteI16(CtpCommodityType);
        oprot.WriteFieldEnd();
      }
      if (CtpContractCode != null && __isset.ctpContractCode) {
        field.Name = "ctpContractCode";
        field.Type = TType.String;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(CtpContractCode);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("CTPContractSummary(");
      sb.Append("CtpExchangeCode: ");
      sb.Append(CtpExchangeCode);
      sb.Append(",CtpCommodityCode: ");
      sb.Append(CtpCommodityCode);
      sb.Append(",CtpCommodityType: ");
      sb.Append(CtpCommodityType);
      sb.Append(",CtpContractCode: ");
      sb.Append(CtpContractCode);
      sb.Append(")");
      return sb.ToString();
    }

  }

}