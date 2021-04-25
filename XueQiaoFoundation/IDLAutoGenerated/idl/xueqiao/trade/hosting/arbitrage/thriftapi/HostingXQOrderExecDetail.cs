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
  public partial class HostingXQOrderExecDetail : TBase, INotifyPropertyChanged
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
    private HostingXQOrder _xqOrder;
    private List<HostingXQTrade> _xqTrades;
    private List<xueqiao.trade.hosting.HostingExecOrder> _execOrders;
    private List<xueqiao.trade.hosting.HostingExecTrade> _execTrades;
    private Dictionary<long, List<HostingXQTradeRelatedItem>> _xqTradeRelatedItems;

    public HostingXQOrder XqOrder
    {
      get
      {
        return _xqOrder;
      }
      set
      {
        __isset.xqOrder = true;
        SetProperty(ref _xqOrder, value);
      }
    }

    public List<HostingXQTrade> XqTrades
    {
      get
      {
        return _xqTrades;
      }
      set
      {
        __isset.xqTrades = true;
        SetProperty(ref _xqTrades, value);
      }
    }

    public List<xueqiao.trade.hosting.HostingExecOrder> ExecOrders
    {
      get
      {
        return _execOrders;
      }
      set
      {
        __isset.execOrders = true;
        SetProperty(ref _execOrders, value);
      }
    }

    public List<xueqiao.trade.hosting.HostingExecTrade> ExecTrades
    {
      get
      {
        return _execTrades;
      }
      set
      {
        __isset.execTrades = true;
        SetProperty(ref _execTrades, value);
      }
    }

    public Dictionary<long, List<HostingXQTradeRelatedItem>> XqTradeRelatedItems
    {
      get
      {
        return _xqTradeRelatedItems;
      }
      set
      {
        __isset.xqTradeRelatedItems = true;
        SetProperty(ref _xqTradeRelatedItems, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool xqOrder;
      public bool xqTrades;
      public bool execOrders;
      public bool execTrades;
      public bool xqTradeRelatedItems;
    }

    public HostingXQOrderExecDetail() {
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
              XqOrder = new HostingXQOrder();
              XqOrder.Read(iprot);
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.List) {
              {
                XqTrades = new List<HostingXQTrade>();
                TList _list67 = iprot.ReadListBegin();
                for( int _i68 = 0; _i68 < _list67.Count; ++_i68)
                {
                  HostingXQTrade _elem69 = new HostingXQTrade();
                  _elem69 = new HostingXQTrade();
                  _elem69.Read(iprot);
                  XqTrades.Add(_elem69);
                }
                iprot.ReadListEnd();
              }
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.List) {
              {
                ExecOrders = new List<xueqiao.trade.hosting.HostingExecOrder>();
                TList _list70 = iprot.ReadListBegin();
                for( int _i71 = 0; _i71 < _list70.Count; ++_i71)
                {
                  xueqiao.trade.hosting.HostingExecOrder _elem72 = new xueqiao.trade.hosting.HostingExecOrder();
                  _elem72 = new xueqiao.trade.hosting.HostingExecOrder();
                  _elem72.Read(iprot);
                  ExecOrders.Add(_elem72);
                }
                iprot.ReadListEnd();
              }
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.List) {
              {
                ExecTrades = new List<xueqiao.trade.hosting.HostingExecTrade>();
                TList _list73 = iprot.ReadListBegin();
                for( int _i74 = 0; _i74 < _list73.Count; ++_i74)
                {
                  xueqiao.trade.hosting.HostingExecTrade _elem75 = new xueqiao.trade.hosting.HostingExecTrade();
                  _elem75 = new xueqiao.trade.hosting.HostingExecTrade();
                  _elem75.Read(iprot);
                  ExecTrades.Add(_elem75);
                }
                iprot.ReadListEnd();
              }
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 5:
            if (field.Type == TType.Map) {
              {
                XqTradeRelatedItems = new Dictionary<long, List<HostingXQTradeRelatedItem>>();
                TMap _map76 = iprot.ReadMapBegin();
                for( int _i77 = 0; _i77 < _map76.Count; ++_i77)
                {
                  long _key78;
                  List<HostingXQTradeRelatedItem> _val79;
                  _key78 = iprot.ReadI64();
                  {
                    _val79 = new List<HostingXQTradeRelatedItem>();
                    TList _list80 = iprot.ReadListBegin();
                    for( int _i81 = 0; _i81 < _list80.Count; ++_i81)
                    {
                      HostingXQTradeRelatedItem _elem82 = new HostingXQTradeRelatedItem();
                      _elem82 = new HostingXQTradeRelatedItem();
                      _elem82.Read(iprot);
                      _val79.Add(_elem82);
                    }
                    iprot.ReadListEnd();
                  }
                  XqTradeRelatedItems[_key78] = _val79;
                }
                iprot.ReadMapEnd();
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
      TStruct struc = new TStruct("HostingXQOrderExecDetail");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (XqOrder != null && __isset.xqOrder) {
        field.Name = "xqOrder";
        field.Type = TType.Struct;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        XqOrder.Write(oprot);
        oprot.WriteFieldEnd();
      }
      if (XqTrades != null && __isset.xqTrades) {
        field.Name = "xqTrades";
        field.Type = TType.List;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteListBegin(new TList(TType.Struct, XqTrades.Count));
          foreach (HostingXQTrade _iter83 in XqTrades)
          {
            _iter83.Write(oprot);
          }
          oprot.WriteListEnd();
        }
        oprot.WriteFieldEnd();
      }
      if (ExecOrders != null && __isset.execOrders) {
        field.Name = "execOrders";
        field.Type = TType.List;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteListBegin(new TList(TType.Struct, ExecOrders.Count));
          foreach (xueqiao.trade.hosting.HostingExecOrder _iter84 in ExecOrders)
          {
            _iter84.Write(oprot);
          }
          oprot.WriteListEnd();
        }
        oprot.WriteFieldEnd();
      }
      if (ExecTrades != null && __isset.execTrades) {
        field.Name = "execTrades";
        field.Type = TType.List;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteListBegin(new TList(TType.Struct, ExecTrades.Count));
          foreach (xueqiao.trade.hosting.HostingExecTrade _iter85 in ExecTrades)
          {
            _iter85.Write(oprot);
          }
          oprot.WriteListEnd();
        }
        oprot.WriteFieldEnd();
      }
      if (XqTradeRelatedItems != null && __isset.xqTradeRelatedItems) {
        field.Name = "xqTradeRelatedItems";
        field.Type = TType.Map;
        field.ID = 5;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteMapBegin(new TMap(TType.I64, TType.List, XqTradeRelatedItems.Count));
          foreach (long _iter86 in XqTradeRelatedItems.Keys)
          {
            oprot.WriteI64(_iter86);
            {
              oprot.WriteListBegin(new TList(TType.Struct, XqTradeRelatedItems[_iter86].Count));
              foreach (HostingXQTradeRelatedItem _iter87 in XqTradeRelatedItems[_iter86])
              {
                _iter87.Write(oprot);
              }
              oprot.WriteListEnd();
            }
          }
          oprot.WriteMapEnd();
        }
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("HostingXQOrderExecDetail(");
      sb.Append("XqOrder: ");
      sb.Append(XqOrder== null ? "<null>" : XqOrder.ToString());
      sb.Append(",XqTrades: ");
      if (XqTrades == null)
      {
        sb.Append("<null>");
      }
      else
      {
        sb.Append("[");
        foreach (HostingXQTrade _iter88 in XqTrades)
        {
          sb.Append(_iter88.ToString());
          sb.Append(", ");
        }
        sb.Append("]");
      }
      sb.Append(",ExecOrders: ");
      if (ExecOrders == null)
      {
        sb.Append("<null>");
      }
      else
      {
        sb.Append("[");
        foreach (xueqiao.trade.hosting.HostingExecOrder _iter89 in ExecOrders)
        {
          sb.Append(_iter89.ToString());
          sb.Append(", ");
        }
        sb.Append("]");
      }
      sb.Append(",ExecTrades: ");
      if (ExecTrades == null)
      {
        sb.Append("<null>");
      }
      else
      {
        sb.Append("[");
        foreach (xueqiao.trade.hosting.HostingExecTrade _iter90 in ExecTrades)
        {
          sb.Append(_iter90.ToString());
          sb.Append(", ");
        }
        sb.Append("]");
      }
      sb.Append(",XqTradeRelatedItems: ");
      if (XqTradeRelatedItems == null)
      {
        sb.Append("<null>");
      }
      else
      {
        sb.Append("{");
        foreach (long _iter91 in XqTradeRelatedItems.Keys)
        {
          sb.Append(_iter91.ToString());
          sb.Append(":");
          sb.Append(XqTradeRelatedItems[_iter91].ToString());
          sb.Append(", ");
        }
        sb.Append("}");
      }
      sb.Append(")");
      return sb.ToString();
    }

  }

}