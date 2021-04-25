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

namespace xueqiao.broker
{

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class ReqBrokerOption : TBase, INotifyPropertyChanged
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
    private int _brokerId;
    private string _engNameWhole;
    private string _engNamePartical;
    private string _cnNameWhole;
    private string _cnNamePartical;
    private xueqiao.broker.TechPlatformEnv _env;

    public int BrokerId
    {
      get
      {
        return _brokerId;
      }
      set
      {
        __isset.brokerId = true;
        SetProperty(ref _brokerId, value);
      }
    }

    public string EngNameWhole
    {
      get
      {
        return _engNameWhole;
      }
      set
      {
        __isset.engNameWhole = true;
        SetProperty(ref _engNameWhole, value);
      }
    }

    public string EngNamePartical
    {
      get
      {
        return _engNamePartical;
      }
      set
      {
        __isset.engNamePartical = true;
        SetProperty(ref _engNamePartical, value);
      }
    }

    public string CnNameWhole
    {
      get
      {
        return _cnNameWhole;
      }
      set
      {
        __isset.cnNameWhole = true;
        SetProperty(ref _cnNameWhole, value);
      }
    }

    public string CnNamePartical
    {
      get
      {
        return _cnNamePartical;
      }
      set
      {
        __isset.cnNamePartical = true;
        SetProperty(ref _cnNamePartical, value);
      }
    }

    /// <summary>
    /// 
    /// <seealso cref="xueqiao.broker.TechPlatformEnv"/>
    /// </summary>
    public xueqiao.broker.TechPlatformEnv Env
    {
      get
      {
        return _env;
      }
      set
      {
        __isset.env = true;
        SetProperty(ref _env, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool brokerId;
      public bool engNameWhole;
      public bool engNamePartical;
      public bool cnNameWhole;
      public bool cnNamePartical;
      public bool env;
    }

    public ReqBrokerOption() {
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
              BrokerId = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.String) {
              EngNameWhole = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.String) {
              EngNamePartical = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.String) {
              CnNameWhole = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 5:
            if (field.Type == TType.String) {
              CnNamePartical = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 6:
            if (field.Type == TType.I32) {
              Env = (xueqiao.broker.TechPlatformEnv)iprot.ReadI32();
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
      TStruct struc = new TStruct("ReqBrokerOption");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.brokerId) {
        field.Name = "brokerId";
        field.Type = TType.I32;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(BrokerId);
        oprot.WriteFieldEnd();
      }
      if (EngNameWhole != null && __isset.engNameWhole) {
        field.Name = "engNameWhole";
        field.Type = TType.String;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(EngNameWhole);
        oprot.WriteFieldEnd();
      }
      if (EngNamePartical != null && __isset.engNamePartical) {
        field.Name = "engNamePartical";
        field.Type = TType.String;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(EngNamePartical);
        oprot.WriteFieldEnd();
      }
      if (CnNameWhole != null && __isset.cnNameWhole) {
        field.Name = "cnNameWhole";
        field.Type = TType.String;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(CnNameWhole);
        oprot.WriteFieldEnd();
      }
      if (CnNamePartical != null && __isset.cnNamePartical) {
        field.Name = "cnNamePartical";
        field.Type = TType.String;
        field.ID = 5;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(CnNamePartical);
        oprot.WriteFieldEnd();
      }
      if (__isset.env) {
        field.Name = "env";
        field.Type = TType.I32;
        field.ID = 6;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32((int)Env);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("ReqBrokerOption(");
      sb.Append("BrokerId: ");
      sb.Append(BrokerId);
      sb.Append(",EngNameWhole: ");
      sb.Append(EngNameWhole);
      sb.Append(",EngNamePartical: ");
      sb.Append(EngNamePartical);
      sb.Append(",CnNameWhole: ");
      sb.Append(CnNameWhole);
      sb.Append(",CnNamePartical: ");
      sb.Append(CnNamePartical);
      sb.Append(",Env: ");
      sb.Append(Env);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
