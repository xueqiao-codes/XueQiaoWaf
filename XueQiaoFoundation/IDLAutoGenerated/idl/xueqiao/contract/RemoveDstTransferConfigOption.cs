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
  public partial class RemoveDstTransferConfigOption : TBase, INotifyPropertyChanged
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
    private List<int> _dstTransferConfigIds;

    public List<int> DstTransferConfigIds
    {
      get
      {
        return _dstTransferConfigIds;
      }
      set
      {
        __isset.dstTransferConfigIds = true;
        SetProperty(ref _dstTransferConfigIds, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool dstTransferConfigIds;
    }

    public RemoveDstTransferConfigOption() {
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
            if (field.Type == TType.List) {
              {
                DstTransferConfigIds = new List<int>();
                TList _list167 = iprot.ReadListBegin();
                for( int _i168 = 0; _i168 < _list167.Count; ++_i168)
                {
                  int _elem169 = 0;
                  _elem169 = iprot.ReadI32();
                  DstTransferConfigIds.Add(_elem169);
                }
                iprot.ReadListEnd();
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
      TStruct struc = new TStruct("RemoveDstTransferConfigOption");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (DstTransferConfigIds != null && __isset.dstTransferConfigIds) {
        field.Name = "dstTransferConfigIds";
        field.Type = TType.List;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteListBegin(new TList(TType.I32, DstTransferConfigIds.Count));
          foreach (int _iter170 in DstTransferConfigIds)
          {
            oprot.WriteI32(_iter170);
          }
          oprot.WriteListEnd();
        }
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("RemoveDstTransferConfigOption(");
      sb.Append("DstTransferConfigIds: ");
      if (DstTransferConfigIds == null)
      {
        sb.Append("<null>");
      }
      else
      {
        sb.Append("[");
        foreach (int _iter171 in DstTransferConfigIds)
        {
          sb.Append(_iter171.ToString());
          sb.Append(", ");
        }
        sb.Append("]");
      }
      sb.Append(")");
      return sb.ToString();
    }

  }

}