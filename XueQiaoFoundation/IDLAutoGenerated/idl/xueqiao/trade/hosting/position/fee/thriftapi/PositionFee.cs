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

namespace xueqiao.trade.hosting.position.fee.thriftapi
{

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class PositionFee : TBase, INotifyPropertyChanged
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
    private long _subAccountId;
    private long _contractId;
    private MarginInfo _margin;
    private CommissionInfo _commission;

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

    public long ContractId
    {
      get
      {
        return _contractId;
      }
      set
      {
        __isset.contractId = true;
        SetProperty(ref _contractId, value);
      }
    }

    public MarginInfo Margin
    {
      get
      {
        return _margin;
      }
      set
      {
        __isset.margin = true;
        SetProperty(ref _margin, value);
      }
    }

    public CommissionInfo Commission
    {
      get
      {
        return _commission;
      }
      set
      {
        __isset.commission = true;
        SetProperty(ref _commission, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool subAccountId;
      public bool contractId;
      public bool margin;
      public bool commission;
    }

    public PositionFee() {
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
              SubAccountId = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.I64) {
              ContractId = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 10:
            if (field.Type == TType.Struct) {
              Margin = new MarginInfo();
              Margin.Read(iprot);
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 11:
            if (field.Type == TType.Struct) {
              Commission = new CommissionInfo();
              Commission.Read(iprot);
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
      TStruct struc = new TStruct("PositionFee");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.subAccountId) {
        field.Name = "subAccountId";
        field.Type = TType.I64;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(SubAccountId);
        oprot.WriteFieldEnd();
      }
      if (__isset.contractId) {
        field.Name = "contractId";
        field.Type = TType.I64;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(ContractId);
        oprot.WriteFieldEnd();
      }
      if (Margin != null && __isset.margin) {
        field.Name = "margin";
        field.Type = TType.Struct;
        field.ID = 10;
        oprot.WriteFieldBegin(field);
        Margin.Write(oprot);
        oprot.WriteFieldEnd();
      }
      if (Commission != null && __isset.commission) {
        field.Name = "commission";
        field.Type = TType.Struct;
        field.ID = 11;
        oprot.WriteFieldBegin(field);
        Commission.Write(oprot);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("PositionFee(");
      sb.Append("SubAccountId: ");
      sb.Append(SubAccountId);
      sb.Append(",ContractId: ");
      sb.Append(ContractId);
      sb.Append(",Margin: ");
      sb.Append(Margin== null ? "<null>" : Margin.ToString());
      sb.Append(",Commission: ");
      sb.Append(Commission== null ? "<null>" : Commission.ToString());
      sb.Append(")");
      return sb.ToString();
    }

  }

}
