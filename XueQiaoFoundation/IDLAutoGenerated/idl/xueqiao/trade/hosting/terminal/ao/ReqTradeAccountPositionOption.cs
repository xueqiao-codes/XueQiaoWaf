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

namespace xueqiao.trade.hosting.terminal.ao
{

  /// <summary>
  /// 资金持仓的明细查询条件
  /// 时间条件：大于 startTradeTimestampMs, 小于 endTradeTimestampMs
  /// 所有option使用逻辑与(AND)操作
  /// </summary>
  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class ReqTradeAccountPositionOption : TBase, INotifyPropertyChanged
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
    private long _sledContractId;
    private long _startTradeTimestampMs;
    private long _endTradeTimestampMs;

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


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool tradeAccountId;
      public bool sledContractId;
      public bool startTradeTimestampMs;
      public bool endTradeTimestampMs;
    }

    public ReqTradeAccountPositionOption() {
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
          default: 
            TProtocolUtil.Skip(iprot, field.Type);
            break;
        }
        iprot.ReadFieldEnd();
      }
      iprot.ReadStructEnd();
    }

    public void Write(TProtocol oprot) {
      TStruct struc = new TStruct("ReqTradeAccountPositionOption");
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
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("ReqTradeAccountPositionOption(");
      sb.Append("TradeAccountId: ");
      sb.Append(TradeAccountId);
      sb.Append(",SledContractId: ");
      sb.Append(SledContractId);
      sb.Append(",StartTradeTimestampMs: ");
      sb.Append(StartTradeTimestampMs);
      sb.Append(",EndTradeTimestampMs: ");
      sb.Append(EndTradeTimestampMs);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
