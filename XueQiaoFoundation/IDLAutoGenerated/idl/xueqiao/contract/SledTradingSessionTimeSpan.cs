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

namespace xueqiao.contract
{

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class SledTradingSessionTimeSpan : TBase, INotifyPropertyChanged
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
    private long _timeSpanId;
    private Days _startDay;
    private string _startTime;
    private Days _endDay;
    private string _endTime;
    private TimeSpanState _timeSpanState;
    private long _tradeSessionId;
    private long _createTimestamp;
    private long _lastModifyTimestamp;

    public long TimeSpanId
    {
      get
      {
        return _timeSpanId;
      }
      set
      {
        __isset.timeSpanId = true;
        SetProperty(ref _timeSpanId, value);
      }
    }

    /// <summary>
    /// 
    /// <seealso cref="Days"/>
    /// </summary>
    public Days StartDay
    {
      get
      {
        return _startDay;
      }
      set
      {
        __isset.startDay = true;
        SetProperty(ref _startDay, value);
      }
    }

    public string StartTime
    {
      get
      {
        return _startTime;
      }
      set
      {
        __isset.startTime = true;
        SetProperty(ref _startTime, value);
      }
    }

    /// <summary>
    /// 
    /// <seealso cref="Days"/>
    /// </summary>
    public Days EndDay
    {
      get
      {
        return _endDay;
      }
      set
      {
        __isset.endDay = true;
        SetProperty(ref _endDay, value);
      }
    }

    public string EndTime
    {
      get
      {
        return _endTime;
      }
      set
      {
        __isset.endTime = true;
        SetProperty(ref _endTime, value);
      }
    }

    /// <summary>
    /// 
    /// <seealso cref="TimeSpanState"/>
    /// </summary>
    public TimeSpanState TimeSpanState
    {
      get
      {
        return _timeSpanState;
      }
      set
      {
        __isset.timeSpanState = true;
        SetProperty(ref _timeSpanState, value);
      }
    }

    public long TradeSessionId
    {
      get
      {
        return _tradeSessionId;
      }
      set
      {
        __isset.tradeSessionId = true;
        SetProperty(ref _tradeSessionId, value);
      }
    }

    public long CreateTimestamp
    {
      get
      {
        return _createTimestamp;
      }
      set
      {
        __isset.createTimestamp = true;
        SetProperty(ref _createTimestamp, value);
      }
    }

    public long LastModifyTimestamp
    {
      get
      {
        return _lastModifyTimestamp;
      }
      set
      {
        __isset.lastModifyTimestamp = true;
        SetProperty(ref _lastModifyTimestamp, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool timeSpanId;
      public bool startDay;
      public bool startTime;
      public bool endDay;
      public bool endTime;
      public bool timeSpanState;
      public bool tradeSessionId;
      public bool createTimestamp;
      public bool lastModifyTimestamp;
    }

    public SledTradingSessionTimeSpan() {
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
              TimeSpanId = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.I32) {
              StartDay = (Days)iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.String) {
              StartTime = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.I32) {
              EndDay = (Days)iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 5:
            if (field.Type == TType.String) {
              EndTime = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 6:
            if (field.Type == TType.I32) {
              TimeSpanState = (TimeSpanState)iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 7:
            if (field.Type == TType.I64) {
              TradeSessionId = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 8:
            if (field.Type == TType.I64) {
              CreateTimestamp = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 9:
            if (field.Type == TType.I64) {
              LastModifyTimestamp = iprot.ReadI64();
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
      TStruct struc = new TStruct("SledTradingSessionTimeSpan");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.timeSpanId) {
        field.Name = "timeSpanId";
        field.Type = TType.I64;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(TimeSpanId);
        oprot.WriteFieldEnd();
      }
      if (__isset.startDay) {
        field.Name = "startDay";
        field.Type = TType.I32;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32((int)StartDay);
        oprot.WriteFieldEnd();
      }
      if (StartTime != null && __isset.startTime) {
        field.Name = "startTime";
        field.Type = TType.String;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(StartTime);
        oprot.WriteFieldEnd();
      }
      if (__isset.endDay) {
        field.Name = "endDay";
        field.Type = TType.I32;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32((int)EndDay);
        oprot.WriteFieldEnd();
      }
      if (EndTime != null && __isset.endTime) {
        field.Name = "endTime";
        field.Type = TType.String;
        field.ID = 5;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(EndTime);
        oprot.WriteFieldEnd();
      }
      if (__isset.timeSpanState) {
        field.Name = "timeSpanState";
        field.Type = TType.I32;
        field.ID = 6;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32((int)TimeSpanState);
        oprot.WriteFieldEnd();
      }
      if (__isset.tradeSessionId) {
        field.Name = "tradeSessionId";
        field.Type = TType.I64;
        field.ID = 7;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(TradeSessionId);
        oprot.WriteFieldEnd();
      }
      if (__isset.createTimestamp) {
        field.Name = "createTimestamp";
        field.Type = TType.I64;
        field.ID = 8;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(CreateTimestamp);
        oprot.WriteFieldEnd();
      }
      if (__isset.lastModifyTimestamp) {
        field.Name = "lastModifyTimestamp";
        field.Type = TType.I64;
        field.ID = 9;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(LastModifyTimestamp);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("SledTradingSessionTimeSpan(");
      sb.Append("TimeSpanId: ");
      sb.Append(TimeSpanId);
      sb.Append(",StartDay: ");
      sb.Append(StartDay);
      sb.Append(",StartTime: ");
      sb.Append(StartTime);
      sb.Append(",EndDay: ");
      sb.Append(EndDay);
      sb.Append(",EndTime: ");
      sb.Append(EndTime);
      sb.Append(",TimeSpanState: ");
      sb.Append(TimeSpanState);
      sb.Append(",TradeSessionId: ");
      sb.Append(TradeSessionId);
      sb.Append(",CreateTimestamp: ");
      sb.Append(CreateTimestamp);
      sb.Append(",LastModifyTimestamp: ");
      sb.Append(LastModifyTimestamp);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
