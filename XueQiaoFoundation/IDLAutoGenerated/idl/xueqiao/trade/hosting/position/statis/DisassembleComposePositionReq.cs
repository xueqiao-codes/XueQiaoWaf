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

namespace xueqiao.trade.hosting.position.statis
{

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class DisassembleComposePositionReq : TBase, INotifyPropertyChanged
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
    private string _targetKey;
    private List<PositionItemData> _positionItemDataList;
    private xueqiao.trade.hosting.arbitrage.thriftapi.HostingXQTargetType _targetType;

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

    public string TargetKey
    {
      get
      {
        return _targetKey;
      }
      set
      {
        __isset.targetKey = true;
        SetProperty(ref _targetKey, value);
      }
    }

    public List<PositionItemData> PositionItemDataList
    {
      get
      {
        return _positionItemDataList;
      }
      set
      {
        __isset.positionItemDataList = true;
        SetProperty(ref _positionItemDataList, value);
      }
    }

    /// <summary>
    /// 
    /// <seealso cref="xueqiao.trade.hosting.arbitrage.thriftapi.HostingXQTargetType"/>
    /// </summary>
    public xueqiao.trade.hosting.arbitrage.thriftapi.HostingXQTargetType TargetType
    {
      get
      {
        return _targetType;
      }
      set
      {
        __isset.targetType = true;
        SetProperty(ref _targetType, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool subAccountId;
      public bool targetKey;
      public bool positionItemDataList;
      public bool targetType;
    }

    public DisassembleComposePositionReq() {
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
          case 2:
            if (field.Type == TType.String) {
              TargetKey = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.List) {
              {
                PositionItemDataList = new List<PositionItemData>();
                TList _list26 = iprot.ReadListBegin();
                for( int _i27 = 0; _i27 < _list26.Count; ++_i27)
                {
                  PositionItemData _elem28 = new PositionItemData();
                  _elem28 = new PositionItemData();
                  _elem28.Read(iprot);
                  PositionItemDataList.Add(_elem28);
                }
                iprot.ReadListEnd();
              }
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.I32) {
              TargetType = (xueqiao.trade.hosting.arbitrage.thriftapi.HostingXQTargetType)iprot.ReadI32();
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
      TStruct struc = new TStruct("DisassembleComposePositionReq");
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
      if (TargetKey != null && __isset.targetKey) {
        field.Name = "targetKey";
        field.Type = TType.String;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(TargetKey);
        oprot.WriteFieldEnd();
      }
      if (PositionItemDataList != null && __isset.positionItemDataList) {
        field.Name = "positionItemDataList";
        field.Type = TType.List;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteListBegin(new TList(TType.Struct, PositionItemDataList.Count));
          foreach (PositionItemData _iter29 in PositionItemDataList)
          {
            _iter29.Write(oprot);
          }
          oprot.WriteListEnd();
        }
        oprot.WriteFieldEnd();
      }
      if (__isset.targetType) {
        field.Name = "targetType";
        field.Type = TType.I32;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32((int)TargetType);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("DisassembleComposePositionReq(");
      sb.Append("SubAccountId: ");
      sb.Append(SubAccountId);
      sb.Append(",TargetKey: ");
      sb.Append(TargetKey);
      sb.Append(",PositionItemDataList: ");
      if (PositionItemDataList == null)
      {
        sb.Append("<null>");
      }
      else
      {
        sb.Append("[");
        foreach (PositionItemData _iter30 in PositionItemDataList)
        {
          sb.Append(_iter30.ToString());
          sb.Append(", ");
        }
        sb.Append("]");
      }
      sb.Append(",TargetType: ");
      sb.Append(TargetType);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
