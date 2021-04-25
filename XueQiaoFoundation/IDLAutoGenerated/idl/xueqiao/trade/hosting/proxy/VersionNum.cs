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

namespace xueqiao.trade.hosting.proxy
{

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class VersionNum : TBase, INotifyPropertyChanged
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
    private int _majorVersionNum;
    private int _minorVersionNum;
    private int _buildVersionNum;
    private int _reversionNum;

    public int MajorVersionNum
    {
      get
      {
        return _majorVersionNum;
      }
      set
      {
        __isset.majorVersionNum = true;
        SetProperty(ref _majorVersionNum, value);
      }
    }

    public int MinorVersionNum
    {
      get
      {
        return _minorVersionNum;
      }
      set
      {
        __isset.minorVersionNum = true;
        SetProperty(ref _minorVersionNum, value);
      }
    }

    public int BuildVersionNum
    {
      get
      {
        return _buildVersionNum;
      }
      set
      {
        __isset.buildVersionNum = true;
        SetProperty(ref _buildVersionNum, value);
      }
    }

    public int ReversionNum
    {
      get
      {
        return _reversionNum;
      }
      set
      {
        __isset.reversionNum = true;
        SetProperty(ref _reversionNum, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool majorVersionNum;
      public bool minorVersionNum;
      public bool buildVersionNum;
      public bool reversionNum;
    }

    public VersionNum() {
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
              MajorVersionNum = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.I32) {
              MinorVersionNum = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.I32) {
              BuildVersionNum = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.I32) {
              ReversionNum = iprot.ReadI32();
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
      TStruct struc = new TStruct("VersionNum");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.majorVersionNum) {
        field.Name = "majorVersionNum";
        field.Type = TType.I32;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(MajorVersionNum);
        oprot.WriteFieldEnd();
      }
      if (__isset.minorVersionNum) {
        field.Name = "minorVersionNum";
        field.Type = TType.I32;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(MinorVersionNum);
        oprot.WriteFieldEnd();
      }
      if (__isset.buildVersionNum) {
        field.Name = "buildVersionNum";
        field.Type = TType.I32;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(BuildVersionNum);
        oprot.WriteFieldEnd();
      }
      if (__isset.reversionNum) {
        field.Name = "reversionNum";
        field.Type = TType.I32;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(ReversionNum);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("VersionNum(");
      sb.Append("MajorVersionNum: ");
      sb.Append(MajorVersionNum);
      sb.Append(",MinorVersionNum: ");
      sb.Append(MinorVersionNum);
      sb.Append(",BuildVersionNum: ");
      sb.Append(BuildVersionNum);
      sb.Append(",ReversionNum: ");
      sb.Append(ReversionNum);
      sb.Append(")");
      return sb.ToString();
    }

  }

}