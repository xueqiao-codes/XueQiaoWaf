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

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class ReqHostingPositionFundOption : TBase, INotifyPropertyChanged
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
    private THashSet<long> _sledContractId;

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

    public THashSet<long> SledContractId
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
      public bool subAccountId;
      public bool sledContractId;
    }

    public ReqHostingPositionFundOption() {
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
            if (field.Type == TType.Set) {
              {
                SledContractId = new THashSet<long>();
                TSet _set50 = iprot.ReadSetBegin();
                for( int _i51 = 0; _i51 < _set50.Count; ++_i51)
                {
                  long _elem52 = 0;
                  _elem52 = iprot.ReadI64();
                  SledContractId.Add(_elem52);
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
      TStruct struc = new TStruct("ReqHostingPositionFundOption");
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
      if (SledContractId != null && __isset.sledContractId) {
        field.Name = "sledContractId";
        field.Type = TType.Set;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteSetBegin(new TSet(TType.I64, SledContractId.Count));
          foreach (long _iter53 in SledContractId)
          {
            oprot.WriteI64(_iter53);
          }
          oprot.WriteSetEnd();
        }
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("ReqHostingPositionFundOption(");
      sb.Append("SubAccountId: ");
      sb.Append(SubAccountId);
      sb.Append(",SledContractId: ");
      if (SledContractId == null)
      {
        sb.Append("<null>");
      }
      else
      {
        sb.Append("[");
        foreach (long _iter54 in SledContractId)
        {
          sb.Append(_iter54.ToString());
          sb.Append(", ");
        }
        sb.Append("]");
      }
      sb.Append(")");
      return sb.ToString();
    }

  }

}