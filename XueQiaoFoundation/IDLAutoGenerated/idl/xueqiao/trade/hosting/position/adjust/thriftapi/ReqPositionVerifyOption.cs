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

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class ReqPositionVerifyOption : TBase, INotifyPropertyChanged
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
    private long _verifyId;
    private long _tradeAccountId;
    private long _startVerifyTimestampMs;
    private long _endVerifyTimestampMs;
    private bool _latest;
    private long _sledContractId;

    public long VerifyId
    {
      get
      {
        return _verifyId;
      }
      set
      {
        __isset.verifyId = true;
        SetProperty(ref _verifyId, value);
      }
    }

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

    public long StartVerifyTimestampMs
    {
      get
      {
        return _startVerifyTimestampMs;
      }
      set
      {
        __isset.startVerifyTimestampMs = true;
        SetProperty(ref _startVerifyTimestampMs, value);
      }
    }

    public long EndVerifyTimestampMs
    {
      get
      {
        return _endVerifyTimestampMs;
      }
      set
      {
        __isset.endVerifyTimestampMs = true;
        SetProperty(ref _endVerifyTimestampMs, value);
      }
    }

    public bool Latest
    {
      get
      {
        return _latest;
      }
      set
      {
        __isset.latest = true;
        SetProperty(ref _latest, value);
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


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool verifyId;
      public bool tradeAccountId;
      public bool startVerifyTimestampMs;
      public bool endVerifyTimestampMs;
      public bool latest;
      public bool sledContractId;
    }

    public ReqPositionVerifyOption() {
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
              VerifyId = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.I64) {
              TradeAccountId = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.I64) {
              StartVerifyTimestampMs = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.I64) {
              EndVerifyTimestampMs = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 5:
            if (field.Type == TType.Bool) {
              Latest = iprot.ReadBool();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 6:
            if (field.Type == TType.I64) {
              SledContractId = iprot.ReadI64();
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
      TStruct struc = new TStruct("ReqPositionVerifyOption");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.verifyId) {
        field.Name = "verifyId";
        field.Type = TType.I64;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(VerifyId);
        oprot.WriteFieldEnd();
      }
      if (__isset.tradeAccountId) {
        field.Name = "tradeAccountId";
        field.Type = TType.I64;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(TradeAccountId);
        oprot.WriteFieldEnd();
      }
      if (__isset.startVerifyTimestampMs) {
        field.Name = "startVerifyTimestampMs";
        field.Type = TType.I64;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(StartVerifyTimestampMs);
        oprot.WriteFieldEnd();
      }
      if (__isset.endVerifyTimestampMs) {
        field.Name = "endVerifyTimestampMs";
        field.Type = TType.I64;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(EndVerifyTimestampMs);
        oprot.WriteFieldEnd();
      }
      if (__isset.latest) {
        field.Name = "latest";
        field.Type = TType.Bool;
        field.ID = 5;
        oprot.WriteFieldBegin(field);
        oprot.WriteBool(Latest);
        oprot.WriteFieldEnd();
      }
      if (__isset.sledContractId) {
        field.Name = "sledContractId";
        field.Type = TType.I64;
        field.ID = 6;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(SledContractId);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("ReqPositionVerifyOption(");
      sb.Append("VerifyId: ");
      sb.Append(VerifyId);
      sb.Append(",TradeAccountId: ");
      sb.Append(TradeAccountId);
      sb.Append(",StartVerifyTimestampMs: ");
      sb.Append(StartVerifyTimestampMs);
      sb.Append(",EndVerifyTimestampMs: ");
      sb.Append(EndVerifyTimestampMs);
      sb.Append(",Latest: ");
      sb.Append(Latest);
      sb.Append(",SledContractId: ");
      sb.Append(SledContractId);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
