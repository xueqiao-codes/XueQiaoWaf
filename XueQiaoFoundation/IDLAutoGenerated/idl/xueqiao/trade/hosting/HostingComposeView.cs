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

namespace xueqiao.trade.hosting
{

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class HostingComposeView : TBase, INotifyPropertyChanged
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
    private string _aliasName;
    private HostingComposeViewSource _viewSource;
    private HostingComposeViewSubscribeStatus _subscribeStatus;
    private HostingComposeViewStatus _viewStatus;
    private short _precisionNumber;
    private int _createTimestamp;
    private int _lastmodifyTimestamp;

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

    public string AliasName
    {
      get
      {
        return _aliasName;
      }
      set
      {
        __isset.aliasName = true;
        SetProperty(ref _aliasName, value);
      }
    }

    /// <summary>
    /// 
    /// <seealso cref="HostingComposeViewSource"/>
    /// </summary>
    public HostingComposeViewSource ViewSource
    {
      get
      {
        return _viewSource;
      }
      set
      {
        __isset.viewSource = true;
        SetProperty(ref _viewSource, value);
      }
    }

    /// <summary>
    /// 
    /// <seealso cref="HostingComposeViewSubscribeStatus"/>
    /// </summary>
    public HostingComposeViewSubscribeStatus SubscribeStatus
    {
      get
      {
        return _subscribeStatus;
      }
      set
      {
        __isset.subscribeStatus = true;
        SetProperty(ref _subscribeStatus, value);
      }
    }

    /// <summary>
    /// 
    /// <seealso cref="HostingComposeViewStatus"/>
    /// </summary>
    public HostingComposeViewStatus ViewStatus
    {
      get
      {
        return _viewStatus;
      }
      set
      {
        __isset.viewStatus = true;
        SetProperty(ref _viewStatus, value);
      }
    }

    public short PrecisionNumber
    {
      get
      {
        return _precisionNumber;
      }
      set
      {
        __isset.precisionNumber = true;
        SetProperty(ref _precisionNumber, value);
      }
    }

    public int CreateTimestamp
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

    public int LastmodifyTimestamp
    {
      get
      {
        return _lastmodifyTimestamp;
      }
      set
      {
        __isset.lastmodifyTimestamp = true;
        SetProperty(ref _lastmodifyTimestamp, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool subUserId;
      public bool composeGraphId;
      public bool aliasName;
      public bool viewSource;
      public bool subscribeStatus;
      public bool viewStatus;
      public bool precisionNumber;
      public bool createTimestamp;
      public bool lastmodifyTimestamp;
    }

    public HostingComposeView() {
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
              AliasName = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.I32) {
              ViewSource = (HostingComposeViewSource)iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 5:
            if (field.Type == TType.I32) {
              SubscribeStatus = (HostingComposeViewSubscribeStatus)iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 6:
            if (field.Type == TType.I32) {
              ViewStatus = (HostingComposeViewStatus)iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 7:
            if (field.Type == TType.I16) {
              PrecisionNumber = iprot.ReadI16();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 8:
            if (field.Type == TType.I32) {
              CreateTimestamp = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 9:
            if (field.Type == TType.I32) {
              LastmodifyTimestamp = iprot.ReadI32();
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
      TStruct struc = new TStruct("HostingComposeView");
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
      if (AliasName != null && __isset.aliasName) {
        field.Name = "aliasName";
        field.Type = TType.String;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(AliasName);
        oprot.WriteFieldEnd();
      }
      if (__isset.viewSource) {
        field.Name = "viewSource";
        field.Type = TType.I32;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32((int)ViewSource);
        oprot.WriteFieldEnd();
      }
      if (__isset.subscribeStatus) {
        field.Name = "subscribeStatus";
        field.Type = TType.I32;
        field.ID = 5;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32((int)SubscribeStatus);
        oprot.WriteFieldEnd();
      }
      if (__isset.viewStatus) {
        field.Name = "viewStatus";
        field.Type = TType.I32;
        field.ID = 6;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32((int)ViewStatus);
        oprot.WriteFieldEnd();
      }
      if (__isset.precisionNumber) {
        field.Name = "precisionNumber";
        field.Type = TType.I16;
        field.ID = 7;
        oprot.WriteFieldBegin(field);
        oprot.WriteI16(PrecisionNumber);
        oprot.WriteFieldEnd();
      }
      if (__isset.createTimestamp) {
        field.Name = "createTimestamp";
        field.Type = TType.I32;
        field.ID = 8;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(CreateTimestamp);
        oprot.WriteFieldEnd();
      }
      if (__isset.lastmodifyTimestamp) {
        field.Name = "lastmodifyTimestamp";
        field.Type = TType.I32;
        field.ID = 9;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(LastmodifyTimestamp);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("HostingComposeView(");
      sb.Append("SubUserId: ");
      sb.Append(SubUserId);
      sb.Append(",ComposeGraphId: ");
      sb.Append(ComposeGraphId);
      sb.Append(",AliasName: ");
      sb.Append(AliasName);
      sb.Append(",ViewSource: ");
      sb.Append(ViewSource);
      sb.Append(",SubscribeStatus: ");
      sb.Append(SubscribeStatus);
      sb.Append(",ViewStatus: ");
      sb.Append(ViewStatus);
      sb.Append(",PrecisionNumber: ");
      sb.Append(PrecisionNumber);
      sb.Append(",CreateTimestamp: ");
      sb.Append(CreateTimestamp);
      sb.Append(",LastmodifyTimestamp: ");
      sb.Append(LastmodifyTimestamp);
      sb.Append(")");
      return sb.ToString();
    }

  }

}