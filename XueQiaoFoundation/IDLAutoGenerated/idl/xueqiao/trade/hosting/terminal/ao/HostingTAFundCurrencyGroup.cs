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

namespace xueqiao.trade.hosting.terminal.ao
{

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class HostingTAFundCurrencyGroup : TBase, INotifyPropertyChanged
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
    private string _currencyNo;
    private List<xueqiao.trade.hosting.tradeaccount.data.TradeAccountFund> _itemFunds;
    private xueqiao.trade.hosting.tradeaccount.data.TradeAccountFund _groupTotalFund;

    public string CurrencyNo
    {
      get
      {
        return _currencyNo;
      }
      set
      {
        __isset.currencyNo = true;
        SetProperty(ref _currencyNo, value);
      }
    }

    public List<xueqiao.trade.hosting.tradeaccount.data.TradeAccountFund> ItemFunds
    {
      get
      {
        return _itemFunds;
      }
      set
      {
        __isset.itemFunds = true;
        SetProperty(ref _itemFunds, value);
      }
    }

    public xueqiao.trade.hosting.tradeaccount.data.TradeAccountFund GroupTotalFund
    {
      get
      {
        return _groupTotalFund;
      }
      set
      {
        __isset.groupTotalFund = true;
        SetProperty(ref _groupTotalFund, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool currencyNo;
      public bool itemFunds;
      public bool groupTotalFund;
    }

    public HostingTAFundCurrencyGroup() {
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
            if (field.Type == TType.String) {
              CurrencyNo = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.List) {
              {
                ItemFunds = new List<xueqiao.trade.hosting.tradeaccount.data.TradeAccountFund>();
                TList _list50 = iprot.ReadListBegin();
                for( int _i51 = 0; _i51 < _list50.Count; ++_i51)
                {
                  xueqiao.trade.hosting.tradeaccount.data.TradeAccountFund _elem52 = new xueqiao.trade.hosting.tradeaccount.data.TradeAccountFund();
                  _elem52 = new xueqiao.trade.hosting.tradeaccount.data.TradeAccountFund();
                  _elem52.Read(iprot);
                  ItemFunds.Add(_elem52);
                }
                iprot.ReadListEnd();
              }
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.Struct) {
              GroupTotalFund = new xueqiao.trade.hosting.tradeaccount.data.TradeAccountFund();
              GroupTotalFund.Read(iprot);
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
      TStruct struc = new TStruct("HostingTAFundCurrencyGroup");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (CurrencyNo != null && __isset.currencyNo) {
        field.Name = "currencyNo";
        field.Type = TType.String;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(CurrencyNo);
        oprot.WriteFieldEnd();
      }
      if (ItemFunds != null && __isset.itemFunds) {
        field.Name = "itemFunds";
        field.Type = TType.List;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteListBegin(new TList(TType.Struct, ItemFunds.Count));
          foreach (xueqiao.trade.hosting.tradeaccount.data.TradeAccountFund _iter53 in ItemFunds)
          {
            _iter53.Write(oprot);
          }
          oprot.WriteListEnd();
        }
        oprot.WriteFieldEnd();
      }
      if (GroupTotalFund != null && __isset.groupTotalFund) {
        field.Name = "groupTotalFund";
        field.Type = TType.Struct;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        GroupTotalFund.Write(oprot);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("HostingTAFundCurrencyGroup(");
      sb.Append("CurrencyNo: ");
      sb.Append(CurrencyNo);
      sb.Append(",ItemFunds: ");
      if (ItemFunds == null)
      {
        sb.Append("<null>");
      }
      else
      {
        sb.Append("[");
        foreach (xueqiao.trade.hosting.tradeaccount.data.TradeAccountFund _iter54 in ItemFunds)
        {
          sb.Append(_iter54.ToString());
          sb.Append(", ");
        }
        sb.Append("]");
      }
      sb.Append(",GroupTotalFund: ");
      sb.Append(GroupTotalFund== null ? "<null>" : GroupTotalFund.ToString());
      sb.Append(")");
      return sb.ToString();
    }

  }

}
