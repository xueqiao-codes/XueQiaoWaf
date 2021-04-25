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

namespace xueqiao.trade.hosting.risk.manager.thriftapi
{

  /// <summary>
  /// 风控条目的数据信息
  /// </summary>
  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class HostingRiskItemDataInfo : TBase, INotifyPropertyChanged
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
    private string _itemId;
    private long _sledCommodityId;
    private long _sledContractId;
    private HostingRiskRuleItemValue _itemValue;
    private bool _alarmTriggered;
    private bool _forbiddenOpenPositionTriggered;

    public string ItemId
    {
      get
      {
        return _itemId;
      }
      set
      {
        __isset.itemId = true;
        SetProperty(ref _itemId, value);
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

    public HostingRiskRuleItemValue ItemValue
    {
      get
      {
        return _itemValue;
      }
      set
      {
        __isset.itemValue = true;
        SetProperty(ref _itemValue, value);
      }
    }

    public bool AlarmTriggered
    {
      get
      {
        return _alarmTriggered;
      }
      set
      {
        __isset.alarmTriggered = true;
        SetProperty(ref _alarmTriggered, value);
      }
    }

    public bool ForbiddenOpenPositionTriggered
    {
      get
      {
        return _forbiddenOpenPositionTriggered;
      }
      set
      {
        __isset.forbiddenOpenPositionTriggered = true;
        SetProperty(ref _forbiddenOpenPositionTriggered, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool itemId;
      public bool sledCommodityId;
      public bool sledContractId;
      public bool itemValue;
      public bool alarmTriggered;
      public bool forbiddenOpenPositionTriggered;
    }

    public HostingRiskItemDataInfo() {
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
              ItemId = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.I64) {
              SledCommodityId = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.I64) {
              SledContractId = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.Struct) {
              ItemValue = new HostingRiskRuleItemValue();
              ItemValue.Read(iprot);
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 5:
            if (field.Type == TType.Bool) {
              AlarmTriggered = iprot.ReadBool();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 6:
            if (field.Type == TType.Bool) {
              ForbiddenOpenPositionTriggered = iprot.ReadBool();
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
      TStruct struc = new TStruct("HostingRiskItemDataInfo");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (ItemId != null && __isset.itemId) {
        field.Name = "itemId";
        field.Type = TType.String;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(ItemId);
        oprot.WriteFieldEnd();
      }
      if (__isset.sledCommodityId) {
        field.Name = "sledCommodityId";
        field.Type = TType.I64;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(SledCommodityId);
        oprot.WriteFieldEnd();
      }
      if (__isset.sledContractId) {
        field.Name = "sledContractId";
        field.Type = TType.I64;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(SledContractId);
        oprot.WriteFieldEnd();
      }
      if (ItemValue != null && __isset.itemValue) {
        field.Name = "itemValue";
        field.Type = TType.Struct;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        ItemValue.Write(oprot);
        oprot.WriteFieldEnd();
      }
      if (__isset.alarmTriggered) {
        field.Name = "alarmTriggered";
        field.Type = TType.Bool;
        field.ID = 5;
        oprot.WriteFieldBegin(field);
        oprot.WriteBool(AlarmTriggered);
        oprot.WriteFieldEnd();
      }
      if (__isset.forbiddenOpenPositionTriggered) {
        field.Name = "forbiddenOpenPositionTriggered";
        field.Type = TType.Bool;
        field.ID = 6;
        oprot.WriteFieldBegin(field);
        oprot.WriteBool(ForbiddenOpenPositionTriggered);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("HostingRiskItemDataInfo(");
      sb.Append("ItemId: ");
      sb.Append(ItemId);
      sb.Append(",SledCommodityId: ");
      sb.Append(SledCommodityId);
      sb.Append(",SledContractId: ");
      sb.Append(SledContractId);
      sb.Append(",ItemValue: ");
      sb.Append(ItemValue== null ? "<null>" : ItemValue.ToString());
      sb.Append(",AlarmTriggered: ");
      sb.Append(AlarmTriggered);
      sb.Append(",ForbiddenOpenPositionTriggered: ");
      sb.Append(ForbiddenOpenPositionTriggered);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
