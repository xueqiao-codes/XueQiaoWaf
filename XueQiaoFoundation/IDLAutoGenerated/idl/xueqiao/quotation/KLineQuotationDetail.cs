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

namespace xueqiao.quotation
{

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class KLineQuotationDetail : TBase, INotifyPropertyChanged
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
    private double _kLineOpenPrice;
    private double _kLineHighPrice;
    private double _kLineLowPrice;
    private double _kLineClosePrice;
    private long _kLineQty;
    private double _kLineSettlementPrice;
    private long _kLineOpenInterest;

    public double KLineOpenPrice
    {
      get
      {
        return _kLineOpenPrice;
      }
      set
      {
        __isset.kLineOpenPrice = true;
        SetProperty(ref _kLineOpenPrice, value);
      }
    }

    public double KLineHighPrice
    {
      get
      {
        return _kLineHighPrice;
      }
      set
      {
        __isset.kLineHighPrice = true;
        SetProperty(ref _kLineHighPrice, value);
      }
    }

    public double KLineLowPrice
    {
      get
      {
        return _kLineLowPrice;
      }
      set
      {
        __isset.kLineLowPrice = true;
        SetProperty(ref _kLineLowPrice, value);
      }
    }

    public double KLineClosePrice
    {
      get
      {
        return _kLineClosePrice;
      }
      set
      {
        __isset.kLineClosePrice = true;
        SetProperty(ref _kLineClosePrice, value);
      }
    }

    public long KLineQty
    {
      get
      {
        return _kLineQty;
      }
      set
      {
        __isset.kLineQty = true;
        SetProperty(ref _kLineQty, value);
      }
    }

    public double KLineSettlementPrice
    {
      get
      {
        return _kLineSettlementPrice;
      }
      set
      {
        __isset.kLineSettlementPrice = true;
        SetProperty(ref _kLineSettlementPrice, value);
      }
    }

    public long KLineOpenInterest
    {
      get
      {
        return _kLineOpenInterest;
      }
      set
      {
        __isset.kLineOpenInterest = true;
        SetProperty(ref _kLineOpenInterest, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool kLineOpenPrice;
      public bool kLineHighPrice;
      public bool kLineLowPrice;
      public bool kLineClosePrice;
      public bool kLineQty;
      public bool kLineSettlementPrice;
      public bool kLineOpenInterest;
    }

    public KLineQuotationDetail() {
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
            if (field.Type == TType.Double) {
              KLineOpenPrice = iprot.ReadDouble();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.Double) {
              KLineHighPrice = iprot.ReadDouble();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.Double) {
              KLineLowPrice = iprot.ReadDouble();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.Double) {
              KLineClosePrice = iprot.ReadDouble();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 5:
            if (field.Type == TType.I64) {
              KLineQty = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 6:
            if (field.Type == TType.Double) {
              KLineSettlementPrice = iprot.ReadDouble();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 7:
            if (field.Type == TType.I64) {
              KLineOpenInterest = iprot.ReadI64();
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
      TStruct struc = new TStruct("KLineQuotationDetail");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.kLineOpenPrice) {
        field.Name = "kLineOpenPrice";
        field.Type = TType.Double;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteDouble(KLineOpenPrice);
        oprot.WriteFieldEnd();
      }
      if (__isset.kLineHighPrice) {
        field.Name = "kLineHighPrice";
        field.Type = TType.Double;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteDouble(KLineHighPrice);
        oprot.WriteFieldEnd();
      }
      if (__isset.kLineLowPrice) {
        field.Name = "kLineLowPrice";
        field.Type = TType.Double;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteDouble(KLineLowPrice);
        oprot.WriteFieldEnd();
      }
      if (__isset.kLineClosePrice) {
        field.Name = "kLineClosePrice";
        field.Type = TType.Double;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteDouble(KLineClosePrice);
        oprot.WriteFieldEnd();
      }
      if (__isset.kLineQty) {
        field.Name = "kLineQty";
        field.Type = TType.I64;
        field.ID = 5;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(KLineQty);
        oprot.WriteFieldEnd();
      }
      if (__isset.kLineSettlementPrice) {
        field.Name = "kLineSettlementPrice";
        field.Type = TType.Double;
        field.ID = 6;
        oprot.WriteFieldBegin(field);
        oprot.WriteDouble(KLineSettlementPrice);
        oprot.WriteFieldEnd();
      }
      if (__isset.kLineOpenInterest) {
        field.Name = "kLineOpenInterest";
        field.Type = TType.I64;
        field.ID = 7;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(KLineOpenInterest);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("KLineQuotationDetail(");
      sb.Append("KLineOpenPrice: ");
      sb.Append(KLineOpenPrice);
      sb.Append(",KLineHighPrice: ");
      sb.Append(KLineHighPrice);
      sb.Append(",KLineLowPrice: ");
      sb.Append(KLineLowPrice);
      sb.Append(",KLineClosePrice: ");
      sb.Append(KLineClosePrice);
      sb.Append(",KLineQty: ");
      sb.Append(KLineQty);
      sb.Append(",KLineSettlementPrice: ");
      sb.Append(KLineSettlementPrice);
      sb.Append(",KLineOpenInterest: ");
      sb.Append(KLineOpenInterest);
      sb.Append(")");
      return sb.ToString();
    }

  }

}