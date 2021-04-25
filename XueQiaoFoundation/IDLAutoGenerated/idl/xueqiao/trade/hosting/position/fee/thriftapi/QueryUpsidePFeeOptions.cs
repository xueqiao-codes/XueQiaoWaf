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
  public partial class QueryUpsidePFeeOptions : TBase, INotifyPropertyChanged
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
    private string _exchangeMic;
    private long _commodityId;
    private string _contractCode;

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

    public string ExchangeMic
    {
      get
      {
        return _exchangeMic;
      }
      set
      {
        __isset.exchangeMic = true;
        SetProperty(ref _exchangeMic, value);
      }
    }

    public long CommodityId
    {
      get
      {
        return _commodityId;
      }
      set
      {
        __isset.commodityId = true;
        SetProperty(ref _commodityId, value);
      }
    }

    public string ContractCode
    {
      get
      {
        return _contractCode;
      }
      set
      {
        __isset.contractCode = true;
        SetProperty(ref _contractCode, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool subAccountId;
      public bool exchangeMic;
      public bool commodityId;
      public bool contractCode;
    }

    public QueryUpsidePFeeOptions() {
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
            if (field.Type == TType.String) {
              ExchangeMic = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.I64) {
              CommodityId = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.String) {
              ContractCode = iprot.ReadString();
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
      TStruct struc = new TStruct("QueryUpsidePFeeOptions");
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
      if (ExchangeMic != null && __isset.exchangeMic) {
        field.Name = "exchangeMic";
        field.Type = TType.String;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(ExchangeMic);
        oprot.WriteFieldEnd();
      }
      if (__isset.commodityId) {
        field.Name = "commodityId";
        field.Type = TType.I64;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(CommodityId);
        oprot.WriteFieldEnd();
      }
      if (ContractCode != null && __isset.contractCode) {
        field.Name = "contractCode";
        field.Type = TType.String;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(ContractCode);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("QueryUpsidePFeeOptions(");
      sb.Append("SubAccountId: ");
      sb.Append(SubAccountId);
      sb.Append(",ExchangeMic: ");
      sb.Append(ExchangeMic);
      sb.Append(",CommodityId: ");
      sb.Append(CommodityId);
      sb.Append(",ContractCode: ");
      sb.Append(ContractCode);
      sb.Append(")");
      return sb.ToString();
    }

  }

}