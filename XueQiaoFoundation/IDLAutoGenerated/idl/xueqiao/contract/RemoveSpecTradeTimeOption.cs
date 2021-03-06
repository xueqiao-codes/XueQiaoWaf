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
  public partial class RemoveSpecTradeTimeOption : TBase, INotifyPropertyChanged
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
    private List<int> _specTradeTimeIds;

    public List<int> SpecTradeTimeIds
    {
      get
      {
        return _specTradeTimeIds;
      }
      set
      {
        __isset.specTradeTimeIds = true;
        SetProperty(ref _specTradeTimeIds, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool specTradeTimeIds;
    }

    public RemoveSpecTradeTimeOption() {
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
                SpecTradeTimeIds = new List<int>();
                TList _list207 = iprot.ReadListBegin();
                for( int _i208 = 0; _i208 < _list207.Count; ++_i208)
                {
                  int _elem209 = 0;
                  _elem209 = iprot.ReadI32();
                  SpecTradeTimeIds.Add(_elem209);
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
      TStruct struc = new TStruct("RemoveSpecTradeTimeOption");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (SpecTradeTimeIds != null && __isset.specTradeTimeIds) {
        field.Name = "specTradeTimeIds";
        field.Type = TType.List;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteListBegin(new TList(TType.I32, SpecTradeTimeIds.Count));
          foreach (int _iter210 in SpecTradeTimeIds)
          {
            oprot.WriteI32(_iter210);
          }
          oprot.WriteListEnd();
        }
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("RemoveSpecTradeTimeOption(");
      sb.Append("SpecTradeTimeIds: ");
      if (SpecTradeTimeIds == null)
      {
        sb.Append("<null>");
      }
      else
      {
        sb.Append("[");
        foreach (int _iter211 in SpecTradeTimeIds)
        {
          sb.Append(_iter211.ToString());
          sb.Append(", ");
        }
        sb.Append("]");
      }
      sb.Append(")");
      return sb.ToString();
    }

  }

}
