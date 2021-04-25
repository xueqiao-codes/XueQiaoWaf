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
  public partial class DateTimeSpan : TBase, INotifyPropertyChanged
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
    private string _date;
    private List<TTimeSpan> _tTimeSpan;

    public string Date
    {
      get
      {
        return _date;
      }
      set
      {
        __isset.date = true;
        SetProperty(ref _date, value);
      }
    }

    public List<TTimeSpan> TTimeSpan
    {
      get
      {
        return _tTimeSpan;
      }
      set
      {
        __isset.tTimeSpan = true;
        SetProperty(ref _tTimeSpan, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool date;
      public bool tTimeSpan;
    }

    public DateTimeSpan() {
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
          case 2:
            if (field.Type == TType.String) {
              Date = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.List) {
              {
                TTimeSpan = new List<TTimeSpan>();
                TList _list65 = iprot.ReadListBegin();
                for( int _i66 = 0; _i66 < _list65.Count; ++_i66)
                {
                  TTimeSpan _elem67 = new TTimeSpan();
                  _elem67 = new TTimeSpan();
                  _elem67.Read(iprot);
                  TTimeSpan.Add(_elem67);
                }
                iprot.ReadListEnd();
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
      TStruct struc = new TStruct("DateTimeSpan");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (Date != null && __isset.date) {
        field.Name = "date";
        field.Type = TType.String;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Date);
        oprot.WriteFieldEnd();
      }
      if (TTimeSpan != null && __isset.tTimeSpan) {
        field.Name = "tTimeSpan";
        field.Type = TType.List;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteListBegin(new TList(TType.Struct, TTimeSpan.Count));
          foreach (TTimeSpan _iter68 in TTimeSpan)
          {
            _iter68.Write(oprot);
          }
          oprot.WriteListEnd();
        }
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("DateTimeSpan(");
      sb.Append("Date: ");
      sb.Append(Date);
      sb.Append(",TTimeSpan: ");
      if (TTimeSpan == null)
      {
        sb.Append("<null>");
      }
      else
      {
        sb.Append("[");
        foreach (TTimeSpan _iter69 in TTimeSpan)
        {
          sb.Append(_iter69.ToString());
          sb.Append(", ");
        }
        sb.Append("]");
      }
      sb.Append(")");
      return sb.ToString();
    }

  }

}
