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

namespace xueqiao.contract.standard
{

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class ReqSledContractOption : TBase, INotifyPropertyChanged
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
    private List<int> _sledContractIdList;
    private int _sledCommodityId;
    private TechPlatformEnv _platformEnv;
    private string _sledContractCode;
    private ContractStatus _contractStatus;
    private bool _needTotalCount;
    private string _contractCodePartical;
    private string _sledTagPartical;
    private string _contractEngNamePartical;
    private string _contractCnNamePartical;

    public List<int> SledContractIdList
    {
      get
      {
        return _sledContractIdList;
      }
      set
      {
        __isset.sledContractIdList = true;
        SetProperty(ref _sledContractIdList, value);
      }
    }

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

    /// <summary>
    /// 
    /// <seealso cref="TechPlatformEnv"/>
    /// </summary>
    public TechPlatformEnv PlatformEnv
    {
      get
      {
        return _platformEnv;
      }
      set
      {
        __isset.platformEnv = true;
        SetProperty(ref _platformEnv, value);
      }
    }

    public string SledContractCode
    {
      get
      {
        return _sledContractCode;
      }
      set
      {
        __isset.sledContractCode = true;
        SetProperty(ref _sledContractCode, value);
      }
    }

    /// <summary>
    /// 
    /// <seealso cref="ContractStatus"/>
    /// </summary>
    public ContractStatus ContractStatus
    {
      get
      {
        return _contractStatus;
      }
      set
      {
        __isset.contractStatus = true;
        SetProperty(ref _contractStatus, value);
      }
    }

    public bool NeedTotalCount
    {
      get
      {
        return _needTotalCount;
      }
      set
      {
        __isset.needTotalCount = true;
        SetProperty(ref _needTotalCount, value);
      }
    }

    public string ContractCodePartical
    {
      get
      {
        return _contractCodePartical;
      }
      set
      {
        __isset.contractCodePartical = true;
        SetProperty(ref _contractCodePartical, value);
      }
    }

    public string SledTagPartical
    {
      get
      {
        return _sledTagPartical;
      }
      set
      {
        __isset.sledTagPartical = true;
        SetProperty(ref _sledTagPartical, value);
      }
    }

    public string ContractEngNamePartical
    {
      get
      {
        return _contractEngNamePartical;
      }
      set
      {
        __isset.contractEngNamePartical = true;
        SetProperty(ref _contractEngNamePartical, value);
      }
    }

    public string ContractCnNamePartical
    {
      get
      {
        return _contractCnNamePartical;
      }
      set
      {
        __isset.contractCnNamePartical = true;
        SetProperty(ref _contractCnNamePartical, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool sledContractIdList;
      public bool sledCommodityId;
      public bool platformEnv;
      public bool sledContractCode;
      public bool contractStatus;
      public bool needTotalCount;
      public bool contractCodePartical;
      public bool sledTagPartical;
      public bool contractEngNamePartical;
      public bool contractCnNamePartical;
    }

    public ReqSledContractOption() {
      this._needTotalCount = true;
      this.__isset.needTotalCount = true;
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
            if (field.Type == TType.List) {
              {
                SledContractIdList = new List<int>();
                TList _list25 = iprot.ReadListBegin();
                for( int _i26 = 0; _i26 < _list25.Count; ++_i26)
                {
                  int _elem27 = 0;
                  _elem27 = iprot.ReadI32();
                  SledContractIdList.Add(_elem27);
                }
                iprot.ReadListEnd();
              }
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.I32) {
              SledCommodityId = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.I32) {
              PlatformEnv = (TechPlatformEnv)iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.String) {
              SledContractCode = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 5:
            if (field.Type == TType.I32) {
              ContractStatus = (ContractStatus)iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 6:
            if (field.Type == TType.Bool) {
              NeedTotalCount = iprot.ReadBool();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 10:
            if (field.Type == TType.String) {
              ContractCodePartical = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 11:
            if (field.Type == TType.String) {
              SledTagPartical = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 12:
            if (field.Type == TType.String) {
              ContractEngNamePartical = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 13:
            if (field.Type == TType.String) {
              ContractCnNamePartical = iprot.ReadString();
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
      TStruct struc = new TStruct("ReqSledContractOption");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (SledContractIdList != null && __isset.sledContractIdList) {
        field.Name = "sledContractIdList";
        field.Type = TType.List;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteListBegin(new TList(TType.I32, SledContractIdList.Count));
          foreach (int _iter28 in SledContractIdList)
          {
            oprot.WriteI32(_iter28);
          }
          oprot.WriteListEnd();
        }
        oprot.WriteFieldEnd();
      }
      if (__isset.sledCommodityId) {
        field.Name = "sledCommodityId";
        field.Type = TType.I32;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(SledCommodityId);
        oprot.WriteFieldEnd();
      }
      if (__isset.platformEnv) {
        field.Name = "platformEnv";
        field.Type = TType.I32;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32((int)PlatformEnv);
        oprot.WriteFieldEnd();
      }
      if (SledContractCode != null && __isset.sledContractCode) {
        field.Name = "sledContractCode";
        field.Type = TType.String;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(SledContractCode);
        oprot.WriteFieldEnd();
      }
      if (__isset.contractStatus) {
        field.Name = "contractStatus";
        field.Type = TType.I32;
        field.ID = 5;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32((int)ContractStatus);
        oprot.WriteFieldEnd();
      }
      if (__isset.needTotalCount) {
        field.Name = "needTotalCount";
        field.Type = TType.Bool;
        field.ID = 6;
        oprot.WriteFieldBegin(field);
        oprot.WriteBool(NeedTotalCount);
        oprot.WriteFieldEnd();
      }
      if (ContractCodePartical != null && __isset.contractCodePartical) {
        field.Name = "contractCodePartical";
        field.Type = TType.String;
        field.ID = 10;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(ContractCodePartical);
        oprot.WriteFieldEnd();
      }
      if (SledTagPartical != null && __isset.sledTagPartical) {
        field.Name = "sledTagPartical";
        field.Type = TType.String;
        field.ID = 11;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(SledTagPartical);
        oprot.WriteFieldEnd();
      }
      if (ContractEngNamePartical != null && __isset.contractEngNamePartical) {
        field.Name = "contractEngNamePartical";
        field.Type = TType.String;
        field.ID = 12;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(ContractEngNamePartical);
        oprot.WriteFieldEnd();
      }
      if (ContractCnNamePartical != null && __isset.contractCnNamePartical) {
        field.Name = "contractCnNamePartical";
        field.Type = TType.String;
        field.ID = 13;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(ContractCnNamePartical);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("ReqSledContractOption(");
      sb.Append("SledContractIdList: ");
      if (SledContractIdList == null)
      {
        sb.Append("<null>");
      }
      else
      {
        sb.Append("[");
        foreach (int _iter29 in SledContractIdList)
        {
          sb.Append(_iter29.ToString());
          sb.Append(", ");
        }
        sb.Append("]");
      }
      sb.Append(",SledCommodityId: ");
      sb.Append(SledCommodityId);
      sb.Append(",PlatformEnv: ");
      sb.Append(PlatformEnv);
      sb.Append(",SledContractCode: ");
      sb.Append(SledContractCode);
      sb.Append(",ContractStatus: ");
      sb.Append(ContractStatus);
      sb.Append(",NeedTotalCount: ");
      sb.Append(NeedTotalCount);
      sb.Append(",ContractCodePartical: ");
      sb.Append(ContractCodePartical);
      sb.Append(",SledTagPartical: ");
      sb.Append(SledTagPartical);
      sb.Append(",ContractEngNamePartical: ");
      sb.Append(ContractEngNamePartical);
      sb.Append(",ContractCnNamePartical: ");
      sb.Append(ContractCnNamePartical);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
