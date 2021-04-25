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

namespace xueqiao.trade.hosting.events
{

  /// <summary>
  /// 实时统计持仓  汇总  变化信息事件，使用 MessageAgent 推送
  /// 推送重新计算的持仓汇总(StatPositionSummary)
  /// </summary>
  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class StatPositionSummaryChangedEvent : TBase, INotifyPropertyChanged
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
    private xueqiao.trade.hosting.position.statis.StatPositionSummary _statPositionSummary;
    private long _eventCreateTimestampMs;
    private StatPositionEventType _eventType;

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

    public xueqiao.trade.hosting.position.statis.StatPositionSummary StatPositionSummary
    {
      get
      {
        return _statPositionSummary;
      }
      set
      {
        __isset.statPositionSummary = true;
        SetProperty(ref _statPositionSummary, value);
      }
    }

    public long EventCreateTimestampMs
    {
      get
      {
        return _eventCreateTimestampMs;
      }
      set
      {
        __isset.eventCreateTimestampMs = true;
        SetProperty(ref _eventCreateTimestampMs, value);
      }
    }

    /// <summary>
    /// 
    /// <seealso cref="StatPositionEventType"/>
    /// </summary>
    public StatPositionEventType EventType
    {
      get
      {
        return _eventType;
      }
      set
      {
        __isset.eventType = true;
        SetProperty(ref _eventType, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool subAccountId;
      public bool statPositionSummary;
      public bool eventCreateTimestampMs;
      public bool eventType;
    }

    public StatPositionSummaryChangedEvent() {
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
            if (field.Type == TType.Struct) {
              StatPositionSummary = new xueqiao.trade.hosting.position.statis.StatPositionSummary();
              StatPositionSummary.Read(iprot);
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.I64) {
              EventCreateTimestampMs = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.I32) {
              EventType = (StatPositionEventType)iprot.ReadI32();
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
      TStruct struc = new TStruct("StatPositionSummaryChangedEvent");
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
      if (StatPositionSummary != null && __isset.statPositionSummary) {
        field.Name = "statPositionSummary";
        field.Type = TType.Struct;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        StatPositionSummary.Write(oprot);
        oprot.WriteFieldEnd();
      }
      if (__isset.eventCreateTimestampMs) {
        field.Name = "eventCreateTimestampMs";
        field.Type = TType.I64;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(EventCreateTimestampMs);
        oprot.WriteFieldEnd();
      }
      if (__isset.eventType) {
        field.Name = "eventType";
        field.Type = TType.I32;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32((int)EventType);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("StatPositionSummaryChangedEvent(");
      sb.Append("SubAccountId: ");
      sb.Append(SubAccountId);
      sb.Append(",StatPositionSummary: ");
      sb.Append(StatPositionSummary== null ? "<null>" : StatPositionSummary.ToString());
      sb.Append(",EventCreateTimestampMs: ");
      sb.Append(EventCreateTimestampMs);
      sb.Append(",EventType: ");
      sb.Append(EventType);
      sb.Append(")");
      return sb.ToString();
    }

  }

}