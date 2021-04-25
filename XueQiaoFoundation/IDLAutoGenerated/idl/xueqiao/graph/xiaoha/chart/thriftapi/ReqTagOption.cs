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

namespace xueqiao.graph.xiaoha.chart.thriftapi
{

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class ReqTagOption : TBase, INotifyPropertyChanged
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
    private long _tagId;
    private string _cnName;
    private string _namePartical;

    public long TagId
    {
      get
      {
        return _tagId;
      }
      set
      {
        __isset.tagId = true;
        SetProperty(ref _tagId, value);
      }
    }

    public string CnName
    {
      get
      {
        return _cnName;
      }
      set
      {
        __isset.cnName = true;
        SetProperty(ref _cnName, value);
      }
    }

    public string NamePartical
    {
      get
      {
        return _namePartical;
      }
      set
      {
        __isset.namePartical = true;
        SetProperty(ref _namePartical, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool tagId;
      public bool cnName;
      public bool namePartical;
    }

    public ReqTagOption() {
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
              TagId = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.String) {
              CnName = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.String) {
              NamePartical = iprot.ReadString();
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
      TStruct struc = new TStruct("ReqTagOption");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.tagId) {
        field.Name = "tagId";
        field.Type = TType.I64;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(TagId);
        oprot.WriteFieldEnd();
      }
      if (CnName != null && __isset.cnName) {
        field.Name = "cnName";
        field.Type = TType.String;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(CnName);
        oprot.WriteFieldEnd();
      }
      if (NamePartical != null && __isset.namePartical) {
        field.Name = "namePartical";
        field.Type = TType.String;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(NamePartical);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("ReqTagOption(");
      sb.Append("TagId: ");
      sb.Append(TagId);
      sb.Append(",CnName: ");
      sb.Append(CnName);
      sb.Append(",NamePartical: ");
      sb.Append(NamePartical);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
