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

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class HostingRiskRuleItem : TBase, INotifyPropertyChanged
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
    private bool _ruleEnabled;
    private HostingRiskRuleItemValue _alarmValue;
    private HostingRiskRuleItemValue _forbiddenOpenPositionValue;

    public bool RuleEnabled
    {
      get
      {
        return _ruleEnabled;
      }
      set
      {
        __isset.ruleEnabled = true;
        SetProperty(ref _ruleEnabled, value);
      }
    }

    public HostingRiskRuleItemValue AlarmValue
    {
      get
      {
        return _alarmValue;
      }
      set
      {
        __isset.alarmValue = true;
        SetProperty(ref _alarmValue, value);
      }
    }

    public HostingRiskRuleItemValue ForbiddenOpenPositionValue
    {
      get
      {
        return _forbiddenOpenPositionValue;
      }
      set
      {
        __isset.forbiddenOpenPositionValue = true;
        SetProperty(ref _forbiddenOpenPositionValue, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool ruleEnabled;
      public bool alarmValue;
      public bool forbiddenOpenPositionValue;
    }

    public HostingRiskRuleItem() {
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
            if (field.Type == TType.Bool) {
              RuleEnabled = iprot.ReadBool();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.Struct) {
              AlarmValue = new HostingRiskRuleItemValue();
              AlarmValue.Read(iprot);
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.Struct) {
              ForbiddenOpenPositionValue = new HostingRiskRuleItemValue();
              ForbiddenOpenPositionValue.Read(iprot);
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
      TStruct struc = new TStruct("HostingRiskRuleItem");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.ruleEnabled) {
        field.Name = "ruleEnabled";
        field.Type = TType.Bool;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteBool(RuleEnabled);
        oprot.WriteFieldEnd();
      }
      if (AlarmValue != null && __isset.alarmValue) {
        field.Name = "alarmValue";
        field.Type = TType.Struct;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        AlarmValue.Write(oprot);
        oprot.WriteFieldEnd();
      }
      if (ForbiddenOpenPositionValue != null && __isset.forbiddenOpenPositionValue) {
        field.Name = "forbiddenOpenPositionValue";
        field.Type = TType.Struct;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        ForbiddenOpenPositionValue.Write(oprot);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("HostingRiskRuleItem(");
      sb.Append("RuleEnabled: ");
      sb.Append(RuleEnabled);
      sb.Append(",AlarmValue: ");
      sb.Append(AlarmValue== null ? "<null>" : AlarmValue.ToString());
      sb.Append(",ForbiddenOpenPositionValue: ");
      sb.Append(ForbiddenOpenPositionValue== null ? "<null>" : ForbiddenOpenPositionValue.ToString());
      sb.Append(")");
      return sb.ToString();
    }

  }

}