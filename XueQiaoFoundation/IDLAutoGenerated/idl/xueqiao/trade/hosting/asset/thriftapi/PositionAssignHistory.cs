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

namespace xueqiao.trade.hosting.asset.thriftapi
{

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class PositionAssignHistory : TBase, INotifyPropertyChanged
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
    private long _assignId;
    private string _content;
    private long _createTimestampMs;
    private long _lastModifyTimestampMs;

    public long AssignId
    {
      get
      {
        return _assignId;
      }
      set
      {
        __isset.assignId = true;
        SetProperty(ref _assignId, value);
      }
    }

    public string Content
    {
      get
      {
        return _content;
      }
      set
      {
        __isset.content = true;
        SetProperty(ref _content, value);
      }
    }

    public long CreateTimestampMs
    {
      get
      {
        return _createTimestampMs;
      }
      set
      {
        __isset.createTimestampMs = true;
        SetProperty(ref _createTimestampMs, value);
      }
    }

    public long LastModifyTimestampMs
    {
      get
      {
        return _lastModifyTimestampMs;
      }
      set
      {
        __isset.lastModifyTimestampMs = true;
        SetProperty(ref _lastModifyTimestampMs, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool assignId;
      public bool content;
      public bool createTimestampMs;
      public bool lastModifyTimestampMs;
    }

    public PositionAssignHistory() {
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
              AssignId = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.String) {
              Content = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.I64) {
              CreateTimestampMs = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.I64) {
              LastModifyTimestampMs = iprot.ReadI64();
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
      TStruct struc = new TStruct("PositionAssignHistory");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.assignId) {
        field.Name = "assignId";
        field.Type = TType.I64;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(AssignId);
        oprot.WriteFieldEnd();
      }
      if (Content != null && __isset.content) {
        field.Name = "content";
        field.Type = TType.String;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Content);
        oprot.WriteFieldEnd();
      }
      if (__isset.createTimestampMs) {
        field.Name = "createTimestampMs";
        field.Type = TType.I64;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(CreateTimestampMs);
        oprot.WriteFieldEnd();
      }
      if (__isset.lastModifyTimestampMs) {
        field.Name = "lastModifyTimestampMs";
        field.Type = TType.I64;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(LastModifyTimestampMs);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("PositionAssignHistory(");
      sb.Append("AssignId: ");
      sb.Append(AssignId);
      sb.Append(",Content: ");
      sb.Append(Content);
      sb.Append(",CreateTimestampMs: ");
      sb.Append(CreateTimestampMs);
      sb.Append(",LastModifyTimestampMs: ");
      sb.Append(LastModifyTimestampMs);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
