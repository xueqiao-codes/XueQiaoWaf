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

namespace xueqiao.trade.hosting.position.fee.thriftapi
{

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class XQGeneralMarginSettings : TBase, INotifyPropertyChanged
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
    private long _subAccountId;
    private FeeCalculateType _type;
    private MarginInfo _marginDelta;
    private bool _isSync;
    private long _createTimestampMs;
    private long _lastmodifyTimestampMs;

    public long SubAccountId
    {
      get
      {
        return _subAccountId;
      }
      set
      {
        __isset.subAccountId = true;
        SetProperty(ref _subAccountId, value);
      }
    }

    /// <summary>
    /// 
    /// <seealso cref="FeeCalculateType"/>
    /// </summary>
    public FeeCalculateType Type
    {
      get
      {
        return _type;
      }
      set
      {
        __isset.type = true;
        SetProperty(ref _type, value);
      }
    }

    public MarginInfo MarginDelta
    {
      get
      {
        return _marginDelta;
      }
      set
      {
        __isset.marginDelta = true;
        SetProperty(ref _marginDelta, value);
      }
    }

    public bool IsSync
    {
      get
      {
        return _isSync;
      }
      set
      {
        __isset.isSync = true;
        SetProperty(ref _isSync, value);
      }
    }

    public long CreateTimestampMs
    {
      get
      {
        return _createTimestampMs;
      }
      set
      {
        __isset.createTimestampMs = true;
        SetProperty(ref _createTimestampMs, value);
      }
    }

    public long LastmodifyTimestampMs
    {
      get
      {
        return _lastmodifyTimestampMs;
      }
      set
      {
        __isset.lastmodifyTimestampMs = true;
        SetProperty(ref _lastmodifyTimestampMs, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool subAccountId;
      public bool type;
      public bool marginDelta;
      public bool isSync;
      public bool createTimestampMs;
      public bool lastmodifyTimestampMs;
    }

    public XQGeneralMarginSettings() {
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
              SubAccountId = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.I32) {
              Type = (FeeCalculateType)iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 10:
            if (field.Type == TType.Struct) {
              MarginDelta = new MarginInfo();
              MarginDelta.Read(iprot);
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 12:
            if (field.Type == TType.Bool) {
              IsSync = iprot.ReadBool();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 20:
            if (field.Type == TType.I64) {
              CreateTimestampMs = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 21:
            if (field.Type == TType.I64) {
              LastmodifyTimestampMs = iprot.ReadI64();
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
      TStruct struc = new TStruct("XQGeneralMarginSettings");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.subAccountId) {
        field.Name = "subAccountId";
        field.Type = TType.I64;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(SubAccountId);
        oprot.WriteFieldEnd();
      }
      if (__isset.type) {
        field.Name = "type";
        field.Type = TType.I32;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32((int)Type);
        oprot.WriteFieldEnd();
      }
      if (MarginDelta != null && __isset.marginDelta) {
        field.Name = "marginDelta";
        field.Type = TType.Struct;
        field.ID = 10;
        oprot.WriteFieldBegin(field);
        MarginDelta.Write(oprot);
        oprot.WriteFieldEnd();
      }
      if (__isset.isSync) {
        field.Name = "isSync";
        field.Type = TType.Bool;
        field.ID = 12;
        oprot.WriteFieldBegin(field);
        oprot.WriteBool(IsSync);
        oprot.WriteFieldEnd();
      }
      if (__isset.createTimestampMs) {
        field.Name = "createTimestampMs";
        field.Type = TType.I64;
        field.ID = 20;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(CreateTimestampMs);
        oprot.WriteFieldEnd();
      }
      if (__isset.lastmodifyTimestampMs) {
        field.Name = "lastmodifyTimestampMs";
        field.Type = TType.I64;
        field.ID = 21;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(LastmodifyTimestampMs);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("XQGeneralMarginSettings(");
      sb.Append("SubAccountId: ");
      sb.Append(SubAccountId);
      sb.Append(",Type: ");
      sb.Append(Type);
      sb.Append(",MarginDelta: ");
      sb.Append(MarginDelta== null ? "<null>" : MarginDelta.ToString());
      sb.Append(",IsSync: ");
      sb.Append(IsSync);
      sb.Append(",CreateTimestampMs: ");
      sb.Append(CreateTimestampMs);
      sb.Append(",LastmodifyTimestampMs: ");
      sb.Append(LastmodifyTimestampMs);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
