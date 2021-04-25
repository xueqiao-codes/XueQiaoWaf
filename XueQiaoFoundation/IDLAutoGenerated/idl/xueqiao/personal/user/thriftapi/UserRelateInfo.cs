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
  public partial class UserRelateInfo : TBase, INotifyPropertyChanged
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
    private long _personalUserId;
    private long _companyId;
    private long _companyUserId;
    private long _createTimestamp;
    private long _lastModifyTimestamp;
    private bool _linked;

    public long PersonalUserId
    {
      get
      {
        return _personalUserId;
      }
      set
      {
        __isset.personalUserId = true;
        SetProperty(ref _personalUserId, value);
      }
    }

    public long CompanyId
    {
      get
      {
        return _companyId;
      }
      set
      {
        __isset.companyId = true;
        SetProperty(ref _companyId, value);
      }
    }

    public long CompanyUserId
    {
      get
      {
        return _companyUserId;
      }
      set
      {
        __isset.companyUserId = true;
        SetProperty(ref _companyUserId, value);
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

    public bool Linked
    {
      get
      {
        return _linked;
      }
      set
      {
        __isset.linked = true;
        SetProperty(ref _linked, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool personalUserId;
      public bool companyId;
      public bool companyUserId;
      public bool createTimestamp;
      public bool lastModifyTimestamp;
      public bool linked;
    }

    public UserRelateInfo() {
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
              PersonalUserId = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.I64) {
              CompanyId = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.I64) {
              CompanyUserId = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.I64) {
              CreateTimestamp = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 5:
            if (field.Type == TType.I64) {
              LastModifyTimestamp = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 6:
            if (field.Type == TType.Bool) {
              Linked = iprot.ReadBool();
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
      TStruct struc = new TStruct("UserRelateInfo");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.personalUserId) {
        field.Name = "personalUserId";
        field.Type = TType.I64;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(PersonalUserId);
        oprot.WriteFieldEnd();
      }
      if (__isset.companyId) {
        field.Name = "companyId";
        field.Type = TType.I64;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(CompanyId);
        oprot.WriteFieldEnd();
      }
      if (__isset.companyUserId) {
        field.Name = "companyUserId";
        field.Type = TType.I64;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(CompanyUserId);
        oprot.WriteFieldEnd();
      }
      if (__isset.createTimestamp) {
        field.Name = "createTimestamp";
        field.Type = TType.I64;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(CreateTimestamp);
        oprot.WriteFieldEnd();
      }
      if (__isset.lastModifyTimestamp) {
        field.Name = "lastModifyTimestamp";
        field.Type = TType.I64;
        field.ID = 5;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(LastModifyTimestamp);
        oprot.WriteFieldEnd();
      }
      if (__isset.linked) {
        field.Name = "linked";
        field.Type = TType.Bool;
        field.ID = 6;
        oprot.WriteFieldBegin(field);
        oprot.WriteBool(Linked);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("UserRelateInfo(");
      sb.Append("PersonalUserId: ");
      sb.Append(PersonalUserId);
      sb.Append(",CompanyId: ");
      sb.Append(CompanyId);
      sb.Append(",CompanyUserId: ");
      sb.Append(CompanyUserId);
      sb.Append(",CreateTimestamp: ");
      sb.Append(CreateTimestamp);
      sb.Append(",LastModifyTimestamp: ");
      sb.Append(LastModifyTimestamp);
      sb.Append(",Linked: ");
      sb.Append(Linked);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
