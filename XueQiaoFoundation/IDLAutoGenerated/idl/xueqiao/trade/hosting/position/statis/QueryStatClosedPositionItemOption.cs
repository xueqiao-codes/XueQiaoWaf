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
  public partial class QueryStatClosedPositionItemOption : TBase, INotifyPropertyChanged
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
    private long _closedId;
    private long _subAccountId;
    private string _targetKey;
    private xueqiao.trade.hosting.arbitrage.thriftapi.HostingXQTargetType _targetType;

    public long ClosedId
    {
      get
      {
        return _closedId;
      }
      set
      {
        __isset.closedId = true;
        SetProperty(ref _closedId, value);
      }
    }

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
      public bool closedId;
      public bool subAccountId;
      public bool targetKey;
      public bool targetType;
    }

    public QueryStatClosedPositionItemOption() {
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
              ClosedId = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.I64) {
              SubAccountId = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.String) {
              TargetKey = iprot.ReadString();
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
      TStruct struc = new TStruct("QueryStatClosedPositionItemOption");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.closedId) {
        field.Name = "closedId";
        field.Type = TType.I64;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(ClosedId);
        oprot.WriteFieldEnd();
      }
      if (__isset.subAccountId) {
        field.Name = "subAccountId";
        field.Type = TType.I64;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(SubAccountId);
        oprot.WriteFieldEnd();
      }
      if (TargetKey != null && __isset.targetKey) {
        field.Name = "targetKey";
        field.Type = TType.String;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(TargetKey);
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
      StringBuilder sb = new StringBuilder("QueryStatClosedPositionItemOption(");
      sb.Append("ClosedId: ");
      sb.Append(ClosedId);
      sb.Append(",SubAccountId: ");
      sb.Append(SubAccountId);
      sb.Append(",TargetKey: ");
      sb.Append(TargetKey);
      sb.Append(",TargetType: ");
      sb.Append(TargetType);
      sb.Append(")");
      return sb.ToString();
    }

  }

}