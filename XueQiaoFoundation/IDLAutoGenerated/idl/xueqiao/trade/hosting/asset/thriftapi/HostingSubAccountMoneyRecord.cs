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

namespace xueqiao.trade.hosting.asset.thriftapi
{

  /// <summary>
  /// 托管机出入金记录
  /// </summary>
  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class HostingSubAccountMoneyRecord : TBase, INotifyPropertyChanged
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
    private HostingSubAccountMoneyRecordDirection _direction;
    private double _totalAmount;
    private double _depositAmountBefore;
    private double _depositAmountAfter;
    private double _withdrawAmountBefore;
    private double _withdrawAmountAfter;
    private long _recordTimestampMs;
    private string _ticket;
    private string _currency;
    private long _createTimestampMs;
    private long _lastModifyTimestampMs;

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

    /// <summary>
    /// 
    /// <seealso cref="HostingSubAccountMoneyRecordDirection"/>
    /// </summary>
    public HostingSubAccountMoneyRecordDirection Direction
    {
      get
      {
        return _direction;
      }
      set
      {
        __isset.direction = true;
        SetProperty(ref _direction, value);
      }
    }

    public double TotalAmount
    {
      get
      {
        return _totalAmount;
      }
      set
      {
        __isset.totalAmount = true;
        SetProperty(ref _totalAmount, value);
      }
    }

    public double DepositAmountBefore
    {
      get
      {
        return _depositAmountBefore;
      }
      set
      {
        __isset.depositAmountBefore = true;
        SetProperty(ref _depositAmountBefore, value);
      }
    }

    public double DepositAmountAfter
    {
      get
      {
        return _depositAmountAfter;
      }
      set
      {
        __isset.depositAmountAfter = true;
        SetProperty(ref _depositAmountAfter, value);
      }
    }

    public double WithdrawAmountBefore
    {
      get
      {
        return _withdrawAmountBefore;
      }
      set
      {
        __isset.withdrawAmountBefore = true;
        SetProperty(ref _withdrawAmountBefore, value);
      }
    }

    public double WithdrawAmountAfter
    {
      get
      {
        return _withdrawAmountAfter;
      }
      set
      {
        __isset.withdrawAmountAfter = true;
        SetProperty(ref _withdrawAmountAfter, value);
      }
    }

    public long RecordTimestampMs
    {
      get
      {
        return _recordTimestampMs;
      }
      set
      {
        __isset.recordTimestampMs = true;
        SetProperty(ref _recordTimestampMs, value);
      }
    }

    public string Ticket
    {
      get
      {
        return _ticket;
      }
      set
      {
        __isset.ticket = true;
        SetProperty(ref _ticket, value);
      }
    }

    public string Currency
    {
      get
      {
        return _currency;
      }
      set
      {
        __isset.currency = true;
        SetProperty(ref _currency, value);
      }
    }

    public long CreateTimestampMs
    {
      get
      {
        return _createTimestampMs;
      }
      set
      {
        __isset.createTimestampMs = true;
        SetProperty(ref _createTimestampMs, value);
      }
    }

    public long LastModifyTimestampMs
    {
      get
      {
        return _lastModifyTimestampMs;
      }
      set
      {
        __isset.lastModifyTimestampMs = true;
        SetProperty(ref _lastModifyTimestampMs, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool subAccountId;
      public bool direction;
      public bool totalAmount;
      public bool depositAmountBefore;
      public bool depositAmountAfter;
      public bool withdrawAmountBefore;
      public bool withdrawAmountAfter;
      public bool recordTimestampMs;
      public bool ticket;
      public bool currency;
      public bool createTimestampMs;
      public bool lastModifyTimestampMs;
    }

    public HostingSubAccountMoneyRecord() {
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
            if (field.Type == TType.I32) {
              Direction = (HostingSubAccountMoneyRecordDirection)iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.Double) {
              TotalAmount = iprot.ReadDouble();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 5:
            if (field.Type == TType.Double) {
              DepositAmountBefore = iprot.ReadDouble();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 6:
            if (field.Type == TType.Double) {
              DepositAmountAfter = iprot.ReadDouble();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 7:
            if (field.Type == TType.Double) {
              WithdrawAmountBefore = iprot.ReadDouble();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 8:
            if (field.Type == TType.Double) {
              WithdrawAmountAfter = iprot.ReadDouble();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 9:
            if (field.Type == TType.I64) {
              RecordTimestampMs = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 10:
            if (field.Type == TType.String) {
              Ticket = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 11:
            if (field.Type == TType.String) {
              Currency = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 20:
            if (field.Type == TType.I64) {
              CreateTimestampMs = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 21:
            if (field.Type == TType.I64) {
              LastModifyTimestampMs = iprot.ReadI64();
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
      TStruct struc = new TStruct("HostingSubAccountMoneyRecord");
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
      if (__isset.direction) {
        field.Name = "direction";
        field.Type = TType.I32;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32((int)Direction);
        oprot.WriteFieldEnd();
      }
      if (__isset.totalAmount) {
        field.Name = "totalAmount";
        field.Type = TType.Double;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteDouble(TotalAmount);
        oprot.WriteFieldEnd();
      }
      if (__isset.depositAmountBefore) {
        field.Name = "depositAmountBefore";
        field.Type = TType.Double;
        field.ID = 5;
        oprot.WriteFieldBegin(field);
        oprot.WriteDouble(DepositAmountBefore);
        oprot.WriteFieldEnd();
      }
      if (__isset.depositAmountAfter) {
        field.Name = "depositAmountAfter";
        field.Type = TType.Double;
        field.ID = 6;
        oprot.WriteFieldBegin(field);
        oprot.WriteDouble(DepositAmountAfter);
        oprot.WriteFieldEnd();
      }
      if (__isset.withdrawAmountBefore) {
        field.Name = "withdrawAmountBefore";
        field.Type = TType.Double;
        field.ID = 7;
        oprot.WriteFieldBegin(field);
        oprot.WriteDouble(WithdrawAmountBefore);
        oprot.WriteFieldEnd();
      }
      if (__isset.withdrawAmountAfter) {
        field.Name = "withdrawAmountAfter";
        field.Type = TType.Double;
        field.ID = 8;
        oprot.WriteFieldBegin(field);
        oprot.WriteDouble(WithdrawAmountAfter);
        oprot.WriteFieldEnd();
      }
      if (__isset.recordTimestampMs) {
        field.Name = "recordTimestampMs";
        field.Type = TType.I64;
        field.ID = 9;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(RecordTimestampMs);
        oprot.WriteFieldEnd();
      }
      if (Ticket != null && __isset.ticket) {
        field.Name = "ticket";
        field.Type = TType.String;
        field.ID = 10;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Ticket);
        oprot.WriteFieldEnd();
      }
      if (Currency != null && __isset.currency) {
        field.Name = "currency";
        field.Type = TType.String;
        field.ID = 11;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Currency);
        oprot.WriteFieldEnd();
      }
      if (__isset.createTimestampMs) {
        field.Name = "createTimestampMs";
        field.Type = TType.I64;
        field.ID = 20;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(CreateTimestampMs);
        oprot.WriteFieldEnd();
      }
      if (__isset.lastModifyTimestampMs) {
        field.Name = "lastModifyTimestampMs";
        field.Type = TType.I64;
        field.ID = 21;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(LastModifyTimestampMs);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("HostingSubAccountMoneyRecord(");
      sb.Append("SubAccountId: ");
      sb.Append(SubAccountId);
      sb.Append(",Direction: ");
      sb.Append(Direction);
      sb.Append(",TotalAmount: ");
      sb.Append(TotalAmount);
      sb.Append(",DepositAmountBefore: ");
      sb.Append(DepositAmountBefore);
      sb.Append(",DepositAmountAfter: ");
      sb.Append(DepositAmountAfter);
      sb.Append(",WithdrawAmountBefore: ");
      sb.Append(WithdrawAmountBefore);
      sb.Append(",WithdrawAmountAfter: ");
      sb.Append(WithdrawAmountAfter);
      sb.Append(",RecordTimestampMs: ");
      sb.Append(RecordTimestampMs);
      sb.Append(",Ticket: ");
      sb.Append(Ticket);
      sb.Append(",Currency: ");
      sb.Append(Currency);
      sb.Append(",CreateTimestampMs: ");
      sb.Append(CreateTimestampMs);
      sb.Append(",LastModifyTimestampMs: ");
      sb.Append(LastModifyTimestampMs);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
