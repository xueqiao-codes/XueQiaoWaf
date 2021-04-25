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
  public partial class QueryHostingComposeViewDetailOption : TBase, INotifyPropertyChanged
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
    private int _subUserId;
    private long _composeGraphId;
    private string _aliasNamePartical;

    public int SubUserId
    {
      get
      {
        return _subUserId;
      }
      set
      {
        __isset.subUserId = true;
        SetProperty(ref _subUserId, value);
      }
    }

    public long ComposeGraphId
    {
      get
      {
        return _composeGraphId;
      }
      set
      {
        __isset.composeGraphId = true;
        SetProperty(ref _composeGraphId, value);
      }
    }

    public string AliasNamePartical
    {
      get
      {
        return _aliasNamePartical;
      }
      set
      {
        __isset.aliasNamePartical = true;
        SetProperty(ref _aliasNamePartical, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool subUserId;
      public bool composeGraphId;
      public bool aliasNamePartical;
    }

    public QueryHostingComposeViewDetailOption() {
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
              SubUserId = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.I64) {
              ComposeGraphId = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.String) {
              AliasNamePartical = iprot.ReadString();
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
      TStruct struc = new TStruct("QueryHostingComposeViewDetailOption");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.subUserId) {
        field.Name = "subUserId";
        field.Type = TType.I32;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(SubUserId);
        oprot.WriteFieldEnd();
      }
      if (__isset.composeGraphId) {
        field.Name = "composeGraphId";
        field.Type = TType.I64;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(ComposeGraphId);
        oprot.WriteFieldEnd();
      }
      if (AliasNamePartical != null && __isset.aliasNamePartical) {
        field.Name = "aliasNamePartical";
        field.Type = TType.String;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(AliasNamePartical);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("QueryHostingComposeViewDetailOption(");
      sb.Append("SubUserId: ");
      sb.Append(SubUserId);
      sb.Append(",ComposeGraphId: ");
      sb.Append(ComposeGraphId);
      sb.Append(",AliasNamePartical: ");
      sb.Append(AliasNamePartical);
      sb.Append(")");
      return sb.ToString();
    }

  }

}