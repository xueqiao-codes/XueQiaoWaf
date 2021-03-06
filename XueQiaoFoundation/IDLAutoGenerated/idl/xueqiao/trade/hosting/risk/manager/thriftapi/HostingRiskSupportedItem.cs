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

namespace xueqiao.trade.hosting.risk.manager.thriftapi
{

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class HostingRiskSupportedItem : TBase, INotifyPropertyChanged
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
    private string _itemId;
    private EHostingRiskLevel _riskLevel;
    private string _itemCnName;
    private string _itemDescription;
    private EHostingRiskItemValueType _itemValueType;
    private EHostingRiskLadderType _riskLadderType;
    private EHostingRiskItemValueLevel _itemValueLevel;
    private int _orderNum;

    public string ItemId
    {
      get
      {
        return _itemId;
      }
      set
      {
        __isset.itemId = true;
        SetProperty(ref _itemId, value);
      }
    }

    /// <summary>
    /// 
    /// <seealso cref="EHostingRiskLevel"/>
    /// </summary>
    public EHostingRiskLevel RiskLevel
    {
      get
      {
        return _riskLevel;
      }
      set
      {
        __isset.riskLevel = true;
        SetProperty(ref _riskLevel, value);
      }
    }

    public string ItemCnName
    {
      get
      {
        return _itemCnName;
      }
      set
      {
        __isset.itemCnName = true;
        SetProperty(ref _itemCnName, value);
      }
    }

    public string ItemDescription
    {
      get
      {
        return _itemDescription;
      }
      set
      {
        __isset.itemDescription = true;
        SetProperty(ref _itemDescription, value);
      }
    }

    /// <summary>
    /// 
    /// <seealso cref="EHostingRiskItemValueType"/>
    /// </summary>
    public EHostingRiskItemValueType ItemValueType
    {
      get
      {
        return _itemValueType;
      }
      set
      {
        __isset.itemValueType = true;
        SetProperty(ref _itemValueType, value);
      }
    }

    /// <summary>
    /// 
    /// <seealso cref="EHostingRiskLadderType"/>
    /// </summary>
    public EHostingRiskLadderType RiskLadderType
    {
      get
      {
        return _riskLadderType;
      }
      set
      {
        __isset.riskLadderType = true;
        SetProperty(ref _riskLadderType, value);
      }
    }

    /// <summary>
    /// 
    /// <seealso cref="EHostingRiskItemValueLevel"/>
    /// </summary>
    public EHostingRiskItemValueLevel ItemValueLevel
    {
      get
      {
        return _itemValueLevel;
      }
      set
      {
        __isset.itemValueLevel = true;
        SetProperty(ref _itemValueLevel, value);
      }
    }

    public int OrderNum
    {
      get
      {
        return _orderNum;
      }
      set
      {
        __isset.orderNum = true;
        SetProperty(ref _orderNum, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool itemId;
      public bool riskLevel;
      public bool itemCnName;
      public bool itemDescription;
      public bool itemValueType;
      public bool riskLadderType;
      public bool itemValueLevel;
      public bool orderNum;
    }

    public HostingRiskSupportedItem() {
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
              ItemId = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.I32) {
              RiskLevel = (EHostingRiskLevel)iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.String) {
              ItemCnName = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.String) {
              ItemDescription = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 5:
            if (field.Type == TType.I32) {
              ItemValueType = (EHostingRiskItemValueType)iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 6:
            if (field.Type == TType.I32) {
              RiskLadderType = (EHostingRiskLadderType)iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 7:
            if (field.Type == TType.I32) {
              ItemValueLevel = (EHostingRiskItemValueLevel)iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 8:
            if (field.Type == TType.I32) {
              OrderNum = iprot.ReadI32();
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
      TStruct struc = new TStruct("HostingRiskSupportedItem");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (ItemId != null && __isset.itemId) {
        field.Name = "itemId";
        field.Type = TType.String;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(ItemId);
        oprot.WriteFieldEnd();
      }
      if (__isset.riskLevel) {
        field.Name = "riskLevel";
        field.Type = TType.I32;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32((int)RiskLevel);
        oprot.WriteFieldEnd();
      }
      if (ItemCnName != null && __isset.itemCnName) {
        field.Name = "itemCnName";
        field.Type = TType.String;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(ItemCnName);
        oprot.WriteFieldEnd();
      }
      if (ItemDescription != null && __isset.itemDescription) {
        field.Name = "itemDescription";
        field.Type = TType.String;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(ItemDescription);
        oprot.WriteFieldEnd();
      }
      if (__isset.itemValueType) {
        field.Name = "itemValueType";
        field.Type = TType.I32;
        field.ID = 5;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32((int)ItemValueType);
        oprot.WriteFieldEnd();
      }
      if (__isset.riskLadderType) {
        field.Name = "riskLadderType";
        field.Type = TType.I32;
        field.ID = 6;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32((int)RiskLadderType);
        oprot.WriteFieldEnd();
      }
      if (__isset.itemValueLevel) {
        field.Name = "itemValueLevel";
        field.Type = TType.I32;
        field.ID = 7;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32((int)ItemValueLevel);
        oprot.WriteFieldEnd();
      }
      if (__isset.orderNum) {
        field.Name = "orderNum";
        field.Type = TType.I32;
        field.ID = 8;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(OrderNum);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("HostingRiskSupportedItem(");
      sb.Append("ItemId: ");
      sb.Append(ItemId);
      sb.Append(",RiskLevel: ");
      sb.Append(RiskLevel);
      sb.Append(",ItemCnName: ");
      sb.Append(ItemCnName);
      sb.Append(",ItemDescription: ");
      sb.Append(ItemDescription);
      sb.Append(",ItemValueType: ");
      sb.Append(ItemValueType);
      sb.Append(",RiskLadderType: ");
      sb.Append(RiskLadderType);
      sb.Append(",ItemValueLevel: ");
      sb.Append(ItemValueLevel);
      sb.Append(",OrderNum: ");
      sb.Append(OrderNum);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
