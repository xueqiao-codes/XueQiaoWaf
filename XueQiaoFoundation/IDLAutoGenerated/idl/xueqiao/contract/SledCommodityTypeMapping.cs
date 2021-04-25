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
  public partial class SledCommodityTypeMapping : TBase, INotifyPropertyChanged
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
    private int _mappingId;
    private xueqiao.contract.standard.SledCommodityType _sledCommodityType;
    private xueqiao.contract.standard.TechPlatform _techPlatform;
    private string _techPlatformCommodityType;
    private long _createTimestamp;
    private long _lastModifyTimestamp;

    public int MappingId
    {
      get
      {
        return _mappingId;
      }
      set
      {
        __isset.mappingId = true;
        SetProperty(ref _mappingId, value);
      }
    }

    /// <summary>
    /// 
    /// <seealso cref="xueqiao.contract.standard.SledCommodityType"/>
    /// </summary>
    public xueqiao.contract.standard.SledCommodityType SledCommodityType
    {
      get
      {
        return _sledCommodityType;
      }
      set
      {
        __isset.sledCommodityType = true;
        SetProperty(ref _sledCommodityType, value);
      }
    }

    /// <summary>
    /// 
    /// <seealso cref="xueqiao.contract.standard.TechPlatform"/>
    /// </summary>
    public xueqiao.contract.standard.TechPlatform TechPlatform
    {
      get
      {
        return _techPlatform;
      }
      set
      {
        __isset.techPlatform = true;
        SetProperty(ref _techPlatform, value);
      }
    }

    public string TechPlatformCommodityType
    {
      get
      {
        return _techPlatformCommodityType;
      }
      set
      {
        __isset.techPlatformCommodityType = true;
        SetProperty(ref _techPlatformCommodityType, value);
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


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool mappingId;
      public bool sledCommodityType;
      public bool techPlatform;
      public bool techPlatformCommodityType;
      public bool createTimestamp;
      public bool lastModifyTimestamp;
    }

    public SledCommodityTypeMapping() {
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
              MappingId = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.I32) {
              SledCommodityType = (xueqiao.contract.standard.SledCommodityType)iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.I32) {
              TechPlatform = (xueqiao.contract.standard.TechPlatform)iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.String) {
              TechPlatformCommodityType = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 5:
            if (field.Type == TType.I64) {
              CreateTimestamp = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 6:
            if (field.Type == TType.I64) {
              LastModifyTimestamp = iprot.ReadI64();
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
      TStruct struc = new TStruct("SledCommodityTypeMapping");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.mappingId) {
        field.Name = "mappingId";
        field.Type = TType.I32;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(MappingId);
        oprot.WriteFieldEnd();
      }
      if (__isset.sledCommodityType) {
        field.Name = "sledCommodityType";
        field.Type = TType.I32;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32((int)SledCommodityType);
        oprot.WriteFieldEnd();
      }
      if (__isset.techPlatform) {
        field.Name = "techPlatform";
        field.Type = TType.I32;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32((int)TechPlatform);
        oprot.WriteFieldEnd();
      }
      if (TechPlatformCommodityType != null && __isset.techPlatformCommodityType) {
        field.Name = "techPlatformCommodityType";
        field.Type = TType.String;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(TechPlatformCommodityType);
        oprot.WriteFieldEnd();
      }
      if (__isset.createTimestamp) {
        field.Name = "createTimestamp";
        field.Type = TType.I64;
        field.ID = 5;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(CreateTimestamp);
        oprot.WriteFieldEnd();
      }
      if (__isset.lastModifyTimestamp) {
        field.Name = "lastModifyTimestamp";
        field.Type = TType.I64;
        field.ID = 6;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(LastModifyTimestamp);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("SledCommodityTypeMapping(");
      sb.Append("MappingId: ");
      sb.Append(MappingId);
      sb.Append(",SledCommodityType: ");
      sb.Append(SledCommodityType);
      sb.Append(",TechPlatform: ");
      sb.Append(TechPlatform);
      sb.Append(",TechPlatformCommodityType: ");
      sb.Append(TechPlatformCommodityType);
      sb.Append(",CreateTimestamp: ");
      sb.Append(CreateTimestamp);
      sb.Append(",LastModifyTimestamp: ");
      sb.Append(LastModifyTimestamp);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
