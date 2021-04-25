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
  public partial class HostingXQOrderDetail : TBase, INotifyPropertyChanged
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
    private HostingXQContractLimitOrderDetail _contractLimitOrderDetail;
    private HostingXQComposeLimitOrderDetail _composeLimitOrderDetail;
    private HostingXQConditionOrderDetail _conditionOrderDetail;
    private HostingXQContractParkedOrderDetail _contractParkedOrderDetail;

    public HostingXQContractLimitOrderDetail ContractLimitOrderDetail
    {
      get
      {
        return _contractLimitOrderDetail;
      }
      set
      {
        __isset.contractLimitOrderDetail = true;
        SetProperty(ref _contractLimitOrderDetail, value);
      }
    }

    public HostingXQComposeLimitOrderDetail ComposeLimitOrderDetail
    {
      get
      {
        return _composeLimitOrderDetail;
      }
      set
      {
        __isset.composeLimitOrderDetail = true;
        SetProperty(ref _composeLimitOrderDetail, value);
      }
    }

    public HostingXQConditionOrderDetail ConditionOrderDetail
    {
      get
      {
        return _conditionOrderDetail;
      }
      set
      {
        __isset.conditionOrderDetail = true;
        SetProperty(ref _conditionOrderDetail, value);
      }
    }

    public HostingXQContractParkedOrderDetail ContractParkedOrderDetail
    {
      get
      {
        return _contractParkedOrderDetail;
      }
      set
      {
        __isset.contractParkedOrderDetail = true;
        SetProperty(ref _contractParkedOrderDetail, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool contractLimitOrderDetail;
      public bool composeLimitOrderDetail;
      public bool conditionOrderDetail;
      public bool contractParkedOrderDetail;
    }

    public HostingXQOrderDetail() {
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
            if (field.Type == TType.Struct) {
              ContractLimitOrderDetail = new HostingXQContractLimitOrderDetail();
              ContractLimitOrderDetail.Read(iprot);
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.Struct) {
              ComposeLimitOrderDetail = new HostingXQComposeLimitOrderDetail();
              ComposeLimitOrderDetail.Read(iprot);
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.Struct) {
              ConditionOrderDetail = new HostingXQConditionOrderDetail();
              ConditionOrderDetail.Read(iprot);
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.Struct) {
              ContractParkedOrderDetail = new HostingXQContractParkedOrderDetail();
              ContractParkedOrderDetail.Read(iprot);
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
      TStruct struc = new TStruct("HostingXQOrderDetail");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (ContractLimitOrderDetail != null && __isset.contractLimitOrderDetail) {
        field.Name = "contractLimitOrderDetail";
        field.Type = TType.Struct;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        ContractLimitOrderDetail.Write(oprot);
        oprot.WriteFieldEnd();
      }
      if (ComposeLimitOrderDetail != null && __isset.composeLimitOrderDetail) {
        field.Name = "composeLimitOrderDetail";
        field.Type = TType.Struct;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        ComposeLimitOrderDetail.Write(oprot);
        oprot.WriteFieldEnd();
      }
      if (ConditionOrderDetail != null && __isset.conditionOrderDetail) {
        field.Name = "conditionOrderDetail";
        field.Type = TType.Struct;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        ConditionOrderDetail.Write(oprot);
        oprot.WriteFieldEnd();
      }
      if (ContractParkedOrderDetail != null && __isset.contractParkedOrderDetail) {
        field.Name = "contractParkedOrderDetail";
        field.Type = TType.Struct;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        ContractParkedOrderDetail.Write(oprot);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("HostingXQOrderDetail(");
      sb.Append("ContractLimitOrderDetail: ");
      sb.Append(ContractLimitOrderDetail== null ? "<null>" : ContractLimitOrderDetail.ToString());
      sb.Append(",ComposeLimitOrderDetail: ");
      sb.Append(ComposeLimitOrderDetail== null ? "<null>" : ComposeLimitOrderDetail.ToString());
      sb.Append(",ConditionOrderDetail: ");
      sb.Append(ConditionOrderDetail== null ? "<null>" : ConditionOrderDetail.ToString());
      sb.Append(",ContractParkedOrderDetail: ");
      sb.Append(ContractParkedOrderDetail== null ? "<null>" : ContractParkedOrderDetail.ToString());
      sb.Append(")");
      return sb.ToString();
    }

  }

}