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

namespace xueqiao.broker
{

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class BrokerAccessEntry : TBase, INotifyPropertyChanged
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
    private int _entryId;
    private int _brokerId;
    private BrokerPlatform _platform;
    private List<AccessAddress> _tradeAddresses;
    private Dictionary<string, string> _customInfoMap;
    private BrokerAccessStatus _status;
    private long _lastModityTimestamp;
    private long _createTimestamp;
    private BrokerAccessWorkingStatus _workingStatus;
    private TechPlatformEnv _techPlatformEnv;
    private string _accessName;
    private List<AccessAddress> _quotaAddresses;

    public int EntryId
    {
      get
      {
        return _entryId;
      }
      set
      {
        __isset.entryId = true;
        SetProperty(ref _entryId, value);
      }
    }

    public int BrokerId
    {
      get
      {
        return _brokerId;
      }
      set
      {
        __isset.brokerId = true;
        SetProperty(ref _brokerId, value);
      }
    }

    /// <summary>
    /// 
    /// <seealso cref="BrokerPlatform"/>
    /// </summary>
    public BrokerPlatform Platform
    {
      get
      {
        return _platform;
      }
      set
      {
        __isset.platform = true;
        SetProperty(ref _platform, value);
      }
    }

    public List<AccessAddress> TradeAddresses
    {
      get
      {
        return _tradeAddresses;
      }
      set
      {
        __isset.tradeAddresses = true;
        SetProperty(ref _tradeAddresses, value);
      }
    }

    public Dictionary<string, string> CustomInfoMap
    {
      get
      {
        return _customInfoMap;
      }
      set
      {
        __isset.customInfoMap = true;
        SetProperty(ref _customInfoMap, value);
      }
    }

    /// <summary>
    /// 
    /// <seealso cref="BrokerAccessStatus"/>
    /// </summary>
    public BrokerAccessStatus Status
    {
      get
      {
        return _status;
      }
      set
      {
        __isset.status = true;
        SetProperty(ref _status, value);
      }
    }

    public long LastModityTimestamp
    {
      get
      {
        return _lastModityTimestamp;
      }
      set
      {
        __isset.lastModityTimestamp = true;
        SetProperty(ref _lastModityTimestamp, value);
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

    /// <summary>
    /// 
    /// <seealso cref="BrokerAccessWorkingStatus"/>
    /// </summary>
    public BrokerAccessWorkingStatus WorkingStatus
    {
      get
      {
        return _workingStatus;
      }
      set
      {
        __isset.workingStatus = true;
        SetProperty(ref _workingStatus, value);
      }
    }

    /// <summary>
    /// 
    /// <seealso cref="TechPlatformEnv"/>
    /// </summary>
    public TechPlatformEnv TechPlatformEnv
    {
      get
      {
        return _techPlatformEnv;
      }
      set
      {
        __isset.techPlatformEnv = true;
        SetProperty(ref _techPlatformEnv, value);
      }
    }

    public string AccessName
    {
      get
      {
        return _accessName;
      }
      set
      {
        __isset.accessName = true;
        SetProperty(ref _accessName, value);
      }
    }

    public List<AccessAddress> QuotaAddresses
    {
      get
      {
        return _quotaAddresses;
      }
      set
      {
        __isset.quotaAddresses = true;
        SetProperty(ref _quotaAddresses, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool entryId;
      public bool brokerId;
      public bool platform;
      public bool tradeAddresses;
      public bool customInfoMap;
      public bool status;
      public bool lastModityTimestamp;
      public bool createTimestamp;
      public bool workingStatus;
      public bool techPlatformEnv;
      public bool accessName;
      public bool quotaAddresses;
    }

    public BrokerAccessEntry() {
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
              EntryId = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.I32) {
              BrokerId = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.I32) {
              Platform = (BrokerPlatform)iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.List) {
              {
                TradeAddresses = new List<AccessAddress>();
                TList _list10 = iprot.ReadListBegin();
                for( int _i11 = 0; _i11 < _list10.Count; ++_i11)
                {
                  AccessAddress _elem12 = new AccessAddress();
                  _elem12 = new AccessAddress();
                  _elem12.Read(iprot);
                  TradeAddresses.Add(_elem12);
                }
                iprot.ReadListEnd();
              }
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 5:
            if (field.Type == TType.Map) {
              {
                CustomInfoMap = new Dictionary<string, string>();
                TMap _map13 = iprot.ReadMapBegin();
                for( int _i14 = 0; _i14 < _map13.Count; ++_i14)
                {
                  string _key15;
                  string _val16;
                  _key15 = iprot.ReadString();
                  _val16 = iprot.ReadString();
                  CustomInfoMap[_key15] = _val16;
                }
                iprot.ReadMapEnd();
              }
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 6:
            if (field.Type == TType.I32) {
              Status = (BrokerAccessStatus)iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 7:
            if (field.Type == TType.I64) {
              LastModityTimestamp = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 8:
            if (field.Type == TType.I64) {
              CreateTimestamp = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 9:
            if (field.Type == TType.I32) {
              WorkingStatus = (BrokerAccessWorkingStatus)iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 10:
            if (field.Type == TType.I32) {
              TechPlatformEnv = (TechPlatformEnv)iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 11:
            if (field.Type == TType.String) {
              AccessName = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 12:
            if (field.Type == TType.List) {
              {
                QuotaAddresses = new List<AccessAddress>();
                TList _list17 = iprot.ReadListBegin();
                for( int _i18 = 0; _i18 < _list17.Count; ++_i18)
                {
                  AccessAddress _elem19 = new AccessAddress();
                  _elem19 = new AccessAddress();
                  _elem19.Read(iprot);
                  QuotaAddresses.Add(_elem19);
                }
                iprot.ReadListEnd();
              }
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
      TStruct struc = new TStruct("BrokerAccessEntry");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.entryId) {
        field.Name = "entryId";
        field.Type = TType.I32;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(EntryId);
        oprot.WriteFieldEnd();
      }
      if (__isset.brokerId) {
        field.Name = "brokerId";
        field.Type = TType.I32;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(BrokerId);
        oprot.WriteFieldEnd();
      }
      if (__isset.platform) {
        field.Name = "platform";
        field.Type = TType.I32;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32((int)Platform);
        oprot.WriteFieldEnd();
      }
      if (TradeAddresses != null && __isset.tradeAddresses) {
        field.Name = "tradeAddresses";
        field.Type = TType.List;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteListBegin(new TList(TType.Struct, TradeAddresses.Count));
          foreach (AccessAddress _iter20 in TradeAddresses)
          {
            _iter20.Write(oprot);
          }
          oprot.WriteListEnd();
        }
        oprot.WriteFieldEnd();
      }
      if (CustomInfoMap != null && __isset.customInfoMap) {
        field.Name = "customInfoMap";
        field.Type = TType.Map;
        field.ID = 5;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteMapBegin(new TMap(TType.String, TType.String, CustomInfoMap.Count));
          foreach (string _iter21 in CustomInfoMap.Keys)
          {
            oprot.WriteString(_iter21);
            oprot.WriteString(CustomInfoMap[_iter21]);
          }
          oprot.WriteMapEnd();
        }
        oprot.WriteFieldEnd();
      }
      if (__isset.status) {
        field.Name = "status";
        field.Type = TType.I32;
        field.ID = 6;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32((int)Status);
        oprot.WriteFieldEnd();
      }
      if (__isset.lastModityTimestamp) {
        field.Name = "lastModityTimestamp";
        field.Type = TType.I64;
        field.ID = 7;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(LastModityTimestamp);
        oprot.WriteFieldEnd();
      }
      if (__isset.createTimestamp) {
        field.Name = "createTimestamp";
        field.Type = TType.I64;
        field.ID = 8;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(CreateTimestamp);
        oprot.WriteFieldEnd();
      }
      if (__isset.workingStatus) {
        field.Name = "workingStatus";
        field.Type = TType.I32;
        field.ID = 9;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32((int)WorkingStatus);
        oprot.WriteFieldEnd();
      }
      if (__isset.techPlatformEnv) {
        field.Name = "techPlatformEnv";
        field.Type = TType.I32;
        field.ID = 10;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32((int)TechPlatformEnv);
        oprot.WriteFieldEnd();
      }
      if (AccessName != null && __isset.accessName) {
        field.Name = "accessName";
        field.Type = TType.String;
        field.ID = 11;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(AccessName);
        oprot.WriteFieldEnd();
      }
      if (QuotaAddresses != null && __isset.quotaAddresses) {
        field.Name = "quotaAddresses";
        field.Type = TType.List;
        field.ID = 12;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteListBegin(new TList(TType.Struct, QuotaAddresses.Count));
          foreach (AccessAddress _iter22 in QuotaAddresses)
          {
            _iter22.Write(oprot);
          }
          oprot.WriteListEnd();
        }
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("BrokerAccessEntry(");
      sb.Append("EntryId: ");
      sb.Append(EntryId);
      sb.Append(",BrokerId: ");
      sb.Append(BrokerId);
      sb.Append(",Platform: ");
      sb.Append(Platform);
      sb.Append(",TradeAddresses: ");
      if (TradeAddresses == null)
      {
        sb.Append("<null>");
      }
      else
      {
        sb.Append("[");
        foreach (AccessAddress _iter23 in TradeAddresses)
        {
          sb.Append(_iter23.ToString());
          sb.Append(", ");
        }
        sb.Append("]");
      }
      sb.Append(",CustomInfoMap: ");
      if (CustomInfoMap == null)
      {
        sb.Append("<null>");
      }
      else
      {
        sb.Append("{");
        foreach (string _iter24 in CustomInfoMap.Keys)
        {
          sb.Append(_iter24.ToString());
          sb.Append(":");
          sb.Append(CustomInfoMap[_iter24].ToString());
          sb.Append(", ");
        }
        sb.Append("}");
      }
      sb.Append(",Status: ");
      sb.Append(Status);
      sb.Append(",LastModityTimestamp: ");
      sb.Append(LastModityTimestamp);
      sb.Append(",CreateTimestamp: ");
      sb.Append(CreateTimestamp);
      sb.Append(",WorkingStatus: ");
      sb.Append(WorkingStatus);
      sb.Append(",TechPlatformEnv: ");
      sb.Append(TechPlatformEnv);
      sb.Append(",AccessName: ");
      sb.Append(AccessName);
      sb.Append(",QuotaAddresses: ");
      if (QuotaAddresses == null)
      {
        sb.Append("<null>");
      }
      else
      {
        sb.Append("[");
        foreach (AccessAddress _iter25 in QuotaAddresses)
        {
          sb.Append(_iter25.ToString());
          sb.Append(", ");
        }
        sb.Append("]");
      }
      sb.Append(")");
      return sb.ToString();
    }

  }

}
