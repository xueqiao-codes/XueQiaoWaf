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
  public partial class FavoriteChart : TBase, INotifyPropertyChanged
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
    private long _favoriteChartId;
    private long _xiaohaChartId;
    private long _parentFolderId;
    private long _personalUserId;
    private string _name;

    public long FavoriteChartId
    {
      get
      {
        return _favoriteChartId;
      }
      set
      {
        __isset.favoriteChartId = true;
        SetProperty(ref _favoriteChartId, value);
      }
    }

    public long XiaohaChartId
    {
      get
      {
        return _xiaohaChartId;
      }
      set
      {
        __isset.xiaohaChartId = true;
        SetProperty(ref _xiaohaChartId, value);
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

    public string Name
    {
      get
      {
        return _name;
      }
      set
      {
        __isset.name = true;
        SetProperty(ref _name, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool favoriteChartId;
      public bool xiaohaChartId;
      public bool parentFolderId;
      public bool personalUserId;
      public bool name;
    }

    public FavoriteChart() {
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
              FavoriteChartId = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.I64) {
              XiaohaChartId = iprot.ReadI64();
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
          case 5:
            if (field.Type == TType.String) {
              Name = iprot.ReadString();
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
      TStruct struc = new TStruct("FavoriteChart");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.favoriteChartId) {
        field.Name = "favoriteChartId";
        field.Type = TType.I64;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(FavoriteChartId);
        oprot.WriteFieldEnd();
      }
      if (__isset.xiaohaChartId) {
        field.Name = "xiaohaChartId";
        field.Type = TType.I64;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(XiaohaChartId);
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
      if (Name != null && __isset.name) {
        field.Name = "name";
        field.Type = TType.String;
        field.ID = 5;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Name);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("FavoriteChart(");
      sb.Append("FavoriteChartId: ");
      sb.Append(FavoriteChartId);
      sb.Append(",XiaohaChartId: ");
      sb.Append(XiaohaChartId);
      sb.Append(",ParentFolderId: ");
      sb.Append(ParentFolderId);
      sb.Append(",PersonalUserId: ");
      sb.Append(PersonalUserId);
      sb.Append(",Name: ");
      sb.Append(Name);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
