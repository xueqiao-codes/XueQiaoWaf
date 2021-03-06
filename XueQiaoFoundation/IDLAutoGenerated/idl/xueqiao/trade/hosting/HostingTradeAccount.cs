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
  public partial class HostingTradeAccount : TBase, INotifyPropertyChanged
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
    private long _tradeAccountId;
    private int _tradeBrokerAccessId;
    private string _loginUserName;
    private string _loginPassword;
    private Dictionary<string, string> _accountProperties;
    private int _tradeBrokerId;
    private BrokerTechPlatform _brokerTechPlatform;
    private string _tradeAccountRemark;
    private TradeAccountState _accountState;
    private string _invalidReason;
    private int _invalidErrorCode;
    private int _apiRetCode;
    private TradeAccountAccessState _accountAccessState;
    private bool _hadBeenActived;
    private int _createTimestamp;
    private int _lastmodifyTimestamp;

    public long TradeAccountId
    {
      get
      {
        return _tradeAccountId;
      }
      set
      {
        __isset.tradeAccountId = true;
        SetProperty(ref _tradeAccountId, value);
      }
    }

    public int TradeBrokerAccessId
    {
      get
      {
        return _tradeBrokerAccessId;
      }
      set
      {
        __isset.tradeBrokerAccessId = true;
        SetProperty(ref _tradeBrokerAccessId, value);
      }
    }

    public string LoginUserName
    {
      get
      {
        return _loginUserName;
      }
      set
      {
        __isset.loginUserName = true;
        SetProperty(ref _loginUserName, value);
      }
    }

    public string LoginPassword
    {
      get
      {
        return _loginPassword;
      }
      set
      {
        __isset.loginPassword = true;
        SetProperty(ref _loginPassword, value);
      }
    }

    public Dictionary<string, string> AccountProperties
    {
      get
      {
        return _accountProperties;
      }
      set
      {
        __isset.accountProperties = true;
        SetProperty(ref _accountProperties, value);
      }
    }

    public int TradeBrokerId
    {
      get
      {
        return _tradeBrokerId;
      }
      set
      {
        __isset.tradeBrokerId = true;
        SetProperty(ref _tradeBrokerId, value);
      }
    }

    /// <summary>
    /// 
    /// <seealso cref="BrokerTechPlatform"/>
    /// </summary>
    public BrokerTechPlatform BrokerTechPlatform
    {
      get
      {
        return _brokerTechPlatform;
      }
      set
      {
        __isset.brokerTechPlatform = true;
        SetProperty(ref _brokerTechPlatform, value);
      }
    }

    public string TradeAccountRemark
    {
      get
      {
        return _tradeAccountRemark;
      }
      set
      {
        __isset.tradeAccountRemark = true;
        SetProperty(ref _tradeAccountRemark, value);
      }
    }

    /// <summary>
    /// 
    /// <seealso cref="TradeAccountState"/>
    /// </summary>
    public TradeAccountState AccountState
    {
      get
      {
        return _accountState;
      }
      set
      {
        __isset.accountState = true;
        SetProperty(ref _accountState, value);
      }
    }

    public string InvalidReason
    {
      get
      {
        return _invalidReason;
      }
      set
      {
        __isset.invalidReason = true;
        SetProperty(ref _invalidReason, value);
      }
    }

    public int InvalidErrorCode
    {
      get
      {
        return _invalidErrorCode;
      }
      set
      {
        __isset.invalidErrorCode = true;
        SetProperty(ref _invalidErrorCode, value);
      }
    }

    public int ApiRetCode
    {
      get
      {
        return _apiRetCode;
      }
      set
      {
        __isset.apiRetCode = true;
        SetProperty(ref _apiRetCode, value);
      }
    }

    /// <summary>
    /// 
    /// <seealso cref="TradeAccountAccessState"/>
    /// </summary>
    public TradeAccountAccessState AccountAccessState
    {
      get
      {
        return _accountAccessState;
      }
      set
      {
        __isset.accountAccessState = true;
        SetProperty(ref _accountAccessState, value);
      }
    }

    public bool HadBeenActived
    {
      get
      {
        return _hadBeenActived;
      }
      set
      {
        __isset.hadBeenActived = true;
        SetProperty(ref _hadBeenActived, value);
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
      public bool tradeAccountId;
      public bool tradeBrokerAccessId;
      public bool loginUserName;
      public bool loginPassword;
      public bool accountProperties;
      public bool tradeBrokerId;
      public bool brokerTechPlatform;
      public bool tradeAccountRemark;
      public bool accountState;
      public bool invalidReason;
      public bool invalidErrorCode;
      public bool apiRetCode;
      public bool accountAccessState;
      public bool hadBeenActived;
      public bool createTimestamp;
      public bool lastmodifyTimestamp;
    }

    public HostingTradeAccount() {
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
          case 2:
            if (field.Type == TType.I64) {
              TradeAccountId = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.I32) {
              TradeBrokerAccessId = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.String) {
              LoginUserName = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 5:
            if (field.Type == TType.String) {
              LoginPassword = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 6:
            if (field.Type == TType.Map) {
              {
                AccountProperties = new Dictionary<string, string>();
                TMap _map11 = iprot.ReadMapBegin();
                for( int _i12 = 0; _i12 < _map11.Count; ++_i12)
                {
                  string _key13;
                  string _val14;
                  _key13 = iprot.ReadString();
                  _val14 = iprot.ReadString();
                  AccountProperties[_key13] = _val14;
                }
                iprot.ReadMapEnd();
              }
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 7:
            if (field.Type == TType.I32) {
              TradeBrokerId = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 8:
            if (field.Type == TType.I32) {
              BrokerTechPlatform = (BrokerTechPlatform)iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 9:
            if (field.Type == TType.String) {
              TradeAccountRemark = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 11:
            if (field.Type == TType.I32) {
              AccountState = (TradeAccountState)iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 12:
            if (field.Type == TType.String) {
              InvalidReason = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 13:
            if (field.Type == TType.I32) {
              InvalidErrorCode = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 14:
            if (field.Type == TType.I32) {
              ApiRetCode = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 15:
            if (field.Type == TType.I32) {
              AccountAccessState = (TradeAccountAccessState)iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 16:
            if (field.Type == TType.Bool) {
              HadBeenActived = iprot.ReadBool();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 21:
            if (field.Type == TType.I32) {
              CreateTimestamp = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 22:
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
      TStruct struc = new TStruct("HostingTradeAccount");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.tradeAccountId) {
        field.Name = "tradeAccountId";
        field.Type = TType.I64;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(TradeAccountId);
        oprot.WriteFieldEnd();
      }
      if (__isset.tradeBrokerAccessId) {
        field.Name = "tradeBrokerAccessId";
        field.Type = TType.I32;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(TradeBrokerAccessId);
        oprot.WriteFieldEnd();
      }
      if (LoginUserName != null && __isset.loginUserName) {
        field.Name = "loginUserName";
        field.Type = TType.String;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(LoginUserName);
        oprot.WriteFieldEnd();
      }
      if (LoginPassword != null && __isset.loginPassword) {
        field.Name = "loginPassword";
        field.Type = TType.String;
        field.ID = 5;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(LoginPassword);
        oprot.WriteFieldEnd();
      }
      if (AccountProperties != null && __isset.accountProperties) {
        field.Name = "accountProperties";
        field.Type = TType.Map;
        field.ID = 6;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteMapBegin(new TMap(TType.String, TType.String, AccountProperties.Count));
          foreach (string _iter15 in AccountProperties.Keys)
          {
            oprot.WriteString(_iter15);
            oprot.WriteString(AccountProperties[_iter15]);
          }
          oprot.WriteMapEnd();
        }
        oprot.WriteFieldEnd();
      }
      if (__isset.tradeBrokerId) {
        field.Name = "tradeBrokerId";
        field.Type = TType.I32;
        field.ID = 7;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(TradeBrokerId);
        oprot.WriteFieldEnd();
      }
      if (__isset.brokerTechPlatform) {
        field.Name = "brokerTechPlatform";
        field.Type = TType.I32;
        field.ID = 8;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32((int)BrokerTechPlatform);
        oprot.WriteFieldEnd();
      }
      if (TradeAccountRemark != null && __isset.tradeAccountRemark) {
        field.Name = "tradeAccountRemark";
        field.Type = TType.String;
        field.ID = 9;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(TradeAccountRemark);
        oprot.WriteFieldEnd();
      }
      if (__isset.accountState) {
        field.Name = "accountState";
        field.Type = TType.I32;
        field.ID = 11;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32((int)AccountState);
        oprot.WriteFieldEnd();
      }
      if (InvalidReason != null && __isset.invalidReason) {
        field.Name = "invalidReason";
        field.Type = TType.String;
        field.ID = 12;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(InvalidReason);
        oprot.WriteFieldEnd();
      }
      if (__isset.invalidErrorCode) {
        field.Name = "invalidErrorCode";
        field.Type = TType.I32;
        field.ID = 13;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(InvalidErrorCode);
        oprot.WriteFieldEnd();
      }
      if (__isset.apiRetCode) {
        field.Name = "apiRetCode";
        field.Type = TType.I32;
        field.ID = 14;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(ApiRetCode);
        oprot.WriteFieldEnd();
      }
      if (__isset.accountAccessState) {
        field.Name = "accountAccessState";
        field.Type = TType.I32;
        field.ID = 15;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32((int)AccountAccessState);
        oprot.WriteFieldEnd();
      }
      if (__isset.hadBeenActived) {
        field.Name = "hadBeenActived";
        field.Type = TType.Bool;
        field.ID = 16;
        oprot.WriteFieldBegin(field);
        oprot.WriteBool(HadBeenActived);
        oprot.WriteFieldEnd();
      }
      if (__isset.createTimestamp) {
        field.Name = "createTimestamp";
        field.Type = TType.I32;
        field.ID = 21;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(CreateTimestamp);
        oprot.WriteFieldEnd();
      }
      if (__isset.lastmodifyTimestamp) {
        field.Name = "lastmodifyTimestamp";
        field.Type = TType.I32;
        field.ID = 22;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(LastmodifyTimestamp);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("HostingTradeAccount(");
      sb.Append("TradeAccountId: ");
      sb.Append(TradeAccountId);
      sb.Append(",TradeBrokerAccessId: ");
      sb.Append(TradeBrokerAccessId);
      sb.Append(",LoginUserName: ");
      sb.Append(LoginUserName);
      sb.Append(",LoginPassword: ");
      sb.Append(LoginPassword);
      sb.Append(",AccountProperties: ");
      if (AccountProperties == null)
      {
        sb.Append("<null>");
      }
      else
      {
        sb.Append("{");
        foreach (string _iter16 in AccountProperties.Keys)
        {
          sb.Append(_iter16.ToString());
          sb.Append(":");
          sb.Append(AccountProperties[_iter16].ToString());
          sb.Append(", ");
        }
        sb.Append("}");
      }
      sb.Append(",TradeBrokerId: ");
      sb.Append(TradeBrokerId);
      sb.Append(",BrokerTechPlatform: ");
      sb.Append(BrokerTechPlatform);
      sb.Append(",TradeAccountRemark: ");
      sb.Append(TradeAccountRemark);
      sb.Append(",AccountState: ");
      sb.Append(AccountState);
      sb.Append(",InvalidReason: ");
      sb.Append(InvalidReason);
      sb.Append(",InvalidErrorCode: ");
      sb.Append(InvalidErrorCode);
      sb.Append(",ApiRetCode: ");
      sb.Append(ApiRetCode);
      sb.Append(",AccountAccessState: ");
      sb.Append(AccountAccessState);
      sb.Append(",HadBeenActived: ");
      sb.Append(HadBeenActived);
      sb.Append(",CreateTimestamp: ");
      sb.Append(CreateTimestamp);
      sb.Append(",LastmodifyTimestamp: ");
      sb.Append(LastmodifyTimestamp);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
