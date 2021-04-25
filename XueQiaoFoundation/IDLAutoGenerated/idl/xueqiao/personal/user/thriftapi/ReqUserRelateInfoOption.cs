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
  public partial class ReqUserRelateInfoOption : TBase, INotifyPropertyChanged
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
    private long _personUserId;
    private long _companyId;
    private long _companyUserId;

    public long PersonUserId
    {
      get
      {
        return _personUserId;
      }
      set
      {
        __isset.personUserId = true;
        SetProperty(ref _personUserId, value);
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


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool personUserId;
      public bool companyId;
      public bool companyUserId;
    }

    public ReqUserRelateInfoOption() {
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
              PersonUserId = iprot.ReadI64();
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
          default: 
            TProtocolUtil.Skip(iprot, field.Type);
            break;
        }
        iprot.ReadFieldEnd();
      }
      iprot.ReadStructEnd();
    }

    public void Write(TProtocol oprot) {
      TStruct struc = new TStruct("ReqUserRelateInfoOption");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.personUserId) {
        field.Name = "personUserId";
        field.Type = TType.I64;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(PersonUserId);
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
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("ReqUserRelateInfoOption(");
      sb.Append("PersonUserId: ");
      sb.Append(PersonUserId);
      sb.Append(",CompanyId: ");
      sb.Append(CompanyId);
      sb.Append(",CompanyUserId: ");
      sb.Append(CompanyUserId);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
