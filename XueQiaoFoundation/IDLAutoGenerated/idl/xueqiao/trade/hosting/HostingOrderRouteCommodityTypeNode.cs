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
  public partial class HostingOrderRouteCommodityTypeNode : TBase, INotifyPropertyChanged
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
    private short _sledCommodityType;
    private Dictionary<string, HostingOrderRouteCommodityCodeNode> _subCommodityCodeNodes;
    private HostingOrderRouteRelatedInfo _relatedInfo;

    public short SledCommodityType
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

    public Dictionary<string, HostingOrderRouteCommodityCodeNode> SubCommodityCodeNodes
    {
      get
      {
        return _subCommodityCodeNodes;
      }
      set
      {
        __isset.subCommodityCodeNodes = true;
        SetProperty(ref _subCommodityCodeNodes, value);
      }
    }

    public HostingOrderRouteRelatedInfo RelatedInfo
    {
      get
      {
        return _relatedInfo;
      }
      set
      {
        __isset.relatedInfo = true;
        SetProperty(ref _relatedInfo, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool sledCommodityType;
      public bool subCommodityCodeNodes;
      public bool relatedInfo;
    }

    public HostingOrderRouteCommodityTypeNode() {
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
            if (field.Type == TType.I16) {
              SledCommodityType = iprot.ReadI16();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.Map) {
              {
                SubCommodityCodeNodes = new Dictionary<string, HostingOrderRouteCommodityCodeNode>();
                TMap _map17 = iprot.ReadMapBegin();
                for( int _i18 = 0; _i18 < _map17.Count; ++_i18)
                {
                  string _key19;
                  HostingOrderRouteCommodityCodeNode _val20;
                  _key19 = iprot.ReadString();
                  _val20 = new HostingOrderRouteCommodityCodeNode();
                  _val20.Read(iprot);
                  SubCommodityCodeNodes[_key19] = _val20;
                }
                iprot.ReadMapEnd();
              }
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.Struct) {
              RelatedInfo = new HostingOrderRouteRelatedInfo();
              RelatedInfo.Read(iprot);
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
      TStruct struc = new TStruct("HostingOrderRouteCommodityTypeNode");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.sledCommodityType) {
        field.Name = "sledCommodityType";
        field.Type = TType.I16;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI16(SledCommodityType);
        oprot.WriteFieldEnd();
      }
      if (SubCommodityCodeNodes != null && __isset.subCommodityCodeNodes) {
        field.Name = "subCommodityCodeNodes";
        field.Type = TType.Map;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteMapBegin(new TMap(TType.String, TType.Struct, SubCommodityCodeNodes.Count));
          foreach (string _iter21 in SubCommodityCodeNodes.Keys)
          {
            oprot.WriteString(_iter21);
            SubCommodityCodeNodes[_iter21].Write(oprot);
          }
          oprot.WriteMapEnd();
        }
        oprot.WriteFieldEnd();
      }
      if (RelatedInfo != null && __isset.relatedInfo) {
        field.Name = "relatedInfo";
        field.Type = TType.Struct;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        RelatedInfo.Write(oprot);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("HostingOrderRouteCommodityTypeNode(");
      sb.Append("SledCommodityType: ");
      sb.Append(SledCommodityType);
      sb.Append(",SubCommodityCodeNodes: ");
      if (SubCommodityCodeNodes == null)
      {
        sb.Append("<null>");
      }
      else
      {
        sb.Append("{");
        foreach (string _iter22 in SubCommodityCodeNodes.Keys)
        {
          sb.Append(_iter22.ToString());
          sb.Append(":");
          sb.Append(SubCommodityCodeNodes[_iter22].ToString());
          sb.Append(", ");
        }
        sb.Append("}");
      }
      sb.Append(",RelatedInfo: ");
      sb.Append(RelatedInfo== null ? "<null>" : RelatedInfo.ToString());
      sb.Append(")");
      return sb.ToString();
    }

  }

}