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

namespace xueqiao.working.order.thriftapi
{

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class ReqWorkingOrderOption : TBase, INotifyPropertyChanged
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
    private long _orderId;
    private long _companyUserId;
    private WorkingOrderType _type;
    private WorkingOrderState _state;
    private THashSet<long> _orderIds;
    private long _companyId;
    private THashSet<long> _companyIds;
    private THashSet<long> _companyUserIds;

    public long OrderId
    {
      get
      {
        return _orderId;
      }
      set
      {
        __isset.orderId = true;
        SetProperty(ref _orderId, value);
      }
    }

    public long CompanyUserId
    {
      get
      {
        return _companyUserId;
      }
      set
      {
        __isset.companyUserId = true;
        SetProperty(ref _companyUserId, value);
      }
    }

    /// <summary>
    /// 
    /// <seealso cref="WorkingOrderType"/>
    /// </summary>
    public WorkingOrderType Type
    {
      get
      {
        return _type;
      }
      set
      {
        __isset.type = true;
        SetProperty(ref _type, value);
      }
    }

    /// <summary>
    /// 
    /// <seealso cref="WorkingOrderState"/>
    /// </summary>
    public WorkingOrderState State
    {
      get
      {
        return _state;
      }
      set
      {
        __isset.state = true;
        SetProperty(ref _state, value);
      }
    }

    public THashSet<long> OrderIds
    {
      get
      {
        return _orderIds;
      }
      set
      {
        __isset.orderIds = true;
        SetProperty(ref _orderIds, value);
      }
    }

    public long CompanyId
    {
      get
      {
        return _companyId;
      }
      set
      {
        __isset.companyId = true;
        SetProperty(ref _companyId, value);
      }
    }

    public THashSet<long> CompanyIds
    {
      get
      {
        return _companyIds;
      }
      set
      {
        __isset.companyIds = true;
        SetProperty(ref _companyIds, value);
      }
    }

    public THashSet<long> CompanyUserIds
    {
      get
      {
        return _companyUserIds;
      }
      set
      {
        __isset.companyUserIds = true;
        SetProperty(ref _companyUserIds, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool orderId;
      public bool companyUserId;
      public bool type;
      public bool state;
      public bool orderIds;
      public bool companyId;
      public bool companyIds;
      public bool companyUserIds;
    }

    public ReqWorkingOrderOption() {
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
              OrderId = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.I64) {
              CompanyUserId = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.I32) {
              Type = (WorkingOrderType)iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.I32) {
              State = (WorkingOrderState)iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 5:
            if (field.Type == TType.Set) {
              {
                OrderIds = new THashSet<long>();
                TSet _set6 = iprot.ReadSetBegin();
                for( int _i7 = 0; _i7 < _set6.Count; ++_i7)
                {
                  long _elem8 = 0;
                  _elem8 = iprot.ReadI64();
                  OrderIds.Add(_elem8);
                }
                iprot.ReadSetEnd();
              }
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 6:
            if (field.Type == TType.I64) {
              CompanyId = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 7:
            if (field.Type == TType.Set) {
              {
                CompanyIds = new THashSet<long>();
                TSet _set9 = iprot.ReadSetBegin();
                for( int _i10 = 0; _i10 < _set9.Count; ++_i10)
                {
                  long _elem11 = 0;
                  _elem11 = iprot.ReadI64();
                  CompanyIds.Add(_elem11);
                }
                iprot.ReadSetEnd();
              }
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 8:
            if (field.Type == TType.Set) {
              {
                CompanyUserIds = new THashSet<long>();
                TSet _set12 = iprot.ReadSetBegin();
                for( int _i13 = 0; _i13 < _set12.Count; ++_i13)
                {
                  long _elem14 = 0;
                  _elem14 = iprot.ReadI64();
                  CompanyUserIds.Add(_elem14);
                }
                iprot.ReadSetEnd();
              }
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
      TStruct struc = new TStruct("ReqWorkingOrderOption");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.orderId) {
        field.Name = "orderId";
        field.Type = TType.I64;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(OrderId);
        oprot.WriteFieldEnd();
      }
      if (__isset.companyUserId) {
        field.Name = "companyUserId";
        field.Type = TType.I64;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(CompanyUserId);
        oprot.WriteFieldEnd();
      }
      if (__isset.type) {
        field.Name = "type";
        field.Type = TType.I32;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32((int)Type);
        oprot.WriteFieldEnd();
      }
      if (__isset.state) {
        field.Name = "state";
        field.Type = TType.I32;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32((int)State);
        oprot.WriteFieldEnd();
      }
      if (OrderIds != null && __isset.orderIds) {
        field.Name = "orderIds";
        field.Type = TType.Set;
        field.ID = 5;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteSetBegin(new TSet(TType.I64, OrderIds.Count));
          foreach (long _iter15 in OrderIds)
          {
            oprot.WriteI64(_iter15);
          }
          oprot.WriteSetEnd();
        }
        oprot.WriteFieldEnd();
      }
      if (__isset.companyId) {
        field.Name = "companyId";
        field.Type = TType.I64;
        field.ID = 6;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(CompanyId);
        oprot.WriteFieldEnd();
      }
      if (CompanyIds != null && __isset.companyIds) {
        field.Name = "companyIds";
        field.Type = TType.Set;
        field.ID = 7;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteSetBegin(new TSet(TType.I64, CompanyIds.Count));
          foreach (long _iter16 in CompanyIds)
          {
            oprot.WriteI64(_iter16);
          }
          oprot.WriteSetEnd();
        }
        oprot.WriteFieldEnd();
      }
      if (CompanyUserIds != null && __isset.companyUserIds) {
        field.Name = "companyUserIds";
        field.Type = TType.Set;
        field.ID = 8;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteSetBegin(new TSet(TType.I64, CompanyUserIds.Count));
          foreach (long _iter17 in CompanyUserIds)
          {
            oprot.WriteI64(_iter17);
          }
          oprot.WriteSetEnd();
        }
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("ReqWorkingOrderOption(");
      sb.Append("OrderId: ");
      sb.Append(OrderId);
      sb.Append(",CompanyUserId: ");
      sb.Append(CompanyUserId);
      sb.Append(",Type: ");
      sb.Append(Type);
      sb.Append(",State: ");
      sb.Append(State);
      sb.Append(",OrderIds: ");
      if (OrderIds == null)
      {
        sb.Append("<null>");
      }
      else
      {
        sb.Append("[");
        foreach (long _iter18 in OrderIds)
        {
          sb.Append(_iter18.ToString());
          sb.Append(", ");
        }
        sb.Append("]");
      }
      sb.Append(",CompanyId: ");
      sb.Append(CompanyId);
      sb.Append(",CompanyIds: ");
      if (CompanyIds == null)
      {
        sb.Append("<null>");
      }
      else
      {
        sb.Append("[");
        foreach (long _iter19 in CompanyIds)
        {
          sb.Append(_iter19.ToString());
          sb.Append(", ");
        }
        sb.Append("]");
      }
      sb.Append(",CompanyUserIds: ");
      if (CompanyUserIds == null)
      {
        sb.Append("<null>");
      }
      else
      {
        sb.Append("[");
        foreach (long _iter20 in CompanyUserIds)
        {
          sb.Append(_iter20.ToString());
          sb.Append(", ");
        }
        sb.Append("]");
      }
      sb.Append(")");
      return sb.ToString();
    }

  }

}