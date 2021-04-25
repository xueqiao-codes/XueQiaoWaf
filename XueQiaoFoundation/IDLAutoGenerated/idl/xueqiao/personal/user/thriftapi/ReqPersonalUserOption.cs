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

namespace xueqiao.personal.user.thriftapi
{

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class ReqPersonalUserOption : TBase, INotifyPropertyChanged
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
    private THashSet<long> _personalUserIds;
    private string _tel;

    public THashSet<long> PersonalUserIds
    {
      get
      {
        return _personalUserIds;
      }
      set
      {
        __isset.personalUserIds = true;
        SetProperty(ref _personalUserIds, value);
      }
    }

    public string Tel
    {
      get
      {
        return _tel;
      }
      set
      {
        __isset.tel = true;
        SetProperty(ref _tel, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool personalUserIds;
      public bool tel;
    }

    public ReqPersonalUserOption() {
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
                PersonalUserIds = new THashSet<long>();
                TSet _set0 = iprot.ReadSetBegin();
                for( int _i1 = 0; _i1 < _set0.Count; ++_i1)
                {
                  long _elem2 = 0;
                  _elem2 = iprot.ReadI64();
                  PersonalUserIds.Add(_elem2);
                }
                iprot.ReadSetEnd();
              }
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.String) {
              Tel = iprot.ReadString();
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
      TStruct struc = new TStruct("ReqPersonalUserOption");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (PersonalUserIds != null && __isset.personalUserIds) {
        field.Name = "personalUserIds";
        field.Type = TType.Set;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteSetBegin(new TSet(TType.I64, PersonalUserIds.Count));
          foreach (long _iter3 in PersonalUserIds)
          {
            oprot.WriteI64(_iter3);
          }
          oprot.WriteSetEnd();
        }
        oprot.WriteFieldEnd();
      }
      if (Tel != null && __isset.tel) {
        field.Name = "tel";
        field.Type = TType.String;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Tel);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("ReqPersonalUserOption(");
      sb.Append("PersonalUserIds: ");
      if (PersonalUserIds == null)
      {
        sb.Append("<null>");
      }
      else
      {
        sb.Append("[");
        foreach (long _iter4 in PersonalUserIds)
        {
          sb.Append(_iter4.ToString());
          sb.Append(", ");
        }
        sb.Append("]");
      }
      sb.Append(",Tel: ");
      sb.Append(Tel);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
