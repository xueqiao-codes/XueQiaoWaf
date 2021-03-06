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

namespace xueqiao.trade.hosting.arbitrage.thriftapi
{

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class HostingXQComposeLimitOrderExecParams : TBase, INotifyPropertyChanged
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
    private HostingXQComposeLimitOrderExecType _execType;
    private int _execEveryQty;
    private HostingXQComposeLimitOrderParallelParams _execParallelParams;
    private HostingXQComposeLimitOrderLegByParams _execLegByParams;
    private int _earlySuspendedForMarketSeconds;

    /// <summary>
    /// 
    /// <seealso cref="HostingXQComposeLimitOrderExecType"/>
    /// </summary>
    public HostingXQComposeLimitOrderExecType ExecType
    {
      get
      {
        return _execType;
      }
      set
      {
        __isset.execType = true;
        SetProperty(ref _execType, value);
      }
    }

    public int ExecEveryQty
    {
      get
      {
        return _execEveryQty;
      }
      set
      {
        __isset.execEveryQty = true;
        SetProperty(ref _execEveryQty, value);
      }
    }

    public HostingXQComposeLimitOrderParallelParams ExecParallelParams
    {
      get
      {
        return _execParallelParams;
      }
      set
      {
        __isset.execParallelParams = true;
        SetProperty(ref _execParallelParams, value);
      }
    }

    public HostingXQComposeLimitOrderLegByParams ExecLegByParams
    {
      get
      {
        return _execLegByParams;
      }
      set
      {
        __isset.execLegByParams = true;
        SetProperty(ref _execLegByParams, value);
      }
    }

    public int EarlySuspendedForMarketSeconds
    {
      get
      {
        return _earlySuspendedForMarketSeconds;
      }
      set
      {
        __isset.earlySuspendedForMarketSeconds = true;
        SetProperty(ref _earlySuspendedForMarketSeconds, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool execType;
      public bool execEveryQty;
      public bool execParallelParams;
      public bool execLegByParams;
      public bool earlySuspendedForMarketSeconds;
    }

    public HostingXQComposeLimitOrderExecParams() {
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
            if (field.Type == TType.I32) {
              ExecType = (HostingXQComposeLimitOrderExecType)iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.I32) {
              ExecEveryQty = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.Struct) {
              ExecParallelParams = new HostingXQComposeLimitOrderParallelParams();
              ExecParallelParams.Read(iprot);
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.Struct) {
              ExecLegByParams = new HostingXQComposeLimitOrderLegByParams();
              ExecLegByParams.Read(iprot);
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 5:
            if (field.Type == TType.I32) {
              EarlySuspendedForMarketSeconds = iprot.ReadI32();
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
      TStruct struc = new TStruct("HostingXQComposeLimitOrderExecParams");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.execType) {
        field.Name = "execType";
        field.Type = TType.I32;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32((int)ExecType);
        oprot.WriteFieldEnd();
      }
      if (__isset.execEveryQty) {
        field.Name = "execEveryQty";
        field.Type = TType.I32;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(ExecEveryQty);
        oprot.WriteFieldEnd();
      }
      if (ExecParallelParams != null && __isset.execParallelParams) {
        field.Name = "execParallelParams";
        field.Type = TType.Struct;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        ExecParallelParams.Write(oprot);
        oprot.WriteFieldEnd();
      }
      if (ExecLegByParams != null && __isset.execLegByParams) {
        field.Name = "execLegByParams";
        field.Type = TType.Struct;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        ExecLegByParams.Write(oprot);
        oprot.WriteFieldEnd();
      }
      if (__isset.earlySuspendedForMarketSeconds) {
        field.Name = "earlySuspendedForMarketSeconds";
        field.Type = TType.I32;
        field.ID = 5;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(EarlySuspendedForMarketSeconds);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("HostingXQComposeLimitOrderExecParams(");
      sb.Append("ExecType: ");
      sb.Append(ExecType);
      sb.Append(",ExecEveryQty: ");
      sb.Append(ExecEveryQty);
      sb.Append(",ExecParallelParams: ");
      sb.Append(ExecParallelParams== null ? "<null>" : ExecParallelParams.ToString());
      sb.Append(",ExecLegByParams: ");
      sb.Append(ExecLegByParams== null ? "<null>" : ExecLegByParams.ToString());
      sb.Append(",EarlySuspendedForMarketSeconds: ");
      sb.Append(EarlySuspendedForMarketSeconds);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
