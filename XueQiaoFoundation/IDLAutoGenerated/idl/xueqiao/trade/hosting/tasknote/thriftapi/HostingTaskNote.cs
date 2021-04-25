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

namespace xueqiao.trade.hosting.tasknote.thriftapi
{

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class HostingTaskNote : TBase, INotifyPropertyChanged
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
    private HostingTaskNoteType _noteType;
    private HostingTaskNoteKey _noteKey;
    private string _noteContent;
    private long _createTimestampMs;
    private long _lastmodifyTimestampMs;

    /// <summary>
    /// 
    /// <seealso cref="HostingTaskNoteType"/>
    /// </summary>
    public HostingTaskNoteType NoteType
    {
      get
      {
        return _noteType;
      }
      set
      {
        __isset.noteType = true;
        SetProperty(ref _noteType, value);
      }
    }

    public HostingTaskNoteKey NoteKey
    {
      get
      {
        return _noteKey;
      }
      set
      {
        __isset.noteKey = true;
        SetProperty(ref _noteKey, value);
      }
    }

    public string NoteContent
    {
      get
      {
        return _noteContent;
      }
      set
      {
        __isset.noteContent = true;
        SetProperty(ref _noteContent, value);
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

    public long LastmodifyTimestampMs
    {
      get
      {
        return _lastmodifyTimestampMs;
      }
      set
      {
        __isset.lastmodifyTimestampMs = true;
        SetProperty(ref _lastmodifyTimestampMs, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool noteType;
      public bool noteKey;
      public bool noteContent;
      public bool createTimestampMs;
      public bool lastmodifyTimestampMs;
    }

    public HostingTaskNote() {
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
            if (field.Type == TType.I32) {
              NoteType = (HostingTaskNoteType)iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.Struct) {
              NoteKey = new HostingTaskNoteKey();
              NoteKey.Read(iprot);
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.String) {
              NoteContent = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.I64) {
              CreateTimestampMs = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 5:
            if (field.Type == TType.I64) {
              LastmodifyTimestampMs = iprot.ReadI64();
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
      TStruct struc = new TStruct("HostingTaskNote");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.noteType) {
        field.Name = "noteType";
        field.Type = TType.I32;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32((int)NoteType);
        oprot.WriteFieldEnd();
      }
      if (NoteKey != null && __isset.noteKey) {
        field.Name = "noteKey";
        field.Type = TType.Struct;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        NoteKey.Write(oprot);
        oprot.WriteFieldEnd();
      }
      if (NoteContent != null && __isset.noteContent) {
        field.Name = "noteContent";
        field.Type = TType.String;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(NoteContent);
        oprot.WriteFieldEnd();
      }
      if (__isset.createTimestampMs) {
        field.Name = "createTimestampMs";
        field.Type = TType.I64;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(CreateTimestampMs);
        oprot.WriteFieldEnd();
      }
      if (__isset.lastmodifyTimestampMs) {
        field.Name = "lastmodifyTimestampMs";
        field.Type = TType.I64;
        field.ID = 5;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(LastmodifyTimestampMs);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("HostingTaskNote(");
      sb.Append("NoteType: ");
      sb.Append(NoteType);
      sb.Append(",NoteKey: ");
      sb.Append(NoteKey== null ? "<null>" : NoteKey.ToString());
      sb.Append(",NoteContent: ");
      sb.Append(NoteContent);
      sb.Append(",CreateTimestampMs: ");
      sb.Append(CreateTimestampMs);
      sb.Append(",LastmodifyTimestampMs: ");
      sb.Append(LastmodifyTimestampMs);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
