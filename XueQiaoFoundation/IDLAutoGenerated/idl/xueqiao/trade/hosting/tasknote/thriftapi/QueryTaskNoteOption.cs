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
  public partial class QueryTaskNoteOption : TBase, INotifyPropertyChanged
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
    private THashSet<long> _key1;
    private THashSet<long> _key2;
    private THashSet<string> _key3;

    /// <summary>
    /// 
    /// <seealso cref="HostingTaskNoteType"/>
    /// </summary>
    public HostingTaskNoteType NoteType { get; set; }

    public THashSet<long> Key1
    {
      get
      {
        return _key1;
      }
      set
      {
        __isset.key1 = true;
        SetProperty(ref _key1, value);
      }
    }

    public THashSet<long> Key2
    {
      get
      {
        return _key2;
      }
      set
      {
        __isset.key2 = true;
        SetProperty(ref _key2, value);
      }
    }

    public THashSet<string> Key3
    {
      get
      {
        return _key3;
      }
      set
      {
        __isset.key3 = true;
        SetProperty(ref _key3, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool key1;
      public bool key2;
      public bool key3;
    }

    public QueryTaskNoteOption() {
    }

    public QueryTaskNoteOption(HostingTaskNoteType noteType) : this() {
      this.NoteType = noteType;
    }

    public void Read (TProtocol iprot)
    {
      bool isset_noteType = false;
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
              isset_noteType = true;
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.Set) {
              {
                Key1 = new THashSet<long>();
                TSet _set0 = iprot.ReadSetBegin();
                for( int _i1 = 0; _i1 < _set0.Count; ++_i1)
                {
                  long _elem2 = 0;
                  _elem2 = iprot.ReadI64();
                  Key1.Add(_elem2);
                }
                iprot.ReadSetEnd();
              }
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.Set) {
              {
                Key2 = new THashSet<long>();
                TSet _set3 = iprot.ReadSetBegin();
                for( int _i4 = 0; _i4 < _set3.Count; ++_i4)
                {
                  long _elem5 = 0;
                  _elem5 = iprot.ReadI64();
                  Key2.Add(_elem5);
                }
                iprot.ReadSetEnd();
              }
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.Set) {
              {
                Key3 = new THashSet<string>();
                TSet _set6 = iprot.ReadSetBegin();
                for( int _i7 = 0; _i7 < _set6.Count; ++_i7)
                {
                  string _elem8 = null;
                  _elem8 = iprot.ReadString();
                  Key3.Add(_elem8);
                }
                iprot.ReadSetEnd();
              }
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
      if (!isset_noteType)
        throw new TProtocolException(TProtocolException.INVALID_DATA);
    }

    public void Write(TProtocol oprot) {
      TStruct struc = new TStruct("QueryTaskNoteOption");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      field.Name = "noteType";
      field.Type = TType.I32;
      field.ID = 1;
      oprot.WriteFieldBegin(field);
      oprot.WriteI32((int)NoteType);
      oprot.WriteFieldEnd();
      if (Key1 != null && __isset.key1) {
        field.Name = "key1";
        field.Type = TType.Set;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteSetBegin(new TSet(TType.I64, Key1.Count));
          foreach (long _iter9 in Key1)
          {
            oprot.WriteI64(_iter9);
          }
          oprot.WriteSetEnd();
        }
        oprot.WriteFieldEnd();
      }
      if (Key2 != null && __isset.key2) {
        field.Name = "key2";
        field.Type = TType.Set;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteSetBegin(new TSet(TType.I64, Key2.Count));
          foreach (long _iter10 in Key2)
          {
            oprot.WriteI64(_iter10);
          }
          oprot.WriteSetEnd();
        }
        oprot.WriteFieldEnd();
      }
      if (Key3 != null && __isset.key3) {
        field.Name = "key3";
        field.Type = TType.Set;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteSetBegin(new TSet(TType.String, Key3.Count));
          foreach (string _iter11 in Key3)
          {
            oprot.WriteString(_iter11);
          }
          oprot.WriteSetEnd();
        }
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("QueryTaskNoteOption(");
      sb.Append("NoteType: ");
      sb.Append(NoteType);
      sb.Append(",Key1: ");
      if (Key1 == null)
      {
        sb.Append("<null>");
      }
      else
      {
        sb.Append("[");
        foreach (long _iter12 in Key1)
        {
          sb.Append(_iter12.ToString());
          sb.Append(", ");
        }
        sb.Append("]");
      }
      sb.Append(",Key2: ");
      if (Key2 == null)
      {
        sb.Append("<null>");
      }
      else
      {
        sb.Append("[");
        foreach (long _iter13 in Key2)
        {
          sb.Append(_iter13.ToString());
          sb.Append(", ");
        }
        sb.Append("]");
      }
      sb.Append(",Key3: ");
      if (Key3 == null)
      {
        sb.Append("<null>");
      }
      else
      {
        sb.Append("[");
        foreach (string _iter14 in Key3)
        {
          sb.Append(_iter14.ToString());
          sb.Append(", ");
        }
        sb.Append("]");
      }
      sb.Append(")");
      return sb.ToString();
    }

  }

}