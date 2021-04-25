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
  public partial class ReqTechPlatformCommodityOption : TBase, INotifyPropertyChanged
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
    private List<int> _techPlatformCommodityIds;
    private List<int> _sledCommodityIds;
    private xueqiao.contract.standard.TechPlatform _techPlatform;
    private string _techPlatformExchange;
    private string _techPlatformCommodityType;
    private string _techPlatformCommodityCode;

    public List<int> TechPlatformCommodityIds
    {
      get
      {
        return _techPlatformCommodityIds;
      }
      set
      {
        __isset.techPlatformCommodityIds = true;
        SetProperty(ref _techPlatformCommodityIds, value);
      }
    }

    public List<int> SledCommodityIds
    {
      get
      {
        return _sledCommodityIds;
      }
      set
      {
        __isset.sledCommodityIds = true;
        SetProperty(ref _sledCommodityIds, value);
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

    public string TechPlatformExchange
    {
      get
      {
        return _techPlatformExchange;
      }
      set
      {
        __isset.techPlatformExchange = true;
        SetProperty(ref _techPlatformExchange, value);
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

    public string TechPlatformCommodityCode
    {
      get
      {
        return _techPlatformCommodityCode;
      }
      set
      {
        __isset.techPlatformCommodityCode = true;
        SetProperty(ref _techPlatformCommodityCode, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool techPlatformCommodityIds;
      public bool sledCommodityIds;
      public bool techPlatform;
      public bool techPlatformExchange;
      public bool techPlatformCommodityType;
      public bool techPlatformCommodityCode;
    }

    public ReqTechPlatformCommodityOption() {
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
                TechPlatformCommodityIds = new List<int>();
                TList _list50 = iprot.ReadListBegin();
                for( int _i51 = 0; _i51 < _list50.Count; ++_i51)
                {
                  int _elem52 = 0;
                  _elem52 = iprot.ReadI32();
                  TechPlatformCommodityIds.Add(_elem52);
                }
                iprot.ReadListEnd();
              }
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.List) {
              {
                SledCommodityIds = new List<int>();
                TList _list53 = iprot.ReadListBegin();
                for( int _i54 = 0; _i54 < _list53.Count; ++_i54)
                {
                  int _elem55 = 0;
                  _elem55 = iprot.ReadI32();
                  SledCommodityIds.Add(_elem55);
                }
                iprot.ReadListEnd();
              }
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
              TechPlatformExchange = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 5:
            if (field.Type == TType.String) {
              TechPlatformCommodityType = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 6:
            if (field.Type == TType.String) {
              TechPlatformCommodityCode = iprot.ReadString();
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
      TStruct struc = new TStruct("ReqTechPlatformCommodityOption");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (TechPlatformCommodityIds != null && __isset.techPlatformCommodityIds) {
        field.Name = "techPlatformCommodityIds";
        field.Type = TType.List;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteListBegin(new TList(TType.I32, TechPlatformCommodityIds.Count));
          foreach (int _iter56 in TechPlatformCommodityIds)
          {
            oprot.WriteI32(_iter56);
          }
          oprot.WriteListEnd();
        }
        oprot.WriteFieldEnd();
      }
      if (SledCommodityIds != null && __isset.sledCommodityIds) {
        field.Name = "sledCommodityIds";
        field.Type = TType.List;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteListBegin(new TList(TType.I32, SledCommodityIds.Count));
          foreach (int _iter57 in SledCommodityIds)
          {
            oprot.WriteI32(_iter57);
          }
          oprot.WriteListEnd();
        }
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
      if (TechPlatformExchange != null && __isset.techPlatformExchange) {
        field.Name = "techPlatformExchange";
        field.Type = TType.String;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(TechPlatformExchange);
        oprot.WriteFieldEnd();
      }
      if (TechPlatformCommodityType != null && __isset.techPlatformCommodityType) {
        field.Name = "techPlatformCommodityType";
        field.Type = TType.String;
        field.ID = 5;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(TechPlatformCommodityType);
        oprot.WriteFieldEnd();
      }
      if (TechPlatformCommodityCode != null && __isset.techPlatformCommodityCode) {
        field.Name = "techPlatformCommodityCode";
        field.Type = TType.String;
        field.ID = 6;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(TechPlatformCommodityCode);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("ReqTechPlatformCommodityOption(");
      sb.Append("TechPlatformCommodityIds: ");
      if (TechPlatformCommodityIds == null)
      {
        sb.Append("<null>");
      }
      else
      {
        sb.Append("[");
        foreach (int _iter58 in TechPlatformCommodityIds)
        {
          sb.Append(_iter58.ToString());
          sb.Append(", ");
        }
        sb.Append("]");
      }
      sb.Append(",SledCommodityIds: ");
      if (SledCommodityIds == null)
      {
        sb.Append("<null>");
      }
      else
      {
        sb.Append("[");
        foreach (int _iter59 in SledCommodityIds)
        {
          sb.Append(_iter59.ToString());
          sb.Append(", ");
        }
        sb.Append("]");
      }
      sb.Append(",TechPlatform: ");
      sb.Append(TechPlatform);
      sb.Append(",TechPlatformExchange: ");
      sb.Append(TechPlatformExchange);
      sb.Append(",TechPlatformCommodityType: ");
      sb.Append(TechPlatformCommodityType);
      sb.Append(",TechPlatformCommodityCode: ");
      sb.Append(TechPlatformCommodityCode);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
