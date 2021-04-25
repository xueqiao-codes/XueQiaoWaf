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

namespace xueqiao.contract
{

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class ReqCommoditySpecTradeTimeOption : TBase, INotifyPropertyChanged
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
    private THashSet<int> _sledCommodityIds;
    private long _startTimestamp;
    private long _endTimestamp;

    public THashSet<int> SledCommodityIds
    {
      get
      {
        return _sledCommodityIds;
      }
      set
      {
        __isset.sledCommodityIds = true;
        SetProperty(ref _sledCommodityIds, value);
      }
    }

    public long StartTimestamp
    {
      get
      {
        return _startTimestamp;
      }
      set
      {
        __isset.startTimestamp = true;
        SetProperty(ref _startTimestamp, value);
      }
    }

    public long EndTimestamp
    {
      get
      {
        return _endTimestamp;
      }
      set
      {
        __isset.endTimestamp = true;
        SetProperty(ref _endTimestamp, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool sledCommodityIds;
      public bool startTimestamp;
      public bool endTimestamp;
    }

    public ReqCommoditySpecTradeTimeOption() {
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
            if (field.Type == TType.Set) {
              {
                SledCommodityIds = new THashSet<int>();
                TSet _set187 = iprot.ReadSetBegin();
                for( int _i188 = 0; _i188 < _set187.Count; ++_i188)
                {
                  int _elem189 = 0;
                  _elem189 = iprot.ReadI32();
                  SledCommodityIds.Add(_elem189);
                }
                iprot.ReadSetEnd();
              }
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.I64) {
              StartTimestamp = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.I64) {
              EndTimestamp = iprot.ReadI64();
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
      TStruct struc = new TStruct("ReqCommoditySpecTradeTimeOption");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (SledCommodityIds != null && __isset.sledCommodityIds) {
        field.Name = "sledCommodityIds";
        field.Type = TType.Set;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteSetBegin(new TSet(TType.I32, SledCommodityIds.Count));
          foreach (int _iter190 in SledCommodityIds)
          {
            oprot.WriteI32(_iter190);
          }
          oprot.WriteSetEnd();
        }
        oprot.WriteFieldEnd();
      }
      if (__isset.startTimestamp) {
        field.Name = "startTimestamp";
        field.Type = TType.I64;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(StartTimestamp);
        oprot.WriteFieldEnd();
      }
      if (__isset.endTimestamp) {
        field.Name = "endTimestamp";
        field.Type = TType.I64;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(EndTimestamp);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("ReqCommoditySpecTradeTimeOption(");
      sb.Append("SledCommodityIds: ");
      if (SledCommodityIds == null)
      {
        sb.Append("<null>");
      }
      else
      {
        sb.Append("[");
        foreach (int _iter191 in SledCommodityIds)
        {
          sb.Append(_iter191.ToString());
          sb.Append(", ");
        }
        sb.Append("]");
      }
      sb.Append(",StartTimestamp: ");
      sb.Append(StartTimestamp);
      sb.Append(",EndTimestamp: ");
      sb.Append(EndTimestamp);
      sb.Append(")");
      return sb.ToString();
    }

  }

}