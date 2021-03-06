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
  public partial class TTimeSpan : TBase, INotifyPropertyChanged
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
    private string _timespan;
    private TimeSpanState _timeSpanState;
    private long _startTimestamp;
    private string _startTimeString;
    private long _endTimestamp;
    private string _endTimeString;

    public string Timespan
    {
      get
      {
        return _timespan;
      }
      set
      {
        __isset.timespan = true;
        SetProperty(ref _timespan, value);
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

    public long StartTimestamp
    {
      get
      {
        return _startTimestamp;
      }
      set
      {
        __isset.startTimestamp = true;
        SetProperty(ref _startTimestamp, value);
      }
    }

    public string StartTimeString
    {
      get
      {
        return _startTimeString;
      }
      set
      {
        __isset.startTimeString = true;
        SetProperty(ref _startTimeString, value);
      }
    }

    public long EndTimestamp
    {
      get
      {
        return _endTimestamp;
      }
      set
      {
        __isset.endTimestamp = true;
        SetProperty(ref _endTimestamp, value);
      }
    }

    public string EndTimeString
    {
      get
      {
        return _endTimeString;
      }
      set
      {
        __isset.endTimeString = true;
        SetProperty(ref _endTimeString, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool timespan;
      public bool timeSpanState;
      public bool startTimestamp;
      public bool startTimeString;
      public bool endTimestamp;
      public bool endTimeString;
    }

    public TTimeSpan() {
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
              Timespan = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.I32) {
              TimeSpanState = (TimeSpanState)iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.I64) {
              StartTimestamp = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.String) {
              StartTimeString = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 5:
            if (field.Type == TType.I64) {
              EndTimestamp = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 6:
            if (field.Type == TType.String) {
              EndTimeString = iprot.ReadString();
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
      TStruct struc = new TStruct("TTimeSpan");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (Timespan != null && __isset.timespan) {
        field.Name = "timespan";
        field.Type = TType.String;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Timespan);
        oprot.WriteFieldEnd();
      }
      if (__isset.timeSpanState) {
        field.Name = "timeSpanState";
        field.Type = TType.I32;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32((int)TimeSpanState);
        oprot.WriteFieldEnd();
      }
      if (__isset.startTimestamp) {
        field.Name = "startTimestamp";
        field.Type = TType.I64;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(StartTimestamp);
        oprot.WriteFieldEnd();
      }
      if (StartTimeString != null && __isset.startTimeString) {
        field.Name = "startTimeString";
        field.Type = TType.String;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(StartTimeString);
        oprot.WriteFieldEnd();
      }
      if (__isset.endTimestamp) {
        field.Name = "endTimestamp";
        field.Type = TType.I64;
        field.ID = 5;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(EndTimestamp);
        oprot.WriteFieldEnd();
      }
      if (EndTimeString != null && __isset.endTimeString) {
        field.Name = "endTimeString";
        field.Type = TType.String;
        field.ID = 6;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(EndTimeString);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("TTimeSpan(");
      sb.Append("Timespan: ");
      sb.Append(Timespan);
      sb.Append(",TimeSpanState: ");
      sb.Append(TimeSpanState);
      sb.Append(",StartTimestamp: ");
      sb.Append(StartTimestamp);
      sb.Append(",StartTimeString: ");
      sb.Append(StartTimeString);
      sb.Append(",EndTimestamp: ");
      sb.Append(EndTimestamp);
      sb.Append(",EndTimeString: ");
      sb.Append(EndTimeString);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
