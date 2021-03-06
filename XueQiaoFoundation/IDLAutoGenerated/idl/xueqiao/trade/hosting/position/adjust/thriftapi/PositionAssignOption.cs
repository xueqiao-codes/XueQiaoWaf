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

namespace xueqiao.trade.hosting.position.adjust.thriftapi
{

  /// <summary>
  /// 分配持仓输入信息
  /// </summary>
  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class PositionAssignOption : TBase, INotifyPropertyChanged
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
    private long _inputId;
    private long _subAccountId;
    private long _subUserId;
    private int _volume;

    public long InputId
    {
      get
      {
        return _inputId;
      }
      set
      {
        __isset.inputId = true;
        SetProperty(ref _inputId, value);
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

    public long SubUserId
    {
      get
      {
        return _subUserId;
      }
      set
      {
        __isset.subUserId = true;
        SetProperty(ref _subUserId, value);
      }
    }

    public int Volume
    {
      get
      {
        return _volume;
      }
      set
      {
        __isset.volume = true;
        SetProperty(ref _volume, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool inputId;
      public bool subAccountId;
      public bool subUserId;
      public bool volume;
    }

    public PositionAssignOption() {
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
              InputId = iprot.ReadI64();
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
            if (field.Type == TType.I64) {
              SubUserId = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.I32) {
              Volume = iprot.ReadI32();
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
      TStruct struc = new TStruct("PositionAssignOption");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.inputId) {
        field.Name = "inputId";
        field.Type = TType.I64;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(InputId);
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
      if (__isset.subUserId) {
        field.Name = "subUserId";
        field.Type = TType.I64;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(SubUserId);
        oprot.WriteFieldEnd();
      }
      if (__isset.volume) {
        field.Name = "volume";
        field.Type = TType.I32;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(Volume);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("PositionAssignOption(");
      sb.Append("InputId: ");
      sb.Append(InputId);
      sb.Append(",SubAccountId: ");
      sb.Append(SubAccountId);
      sb.Append(",SubUserId: ");
      sb.Append(SubUserId);
      sb.Append(",Volume: ");
      sb.Append(Volume);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
