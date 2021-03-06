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
  public partial class CommoditySourceAccountPage : TBase, INotifyPropertyChanged
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
    private int _total;
    private List<CommoditySourceAccount> _page;

    public int Total
    {
      get
      {
        return _total;
      }
      set
      {
        __isset.total = true;
        SetProperty(ref _total, value);
      }
    }

    public List<CommoditySourceAccount> Page
    {
      get
      {
        return _page;
      }
      set
      {
        __isset.page = true;
        SetProperty(ref _page, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool total;
      public bool page;
    }

    public CommoditySourceAccountPage() {
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
              Total = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.List) {
              {
                Page = new List<CommoditySourceAccount>();
                TList _list248 = iprot.ReadListBegin();
                for( int _i249 = 0; _i249 < _list248.Count; ++_i249)
                {
                  CommoditySourceAccount _elem250 = new CommoditySourceAccount();
                  _elem250 = new CommoditySourceAccount();
                  _elem250.Read(iprot);
                  Page.Add(_elem250);
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
      TStruct struc = new TStruct("CommoditySourceAccountPage");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.total) {
        field.Name = "total";
        field.Type = TType.I32;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(Total);
        oprot.WriteFieldEnd();
      }
      if (Page != null && __isset.page) {
        field.Name = "page";
        field.Type = TType.List;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteListBegin(new TList(TType.Struct, Page.Count));
          foreach (CommoditySourceAccount _iter251 in Page)
          {
            _iter251.Write(oprot);
          }
          oprot.WriteListEnd();
        }
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("CommoditySourceAccountPage(");
      sb.Append("Total: ");
      sb.Append(Total);
      sb.Append(",Page: ");
      if (Page == null)
      {
        sb.Append("<null>");
      }
      else
      {
        sb.Append("[");
        foreach (CommoditySourceAccount _iter252 in Page)
        {
          sb.Append(_iter252.ToString());
          sb.Append(", ");
        }
        sb.Append("]");
      }
      sb.Append(")");
      return sb.ToString();
    }

  }

}
