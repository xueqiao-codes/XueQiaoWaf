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
  public partial class FavoriteFolder : TBase, INotifyPropertyChanged
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
    private long _folderId;
    private string _folderName;
    private long _parentFolderId;
    private long _personalUserId;

    public long FolderId
    {
      get
      {
        return _folderId;
      }
      set
      {
        __isset.folderId = true;
        SetProperty(ref _folderId, value);
      }
    }

    public string FolderName
    {
      get
      {
        return _folderName;
      }
      set
      {
        __isset.folderName = true;
        SetProperty(ref _folderName, value);
      }
    }

    public long ParentFolderId
    {
      get
      {
        return _parentFolderId;
      }
      set
      {
        __isset.parentFolderId = true;
        SetProperty(ref _parentFolderId, value);
      }
    }

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


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool folderId;
      public bool folderName;
      public bool parentFolderId;
      public bool personalUserId;
    }

    public FavoriteFolder() {
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
              FolderId = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.String) {
              FolderName = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.I64) {
              ParentFolderId = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.I64) {
              PersonalUserId = iprot.ReadI64();
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
      TStruct struc = new TStruct("FavoriteFolder");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.folderId) {
        field.Name = "folderId";
        field.Type = TType.I64;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(FolderId);
        oprot.WriteFieldEnd();
      }
      if (FolderName != null && __isset.folderName) {
        field.Name = "folderName";
        field.Type = TType.String;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(FolderName);
        oprot.WriteFieldEnd();
      }
      if (__isset.parentFolderId) {
        field.Name = "parentFolderId";
        field.Type = TType.I64;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(ParentFolderId);
        oprot.WriteFieldEnd();
      }
      if (__isset.personalUserId) {
        field.Name = "personalUserId";
        field.Type = TType.I64;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(PersonalUserId);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("FavoriteFolder(");
      sb.Append("FolderId: ");
      sb.Append(FolderId);
      sb.Append(",FolderName: ");
      sb.Append(FolderName);
      sb.Append(",ParentFolderId: ");
      sb.Append(ParentFolderId);
      sb.Append(",PersonalUserId: ");
      sb.Append(PersonalUserId);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
