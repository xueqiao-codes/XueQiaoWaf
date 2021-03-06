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
  public partial class TechPlatformCommodity : TBase, INotifyPropertyChanged
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
    private int _sledCommodityId;
    private string _exchange;
    private string _commodityType;
    private string _commodityCode;
    private List<string> _relateCommodityCodes;
    private string _tradeCurrency;
    private string _timezone;
    private double _contractSize;
    private double _tickSize;
    private int _denominator;
    private xueqiao.contract.standard.CmbDirect _cmbDirect;
    private xueqiao.contract.standard.CommodityState _commodityState;
    private string _engName;
    private string _cnName;
    private string _tcName;
    private xueqiao.contract.standard.DeliveryMode _deliveryMode;
    private int _deliveryDays;
    private int _maxSingleOrderVol;
    private int _maxHoldVol;
    private xueqiao.contract.standard.CalculateMode _commissionCalculateMode;
    private double _openCloseFee;
    private xueqiao.contract.standard.CalculateMode _marginCalculateMode;
    private double _initialMargin;
    private double _maintenanceMargin;
    private double _sellInitialMargin;
    private double _sellMaintenanceMargin;
    private double _lockMargin;
    private xueqiao.contract.standard.TechPlatform _techPlatform;
    private long _createTimestamp;
    private long _lastModityTimestamp;

    public int SledCommodityId
    {
      get
      {
        return _sledCommodityId;
      }
      set
      {
        __isset.sledCommodityId = true;
        SetProperty(ref _sledCommodityId, value);
      }
    }

    public string Exchange
    {
      get
      {
        return _exchange;
      }
      set
      {
        __isset.exchange = true;
        SetProperty(ref _exchange, value);
      }
    }

    public string CommodityType
    {
      get
      {
        return _commodityType;
      }
      set
      {
        __isset.commodityType = true;
        SetProperty(ref _commodityType, value);
      }
    }

    public string CommodityCode
    {
      get
      {
        return _commodityCode;
      }
      set
      {
        __isset.commodityCode = true;
        SetProperty(ref _commodityCode, value);
      }
    }

    public List<string> RelateCommodityCodes
    {
      get
      {
        return _relateCommodityCodes;
      }
      set
      {
        __isset.relateCommodityCodes = true;
        SetProperty(ref _relateCommodityCodes, value);
      }
    }

    public string TradeCurrency
    {
      get
      {
        return _tradeCurrency;
      }
      set
      {
        __isset.tradeCurrency = true;
        SetProperty(ref _tradeCurrency, value);
      }
    }

    public string Timezone
    {
      get
      {
        return _timezone;
      }
      set
      {
        __isset.timezone = true;
        SetProperty(ref _timezone, value);
      }
    }

    public double ContractSize
    {
      get
      {
        return _contractSize;
      }
      set
      {
        __isset.contractSize = true;
        SetProperty(ref _contractSize, value);
      }
    }

    public double TickSize
    {
      get
      {
        return _tickSize;
      }
      set
      {
        __isset.tickSize = true;
        SetProperty(ref _tickSize, value);
      }
    }

    public int Denominator
    {
      get
      {
        return _denominator;
      }
      set
      {
        __isset.denominator = true;
        SetProperty(ref _denominator, value);
      }
    }

    /// <summary>
    /// 
    /// <seealso cref="xueqiao.contract.standard.CmbDirect"/>
    /// </summary>
    public xueqiao.contract.standard.CmbDirect CmbDirect
    {
      get
      {
        return _cmbDirect;
      }
      set
      {
        __isset.cmbDirect = true;
        SetProperty(ref _cmbDirect, value);
      }
    }

    /// <summary>
    /// 
    /// <seealso cref="xueqiao.contract.standard.CommodityState"/>
    /// </summary>
    public xueqiao.contract.standard.CommodityState CommodityState
    {
      get
      {
        return _commodityState;
      }
      set
      {
        __isset.commodityState = true;
        SetProperty(ref _commodityState, value);
      }
    }

    public string EngName
    {
      get
      {
        return _engName;
      }
      set
      {
        __isset.engName = true;
        SetProperty(ref _engName, value);
      }
    }

    public string CnName
    {
      get
      {
        return _cnName;
      }
      set
      {
        __isset.cnName = true;
        SetProperty(ref _cnName, value);
      }
    }

    public string TcName
    {
      get
      {
        return _tcName;
      }
      set
      {
        __isset.tcName = true;
        SetProperty(ref _tcName, value);
      }
    }

    /// <summary>
    /// 
    /// <seealso cref="xueqiao.contract.standard.DeliveryMode"/>
    /// </summary>
    public xueqiao.contract.standard.DeliveryMode DeliveryMode
    {
      get
      {
        return _deliveryMode;
      }
      set
      {
        __isset.deliveryMode = true;
        SetProperty(ref _deliveryMode, value);
      }
    }

    public int DeliveryDays
    {
      get
      {
        return _deliveryDays;
      }
      set
      {
        __isset.deliveryDays = true;
        SetProperty(ref _deliveryDays, value);
      }
    }

    public int MaxSingleOrderVol
    {
      get
      {
        return _maxSingleOrderVol;
      }
      set
      {
        __isset.maxSingleOrderVol = true;
        SetProperty(ref _maxSingleOrderVol, value);
      }
    }

    public int MaxHoldVol
    {
      get
      {
        return _maxHoldVol;
      }
      set
      {
        __isset.maxHoldVol = true;
        SetProperty(ref _maxHoldVol, value);
      }
    }

    /// <summary>
    /// 
    /// <seealso cref="xueqiao.contract.standard.CalculateMode"/>
    /// </summary>
    public xueqiao.contract.standard.CalculateMode CommissionCalculateMode
    {
      get
      {
        return _commissionCalculateMode;
      }
      set
      {
        __isset.commissionCalculateMode = true;
        SetProperty(ref _commissionCalculateMode, value);
      }
    }

    public double OpenCloseFee
    {
      get
      {
        return _openCloseFee;
      }
      set
      {
        __isset.openCloseFee = true;
        SetProperty(ref _openCloseFee, value);
      }
    }

    /// <summary>
    /// 
    /// <seealso cref="xueqiao.contract.standard.CalculateMode"/>
    /// </summary>
    public xueqiao.contract.standard.CalculateMode MarginCalculateMode
    {
      get
      {
        return _marginCalculateMode;
      }
      set
      {
        __isset.marginCalculateMode = true;
        SetProperty(ref _marginCalculateMode, value);
      }
    }

    public double InitialMargin
    {
      get
      {
        return _initialMargin;
      }
      set
      {
        __isset.initialMargin = true;
        SetProperty(ref _initialMargin, value);
      }
    }

    public double MaintenanceMargin
    {
      get
      {
        return _maintenanceMargin;
      }
      set
      {
        __isset.maintenanceMargin = true;
        SetProperty(ref _maintenanceMargin, value);
      }
    }

    public double SellInitialMargin
    {
      get
      {
        return _sellInitialMargin;
      }
      set
      {
        __isset.sellInitialMargin = true;
        SetProperty(ref _sellInitialMargin, value);
      }
    }

    public double SellMaintenanceMargin
    {
      get
      {
        return _sellMaintenanceMargin;
      }
      set
      {
        __isset.sellMaintenanceMargin = true;
        SetProperty(ref _sellMaintenanceMargin, value);
      }
    }

    public double LockMargin
    {
      get
      {
        return _lockMargin;
      }
      set
      {
        __isset.lockMargin = true;
        SetProperty(ref _lockMargin, value);
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


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool sledCommodityId;
      public bool exchange;
      public bool commodityType;
      public bool commodityCode;
      public bool relateCommodityCodes;
      public bool tradeCurrency;
      public bool timezone;
      public bool contractSize;
      public bool tickSize;
      public bool denominator;
      public bool cmbDirect;
      public bool commodityState;
      public bool engName;
      public bool cnName;
      public bool tcName;
      public bool deliveryMode;
      public bool deliveryDays;
      public bool maxSingleOrderVol;
      public bool maxHoldVol;
      public bool commissionCalculateMode;
      public bool openCloseFee;
      public bool marginCalculateMode;
      public bool initialMargin;
      public bool maintenanceMargin;
      public bool sellInitialMargin;
      public bool sellMaintenanceMargin;
      public bool lockMargin;
      public bool techPlatform;
      public bool createTimestamp;
      public bool lastModityTimestamp;
    }

    public TechPlatformCommodity() {
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
              SledCommodityId = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.String) {
              Exchange = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.String) {
              CommodityType = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.String) {
              CommodityCode = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 5:
            if (field.Type == TType.List) {
              {
                RelateCommodityCodes = new List<string>();
                TList _list40 = iprot.ReadListBegin();
                for( int _i41 = 0; _i41 < _list40.Count; ++_i41)
                {
                  string _elem42 = null;
                  _elem42 = iprot.ReadString();
                  RelateCommodityCodes.Add(_elem42);
                }
                iprot.ReadListEnd();
              }
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 6:
            if (field.Type == TType.String) {
              TradeCurrency = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 7:
            if (field.Type == TType.String) {
              Timezone = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 8:
            if (field.Type == TType.Double) {
              ContractSize = iprot.ReadDouble();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 9:
            if (field.Type == TType.Double) {
              TickSize = iprot.ReadDouble();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 10:
            if (field.Type == TType.I32) {
              Denominator = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 11:
            if (field.Type == TType.I32) {
              CmbDirect = (xueqiao.contract.standard.CmbDirect)iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 15:
            if (field.Type == TType.I32) {
              CommodityState = (xueqiao.contract.standard.CommodityState)iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 22:
            if (field.Type == TType.String) {
              EngName = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 23:
            if (field.Type == TType.String) {
              CnName = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 24:
            if (field.Type == TType.String) {
              TcName = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 12:
            if (field.Type == TType.I32) {
              DeliveryMode = (xueqiao.contract.standard.DeliveryMode)iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 13:
            if (field.Type == TType.I32) {
              DeliveryDays = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 17:
            if (field.Type == TType.I32) {
              MaxSingleOrderVol = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 18:
            if (field.Type == TType.I32) {
              MaxHoldVol = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 20:
            if (field.Type == TType.I32) {
              CommissionCalculateMode = (xueqiao.contract.standard.CalculateMode)iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 21:
            if (field.Type == TType.Double) {
              OpenCloseFee = iprot.ReadDouble();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 26:
            if (field.Type == TType.I32) {
              MarginCalculateMode = (xueqiao.contract.standard.CalculateMode)iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 27:
            if (field.Type == TType.Double) {
              InitialMargin = iprot.ReadDouble();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 28:
            if (field.Type == TType.Double) {
              MaintenanceMargin = iprot.ReadDouble();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 29:
            if (field.Type == TType.Double) {
              SellInitialMargin = iprot.ReadDouble();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 30:
            if (field.Type == TType.Double) {
              SellMaintenanceMargin = iprot.ReadDouble();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 31:
            if (field.Type == TType.Double) {
              LockMargin = iprot.ReadDouble();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 35:
            if (field.Type == TType.I32) {
              TechPlatform = (xueqiao.contract.standard.TechPlatform)iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 40:
            if (field.Type == TType.I64) {
              CreateTimestamp = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 41:
            if (field.Type == TType.I64) {
              LastModityTimestamp = iprot.ReadI64();
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
      TStruct struc = new TStruct("TechPlatformCommodity");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.sledCommodityId) {
        field.Name = "sledCommodityId";
        field.Type = TType.I32;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(SledCommodityId);
        oprot.WriteFieldEnd();
      }
      if (Exchange != null && __isset.exchange) {
        field.Name = "exchange";
        field.Type = TType.String;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Exchange);
        oprot.WriteFieldEnd();
      }
      if (CommodityType != null && __isset.commodityType) {
        field.Name = "commodityType";
        field.Type = TType.String;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(CommodityType);
        oprot.WriteFieldEnd();
      }
      if (CommodityCode != null && __isset.commodityCode) {
        field.Name = "commodityCode";
        field.Type = TType.String;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(CommodityCode);
        oprot.WriteFieldEnd();
      }
      if (RelateCommodityCodes != null && __isset.relateCommodityCodes) {
        field.Name = "relateCommodityCodes";
        field.Type = TType.List;
        field.ID = 5;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteListBegin(new TList(TType.String, RelateCommodityCodes.Count));
          foreach (string _iter43 in RelateCommodityCodes)
          {
            oprot.WriteString(_iter43);
          }
          oprot.WriteListEnd();
        }
        oprot.WriteFieldEnd();
      }
      if (TradeCurrency != null && __isset.tradeCurrency) {
        field.Name = "tradeCurrency";
        field.Type = TType.String;
        field.ID = 6;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(TradeCurrency);
        oprot.WriteFieldEnd();
      }
      if (Timezone != null && __isset.timezone) {
        field.Name = "timezone";
        field.Type = TType.String;
        field.ID = 7;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Timezone);
        oprot.WriteFieldEnd();
      }
      if (__isset.contractSize) {
        field.Name = "contractSize";
        field.Type = TType.Double;
        field.ID = 8;
        oprot.WriteFieldBegin(field);
        oprot.WriteDouble(ContractSize);
        oprot.WriteFieldEnd();
      }
      if (__isset.tickSize) {
        field.Name = "tickSize";
        field.Type = TType.Double;
        field.ID = 9;
        oprot.WriteFieldBegin(field);
        oprot.WriteDouble(TickSize);
        oprot.WriteFieldEnd();
      }
      if (__isset.denominator) {
        field.Name = "denominator";
        field.Type = TType.I32;
        field.ID = 10;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(Denominator);
        oprot.WriteFieldEnd();
      }
      if (__isset.cmbDirect) {
        field.Name = "cmbDirect";
        field.Type = TType.I32;
        field.ID = 11;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32((int)CmbDirect);
        oprot.WriteFieldEnd();
      }
      if (__isset.deliveryMode) {
        field.Name = "deliveryMode";
        field.Type = TType.I32;
        field.ID = 12;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32((int)DeliveryMode);
        oprot.WriteFieldEnd();
      }
      if (__isset.deliveryDays) {
        field.Name = "deliveryDays";
        field.Type = TType.I32;
        field.ID = 13;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(DeliveryDays);
        oprot.WriteFieldEnd();
      }
      if (__isset.commodityState) {
        field.Name = "commodityState";
        field.Type = TType.I32;
        field.ID = 15;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32((int)CommodityState);
        oprot.WriteFieldEnd();
      }
      if (__isset.maxSingleOrderVol) {
        field.Name = "maxSingleOrderVol";
        field.Type = TType.I32;
        field.ID = 17;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(MaxSingleOrderVol);
        oprot.WriteFieldEnd();
      }
      if (__isset.maxHoldVol) {
        field.Name = "maxHoldVol";
        field.Type = TType.I32;
        field.ID = 18;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(MaxHoldVol);
        oprot.WriteFieldEnd();
      }
      if (__isset.commissionCalculateMode) {
        field.Name = "commissionCalculateMode";
        field.Type = TType.I32;
        field.ID = 20;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32((int)CommissionCalculateMode);
        oprot.WriteFieldEnd();
      }
      if (__isset.openCloseFee) {
        field.Name = "openCloseFee";
        field.Type = TType.Double;
        field.ID = 21;
        oprot.WriteFieldBegin(field);
        oprot.WriteDouble(OpenCloseFee);
        oprot.WriteFieldEnd();
      }
      if (EngName != null && __isset.engName) {
        field.Name = "engName";
        field.Type = TType.String;
        field.ID = 22;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(EngName);
        oprot.WriteFieldEnd();
      }
      if (CnName != null && __isset.cnName) {
        field.Name = "cnName";
        field.Type = TType.String;
        field.ID = 23;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(CnName);
        oprot.WriteFieldEnd();
      }
      if (TcName != null && __isset.tcName) {
        field.Name = "tcName";
        field.Type = TType.String;
        field.ID = 24;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(TcName);
        oprot.WriteFieldEnd();
      }
      if (__isset.marginCalculateMode) {
        field.Name = "marginCalculateMode";
        field.Type = TType.I32;
        field.ID = 26;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32((int)MarginCalculateMode);
        oprot.WriteFieldEnd();
      }
      if (__isset.initialMargin) {
        field.Name = "initialMargin";
        field.Type = TType.Double;
        field.ID = 27;
        oprot.WriteFieldBegin(field);
        oprot.WriteDouble(InitialMargin);
        oprot.WriteFieldEnd();
      }
      if (__isset.maintenanceMargin) {
        field.Name = "maintenanceMargin";
        field.Type = TType.Double;
        field.ID = 28;
        oprot.WriteFieldBegin(field);
        oprot.WriteDouble(MaintenanceMargin);
        oprot.WriteFieldEnd();
      }
      if (__isset.sellInitialMargin) {
        field.Name = "sellInitialMargin";
        field.Type = TType.Double;
        field.ID = 29;
        oprot.WriteFieldBegin(field);
        oprot.WriteDouble(SellInitialMargin);
        oprot.WriteFieldEnd();
      }
      if (__isset.sellMaintenanceMargin) {
        field.Name = "sellMaintenanceMargin";
        field.Type = TType.Double;
        field.ID = 30;
        oprot.WriteFieldBegin(field);
        oprot.WriteDouble(SellMaintenanceMargin);
        oprot.WriteFieldEnd();
      }
      if (__isset.lockMargin) {
        field.Name = "lockMargin";
        field.Type = TType.Double;
        field.ID = 31;
        oprot.WriteFieldBegin(field);
        oprot.WriteDouble(LockMargin);
        oprot.WriteFieldEnd();
      }
      if (__isset.techPlatform) {
        field.Name = "techPlatform";
        field.Type = TType.I32;
        field.ID = 35;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32((int)TechPlatform);
        oprot.WriteFieldEnd();
      }
      if (__isset.createTimestamp) {
        field.Name = "createTimestamp";
        field.Type = TType.I64;
        field.ID = 40;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(CreateTimestamp);
        oprot.WriteFieldEnd();
      }
      if (__isset.lastModityTimestamp) {
        field.Name = "lastModityTimestamp";
        field.Type = TType.I64;
        field.ID = 41;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(LastModityTimestamp);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("TechPlatformCommodity(");
      sb.Append("SledCommodityId: ");
      sb.Append(SledCommodityId);
      sb.Append(",Exchange: ");
      sb.Append(Exchange);
      sb.Append(",CommodityType: ");
      sb.Append(CommodityType);
      sb.Append(",CommodityCode: ");
      sb.Append(CommodityCode);
      sb.Append(",RelateCommodityCodes: ");
      if (RelateCommodityCodes == null)
      {
        sb.Append("<null>");
      }
      else
      {
        sb.Append("[");
        foreach (string _iter44 in RelateCommodityCodes)
        {
          sb.Append(_iter44.ToString());
          sb.Append(", ");
        }
        sb.Append("]");
      }
      sb.Append(",TradeCurrency: ");
      sb.Append(TradeCurrency);
      sb.Append(",Timezone: ");
      sb.Append(Timezone);
      sb.Append(",ContractSize: ");
      sb.Append(ContractSize);
      sb.Append(",TickSize: ");
      sb.Append(TickSize);
      sb.Append(",Denominator: ");
      sb.Append(Denominator);
      sb.Append(",CmbDirect: ");
      sb.Append(CmbDirect);
      sb.Append(",CommodityState: ");
      sb.Append(CommodityState);
      sb.Append(",EngName: ");
      sb.Append(EngName);
      sb.Append(",CnName: ");
      sb.Append(CnName);
      sb.Append(",TcName: ");
      sb.Append(TcName);
      sb.Append(",DeliveryMode: ");
      sb.Append(DeliveryMode);
      sb.Append(",DeliveryDays: ");
      sb.Append(DeliveryDays);
      sb.Append(",MaxSingleOrderVol: ");
      sb.Append(MaxSingleOrderVol);
      sb.Append(",MaxHoldVol: ");
      sb.Append(MaxHoldVol);
      sb.Append(",CommissionCalculateMode: ");
      sb.Append(CommissionCalculateMode);
      sb.Append(",OpenCloseFee: ");
      sb.Append(OpenCloseFee);
      sb.Append(",MarginCalculateMode: ");
      sb.Append(MarginCalculateMode);
      sb.Append(",InitialMargin: ");
      sb.Append(InitialMargin);
      sb.Append(",MaintenanceMargin: ");
      sb.Append(MaintenanceMargin);
      sb.Append(",SellInitialMargin: ");
      sb.Append(SellInitialMargin);
      sb.Append(",SellMaintenanceMargin: ");
      sb.Append(SellMaintenanceMargin);
      sb.Append(",LockMargin: ");
      sb.Append(LockMargin);
      sb.Append(",TechPlatform: ");
      sb.Append(TechPlatform);
      sb.Append(",CreateTimestamp: ");
      sb.Append(CreateTimestamp);
      sb.Append(",LastModityTimestamp: ");
      sb.Append(LastModityTimestamp);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
