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
  public partial class HostingXQOrderWithTradeList : TBase, INotifyPropertyChanged
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
    private HostingXQOrder _order;
    private List<HostingXQTrade> _tradeList;

    public HostingXQOrder Order
    {
      get
      {
        return _order;
      }
      set
      {
        __isset.order = true;
        SetProperty(ref _order, value);
      }
    }

    public List<HostingXQTrade> TradeList
    {
      get
      {
        return _tradeList;
      }
      set
      {
        __isset.tradeList = true;
        SetProperty(ref _tradeList, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool order;
      public bool tradeList;
    }

    public HostingXQOrderWithTradeList() {
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
            if (field.Type == TType.Struct) {
              Order = new HostingXQOrder();
              Order.Read(iprot);
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.List) {
              {
                TradeList = new List<HostingXQTrade>();
                TList _list92 = iprot.ReadListBegin();
                for( int _i93 = 0; _i93 < _list92.Count; ++_i93)
                {
                  HostingXQTrade _elem94 = new HostingXQTrade();
                  _elem94 = new HostingXQTrade();
                  _elem94.Read(iprot);
                  TradeList.Add(_elem94);
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
      TStruct struc = new TStruct("HostingXQOrderWithTradeList");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (Order != null && __isset.order) {
        field.Name = "order";
        field.Type = TType.Struct;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        Order.Write(oprot);
        oprot.WriteFieldEnd();
      }
      if (TradeList != null && __isset.tradeList) {
        field.Name = "tradeList";
        field.Type = TType.List;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteListBegin(new TList(TType.Struct, TradeList.Count));
          foreach (HostingXQTrade _iter95 in TradeList)
          {
            _iter95.Write(oprot);
          }
          oprot.WriteListEnd();
        }
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("HostingXQOrderWithTradeList(");
      sb.Append("Order: ");
      sb.Append(Order== null ? "<null>" : Order.ToString());
      sb.Append(",TradeList: ");
      if (TradeList == null)
      {
        sb.Append("<null>");
      }
      else
      {
        sb.Append("[");
        foreach (HostingXQTrade _iter96 in TradeList)
        {
          sb.Append(_iter96.ToString());
          sb.Append(", ");
        }
        sb.Append("]");
      }
      sb.Append(")");
      return sb.ToString();
    }

  }

}
