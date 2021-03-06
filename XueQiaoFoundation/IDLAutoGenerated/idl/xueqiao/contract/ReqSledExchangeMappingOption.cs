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
  public partial class ReqSledExchangeMappingOption : TBase, INotifyPropertyChanged
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
    private THashSet<int> _mappingIds;
    private string _sledExchangeMic;
    private xueqiao.contract.standard.TechPlatform _techPlatform;
    private string _techPlatformExchangeCode;
    private IndexedPageOption _pageOption;

    public THashSet<int> MappingIds
    {
      get
      {
        return _mappingIds;
      }
      set
      {
        __isset.mappingIds = true;
        SetProperty(ref _mappingIds, value);
      }
    }

    public string SledExchangeMic
    {
      get
      {
        return _sledExchangeMic;
      }
      set
      {
        __isset.sledExchangeMic = true;
        SetProperty(ref _sledExchangeMic, value);
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

    public string TechPlatformExchangeCode
    {
      get
      {
        return _techPlatformExchangeCode;
      }
      set
      {
        __isset.techPlatformExchangeCode = true;
        SetProperty(ref _techPlatformExchangeCode, value);
      }
    }

    public IndexedPageOption PageOption
    {
      get
      {
        return _pageOption;
      }
      set
      {
        __isset.pageOption = true;
        SetProperty(ref _pageOption, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool mappingIds;
      public bool sledExchangeMic;
      public bool techPlatform;
      public bool techPlatformExchangeCode;
      public bool pageOption;
    }

    public ReqSledExchangeMappingOption() {
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
            if (field.Type == TType.Set) {
              {
                MappingIds = new THashSet<int>();
                TSet _set212 = iprot.ReadSetBegin();
                for( int _i213 = 0; _i213 < _set212.Count; ++_i213)
                {
                  int _elem214 = 0;
                  _elem214 = iprot.ReadI32();
                  MappingIds.Add(_elem214);
                }
                iprot.ReadSetEnd();
              }
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.String) {
              SledExchangeMic = iprot.ReadString();
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
              TechPlatformExchangeCode = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 5:
            if (field.Type == TType.Struct) {
              PageOption = new IndexedPageOption();
              PageOption.Read(iprot);
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
      TStruct struc = new TStruct("ReqSledExchangeMappingOption");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (MappingIds != null && __isset.mappingIds) {
        field.Name = "mappingIds";
        field.Type = TType.Set;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteSetBegin(new TSet(TType.I32, MappingIds.Count));
          foreach (int _iter215 in MappingIds)
          {
            oprot.WriteI32(_iter215);
          }
          oprot.WriteSetEnd();
        }
        oprot.WriteFieldEnd();
      }
      if (SledExchangeMic != null && __isset.sledExchangeMic) {
        field.Name = "sledExchangeMic";
        field.Type = TType.String;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(SledExchangeMic);
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
      if (TechPlatformExchangeCode != null && __isset.techPlatformExchangeCode) {
        field.Name = "techPlatformExchangeCode";
        field.Type = TType.String;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(TechPlatformExchangeCode);
        oprot.WriteFieldEnd();
      }
      if (PageOption != null && __isset.pageOption) {
        field.Name = "pageOption";
        field.Type = TType.Struct;
        field.ID = 5;
        oprot.WriteFieldBegin(field);
        PageOption.Write(oprot);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("ReqSledExchangeMappingOption(");
      sb.Append("MappingIds: ");
      if (MappingIds == null)
      {
        sb.Append("<null>");
      }
      else
      {
        sb.Append("[");
        foreach (int _iter216 in MappingIds)
        {
          sb.Append(_iter216.ToString());
          sb.Append(", ");
        }
        sb.Append("]");
      }
      sb.Append(",SledExchangeMic: ");
      sb.Append(SledExchangeMic);
      sb.Append(",TechPlatform: ");
      sb.Append(TechPlatform);
      sb.Append(",TechPlatformExchangeCode: ");
      sb.Append(TechPlatformExchangeCode);
      sb.Append(",PageOption: ");
      sb.Append(PageOption== null ? "<null>" : PageOption.ToString());
      sb.Append(")");
      return sb.ToString();
    }

  }

}
