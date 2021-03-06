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
  public partial class StatClosedPositionSummary : TBase, INotifyPropertyChanged
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
    private long _closedPosition;
    private List<ClosedProfit> _closedProfits;
    private double _spreadProfit;
    private long _closedTimestampMs;
    private long _archivedDateTimestampMs;

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

    public long ClosedPosition
    {
      get
      {
        return _closedPosition;
      }
      set
      {
        __isset.closedPosition = true;
        SetProperty(ref _closedPosition, value);
      }
    }

    public List<ClosedProfit> ClosedProfits
    {
      get
      {
        return _closedProfits;
      }
      set
      {
        __isset.closedProfits = true;
        SetProperty(ref _closedProfits, value);
      }
    }

    public double SpreadProfit
    {
      get
      {
        return _spreadProfit;
      }
      set
      {
        __isset.spreadProfit = true;
        SetProperty(ref _spreadProfit, value);
      }
    }

    public long ClosedTimestampMs
    {
      get
      {
        return _closedTimestampMs;
      }
      set
      {
        __isset.closedTimestampMs = true;
        SetProperty(ref _closedTimestampMs, value);
      }
    }

    public long ArchivedDateTimestampMs
    {
      get
      {
        return _archivedDateTimestampMs;
      }
      set
      {
        __isset.archivedDateTimestampMs = true;
        SetProperty(ref _archivedDateTimestampMs, value);
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
      public bool closedPosition;
      public bool closedProfits;
      public bool spreadProfit;
      public bool closedTimestampMs;
      public bool archivedDateTimestampMs;
    }

    public StatClosedPositionSummary() {
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
          case 5:
            if (field.Type == TType.I64) {
              ClosedPosition = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 8:
            if (field.Type == TType.List) {
              {
                ClosedProfits = new List<ClosedProfit>();
                TList _list6 = iprot.ReadListBegin();
                for( int _i7 = 0; _i7 < _list6.Count; ++_i7)
                {
                  ClosedProfit _elem8 = new ClosedProfit();
                  _elem8 = new ClosedProfit();
                  _elem8.Read(iprot);
                  ClosedProfits.Add(_elem8);
                }
                iprot.ReadListEnd();
              }
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 9:
            if (field.Type == TType.Double) {
              SpreadProfit = iprot.ReadDouble();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 20:
            if (field.Type == TType.I64) {
              ClosedTimestampMs = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 21:
            if (field.Type == TType.I64) {
              ArchivedDateTimestampMs = iprot.ReadI64();
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
      TStruct struc = new TStruct("StatClosedPositionSummary");
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
      if (__isset.closedPosition) {
        field.Name = "closedPosition";
        field.Type = TType.I64;
        field.ID = 5;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(ClosedPosition);
        oprot.WriteFieldEnd();
      }
      if (ClosedProfits != null && __isset.closedProfits) {
        field.Name = "closedProfits";
        field.Type = TType.List;
        field.ID = 8;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteListBegin(new TList(TType.Struct, ClosedProfits.Count));
          foreach (ClosedProfit _iter9 in ClosedProfits)
          {
            _iter9.Write(oprot);
          }
          oprot.WriteListEnd();
        }
        oprot.WriteFieldEnd();
      }
      if (__isset.spreadProfit) {
        field.Name = "spreadProfit";
        field.Type = TType.Double;
        field.ID = 9;
        oprot.WriteFieldBegin(field);
        oprot.WriteDouble(SpreadProfit);
        oprot.WriteFieldEnd();
      }
      if (__isset.closedTimestampMs) {
        field.Name = "closedTimestampMs";
        field.Type = TType.I64;
        field.ID = 20;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(ClosedTimestampMs);
        oprot.WriteFieldEnd();
      }
      if (__isset.archivedDateTimestampMs) {
        field.Name = "archivedDateTimestampMs";
        field.Type = TType.I64;
        field.ID = 21;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(ArchivedDateTimestampMs);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("StatClosedPositionSummary(");
      sb.Append("ClosedId: ");
      sb.Append(ClosedId);
      sb.Append(",SubAccountId: ");
      sb.Append(SubAccountId);
      sb.Append(",TargetKey: ");
      sb.Append(TargetKey);
      sb.Append(",TargetType: ");
      sb.Append(TargetType);
      sb.Append(",ClosedPosition: ");
      sb.Append(ClosedPosition);
      sb.Append(",ClosedProfits: ");
      if (ClosedProfits == null)
      {
        sb.Append("<null>");
      }
      else
      {
        sb.Append("[");
        foreach (ClosedProfit _iter10 in ClosedProfits)
        {
          sb.Append(_iter10.ToString());
          sb.Append(", ");
        }
        sb.Append("]");
      }
      sb.Append(",SpreadProfit: ");
      sb.Append(SpreadProfit);
      sb.Append(",ClosedTimestampMs: ");
      sb.Append(ClosedTimestampMs);
      sb.Append(",ArchivedDateTimestampMs: ");
      sb.Append(ArchivedDateTimestampMs);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
