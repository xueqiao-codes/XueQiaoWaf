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

namespace xueqiao.trade.hosting.proxy
{

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class ProxyLoginReq : TBase, INotifyPropertyChanged
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
    private string _companyCode;
    private string _userName;
    private string _passwordMd5;
    private string _companyGroupCode;
    private string _verifyCode;

    public string CompanyCode
    {
      get
      {
        return _companyCode;
      }
      set
      {
        __isset.companyCode = true;
        SetProperty(ref _companyCode, value);
      }
    }

    public string UserName
    {
      get
      {
        return _userName;
      }
      set
      {
        __isset.userName = true;
        SetProperty(ref _userName, value);
      }
    }

    public string PasswordMd5
    {
      get
      {
        return _passwordMd5;
      }
      set
      {
        __isset.passwordMd5 = true;
        SetProperty(ref _passwordMd5, value);
      }
    }

    public string CompanyGroupCode
    {
      get
      {
        return _companyGroupCode;
      }
      set
      {
        __isset.companyGroupCode = true;
        SetProperty(ref _companyGroupCode, value);
      }
    }

    public string VerifyCode
    {
      get
      {
        return _verifyCode;
      }
      set
      {
        __isset.verifyCode = true;
        SetProperty(ref _verifyCode, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool companyCode;
      public bool userName;
      public bool passwordMd5;
      public bool companyGroupCode;
      public bool verifyCode;
    }

    public ProxyLoginReq() {
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
            if (field.Type == TType.String) {
              CompanyCode = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.String) {
              UserName = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.String) {
              PasswordMd5 = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.String) {
              CompanyGroupCode = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 5:
            if (field.Type == TType.String) {
              VerifyCode = iprot.ReadString();
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
      TStruct struc = new TStruct("ProxyLoginReq");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (CompanyCode != null && __isset.companyCode) {
        field.Name = "companyCode";
        field.Type = TType.String;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(CompanyCode);
        oprot.WriteFieldEnd();
      }
      if (UserName != null && __isset.userName) {
        field.Name = "userName";
        field.Type = TType.String;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(UserName);
        oprot.WriteFieldEnd();
      }
      if (PasswordMd5 != null && __isset.passwordMd5) {
        field.Name = "passwordMd5";
        field.Type = TType.String;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(PasswordMd5);
        oprot.WriteFieldEnd();
      }
      if (CompanyGroupCode != null && __isset.companyGroupCode) {
        field.Name = "companyGroupCode";
        field.Type = TType.String;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(CompanyGroupCode);
        oprot.WriteFieldEnd();
      }
      if (VerifyCode != null && __isset.verifyCode) {
        field.Name = "verifyCode";
        field.Type = TType.String;
        field.ID = 5;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(VerifyCode);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("ProxyLoginReq(");
      sb.Append("CompanyCode: ");
      sb.Append(CompanyCode);
      sb.Append(",UserName: ");
      sb.Append(UserName);
      sb.Append(",PasswordMd5: ");
      sb.Append(PasswordMd5);
      sb.Append(",CompanyGroupCode: ");
      sb.Append(CompanyGroupCode);
      sb.Append(",VerifyCode: ");
      sb.Append(VerifyCode);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
