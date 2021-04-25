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
  /// 录入持仓明细查询条件
  /// 时间条件：大于 startTradeTimestamp, 小于 endTradeTimestamp
  /// 所有option使用逻辑与(AND)操作
  /// </summary>
  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class ReqPositionManualInputOption : TBase, INotifyPropertyChanged
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
    private long _subUserId;
    private long _sledContractId;
    private long _startTradeTimestampMs;
    private long _endTradeTimestampMs;
    private long _inputId;
    private long _subAccountId;
    private long _assignSubUserId;
    private xueqiao.trade.hosting.position.adjust.assign.thriftapi.PositionDirection _positionDirection;
    private long _startInputTimestampMs;
    private long _endInputTimestampMs;

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

    public long SledContractId
    {
      get
      {
        return _sledContractId;
      }
      set
      {
        __isset.sledContractId = true;
        SetProperty(ref _sledContractId, value);
      }
    }

    public long StartTradeTimestampMs
    {
      get
      {
        return _startTradeTimestampMs;
      }
      set
      {
        __isset.startTradeTimestampMs = true;
        SetProperty(ref _startTradeTimestampMs, value);
      }
    }

    public long EndTradeTimestampMs
    {
      get
      {
        return _endTradeTimestampMs;
      }
      set
      {
        __isset.endTradeTimestampMs = true;
        SetProperty(ref _endTradeTimestampMs, value);
      }
    }

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

    public long AssignSubUserId
    {
      get
      {
        return _assignSubUserId;
      }
      set
      {
        __isset.assignSubUserId = true;
        SetProperty(ref _assignSubUserId, value);
      }
    }

    /// <summary>
    /// 
    /// <seealso cref="xueqiao.trade.hosting.position.adjust.assign.thriftapi.PositionDirection"/>
    /// </summary>
    public xueqiao.trade.hosting.position.adjust.assign.thriftapi.PositionDirection PositionDirection
    {
      get
      {
        return _positionDirection;
      }
      set
      {
        __isset.positionDirection = true;
        SetProperty(ref _positionDirection, value);
      }
    }

    public long StartInputTimestampMs
    {
      get
      {
        return _startInputTimestampMs;
      }
      set
      {
        __isset.startInputTimestampMs = true;
        SetProperty(ref _startInputTimestampMs, value);
      }
    }

    public long EndInputTimestampMs
    {
      get
      {
        return _endInputTimestampMs;
      }
      set
      {
        __isset.endInputTimestampMs = true;
        SetProperty(ref _endInputTimestampMs, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool tradeAccountId;
      public bool subUserId;
      public bool sledContractId;
      public bool startTradeTimestampMs;
      public bool endTradeTimestampMs;
      public bool inputId;
      public bool subAccountId;
      public bool assignSubUserId;
      public bool positionDirection;
      public bool startInputTimestampMs;
      public bool endInputTimestampMs;
    }

    public ReqPositionManualInputOption() {
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
              TradeAccountId = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.I64) {
              SubUserId = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.I64) {
              SledContractId = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.I64) {
              StartTradeTimestampMs = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 5:
            if (field.Type == TType.I64) {
              EndTradeTimestampMs = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 6:
            if (field.Type == TType.I64) {
              InputId = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 7:
            if (field.Type == TType.I64) {
              SubAccountId = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 8:
            if (field.Type == TType.I64) {
              AssignSubUserId = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 9:
            if (field.Type == TType.I32) {
              PositionDirection = (xueqiao.trade.hosting.position.adjust.assign.thriftapi.PositionDirection)iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 10:
            if (field.Type == TType.I64) {
              StartInputTimestampMs = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 11:
            if (field.Type == TType.I64) {
              EndInputTimestampMs = iprot.ReadI64();
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
      TStruct struc = new TStruct("ReqPositionManualInputOption");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.tradeAccountId) {
        field.Name = "tradeAccountId";
        field.Type = TType.I64;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(TradeAccountId);
        oprot.WriteFieldEnd();
      }
      if (__isset.subUserId) {
        field.Name = "subUserId";
        field.Type = TType.I64;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(SubUserId);
        oprot.WriteFieldEnd();
      }
      if (__isset.sledContractId) {
        field.Name = "sledContractId";
        field.Type = TType.I64;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(SledContractId);
        oprot.WriteFieldEnd();
      }
      if (__isset.startTradeTimestampMs) {
        field.Name = "startTradeTimestampMs";
        field.Type = TType.I64;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(StartTradeTimestampMs);
        oprot.WriteFieldEnd();
      }
      if (__isset.endTradeTimestampMs) {
        field.Name = "endTradeTimestampMs";
        field.Type = TType.I64;
        field.ID = 5;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(EndTradeTimestampMs);
        oprot.WriteFieldEnd();
      }
      if (__isset.inputId) {
        field.Name = "inputId";
        field.Type = TType.I64;
        field.ID = 6;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(InputId);
        oprot.WriteFieldEnd();
      }
      if (__isset.subAccountId) {
        field.Name = "subAccountId";
        field.Type = TType.I64;
        field.ID = 7;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(SubAccountId);
        oprot.WriteFieldEnd();
      }
      if (__isset.assignSubUserId) {
        field.Name = "assignSubUserId";
        field.Type = TType.I64;
        field.ID = 8;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(AssignSubUserId);
        oprot.WriteFieldEnd();
      }
      if (__isset.positionDirection) {
        field.Name = "positionDirection";
        field.Type = TType.I32;
        field.ID = 9;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32((int)PositionDirection);
        oprot.WriteFieldEnd();
      }
      if (__isset.startInputTimestampMs) {
        field.Name = "startInputTimestampMs";
        field.Type = TType.I64;
        field.ID = 10;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(StartInputTimestampMs);
        oprot.WriteFieldEnd();
      }
      if (__isset.endInputTimestampMs) {
        field.Name = "endInputTimestampMs";
        field.Type = TType.I64;
        field.ID = 11;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(EndInputTimestampMs);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("ReqPositionManualInputOption(");
      sb.Append("TradeAccountId: ");
      sb.Append(TradeAccountId);
      sb.Append(",SubUserId: ");
      sb.Append(SubUserId);
      sb.Append(",SledContractId: ");
      sb.Append(SledContractId);
      sb.Append(",StartTradeTimestampMs: ");
      sb.Append(StartTradeTimestampMs);
      sb.Append(",EndTradeTimestampMs: ");
      sb.Append(EndTradeTimestampMs);
      sb.Append(",InputId: ");
      sb.Append(InputId);
      sb.Append(",SubAccountId: ");
      sb.Append(SubAccountId);
      sb.Append(",AssignSubUserId: ");
      sb.Append(AssignSubUserId);
      sb.Append(",PositionDirection: ");
      sb.Append(PositionDirection);
      sb.Append(",StartInputTimestampMs: ");
      sb.Append(StartInputTimestampMs);
      sb.Append(",EndInputTimestampMs: ");
      sb.Append(EndInputTimestampMs);
      sb.Append(")");
      return sb.ToString();
    }

  }

}