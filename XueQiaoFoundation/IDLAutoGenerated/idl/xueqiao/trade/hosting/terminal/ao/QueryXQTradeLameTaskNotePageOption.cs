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

namespace xueqiao.trade.hosting.terminal.ao
{

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class QueryXQTradeLameTaskNotePageOption : TBase, INotifyPropertyChanged
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
    private THashSet<long> _subAccountIds;
    private THashSet<long> _xqTradeIds;

    public THashSet<long> SubAccountIds
    {
      get
      {
        return _subAccountIds;
      }
      set
      {
        __isset.subAccountIds = true;
        SetProperty(ref _subAccountIds, value);
      }
    }

    public THashSet<long> XqTradeIds
    {
      get
      {
        return _xqTradeIds;
      }
      set
      {
        __isset.xqTradeIds = true;
        SetProperty(ref _xqTradeIds, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool subAccountIds;
      public bool xqTradeIds;
    }

    public QueryXQTradeLameTaskNotePageOption() {
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
            if (field.Type == TType.Set) {
              {
                SubAccountIds = new THashSet<long>();
                TSet _set61 = iprot.ReadSetBegin();
                for( int _i62 = 0; _i62 < _set61.Count; ++_i62)
                {
                  long _elem63 = 0;
                  _elem63 = iprot.ReadI64();
                  SubAccountIds.Add(_elem63);
                }
                iprot.ReadSetEnd();
              }
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.Set) {
              {
                XqTradeIds = new THashSet<long>();
                TSet _set64 = iprot.ReadSetBegin();
                for( int _i65 = 0; _i65 < _set64.Count; ++_i65)
                {
                  long _elem66 = 0;
                  _elem66 = iprot.ReadI64();
                  XqTradeIds.Add(_elem66);
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
    }

    public void Write(TProtocol oprot) {
      TStruct struc = new TStruct("QueryXQTradeLameTaskNotePageOption");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (SubAccountIds != null && __isset.subAccountIds) {
        field.Name = "subAccountIds";
        field.Type = TType.Set;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteSetBegin(new TSet(TType.I64, SubAccountIds.Count));
          foreach (long _iter67 in SubAccountIds)
          {
            oprot.WriteI64(_iter67);
          }
          oprot.WriteSetEnd();
        }
        oprot.WriteFieldEnd();
      }
      if (XqTradeIds != null && __isset.xqTradeIds) {
        field.Name = "xqTradeIds";
        field.Type = TType.Set;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteSetBegin(new TSet(TType.I64, XqTradeIds.Count));
          foreach (long _iter68 in XqTradeIds)
          {
            oprot.WriteI64(_iter68);
          }
          oprot.WriteSetEnd();
        }
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("QueryXQTradeLameTaskNotePageOption(");
      sb.Append("SubAccountIds: ");
      if (SubAccountIds == null)
      {
        sb.Append("<null>");
      }
      else
      {
        sb.Append("[");
        foreach (long _iter69 in SubAccountIds)
        {
          sb.Append(_iter69.ToString());
          sb.Append(", ");
        }
        sb.Append("]");
      }
      sb.Append(",XqTradeIds: ");
      if (XqTradeIds == null)
      {
        sb.Append("<null>");
      }
      else
      {
        sb.Append("[");
        foreach (long _iter70 in XqTradeIds)
        {
          sb.Append(_iter70.ToString());
          sb.Append(", ");
        }
        sb.Append("]");
      }
      sb.Append(")");
      return sb.ToString();
    }

  }

}
