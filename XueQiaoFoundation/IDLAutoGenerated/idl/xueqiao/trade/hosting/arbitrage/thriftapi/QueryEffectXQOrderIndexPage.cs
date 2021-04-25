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
  public partial class QueryEffectXQOrderIndexPage : TBase, INotifyPropertyChanged
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
    private int _totalNum;
    private List<HostingEffectXQOrderIndexItem> _resultIndexItems;

    public int TotalNum
    {
      get
      {
        return _totalNum;
      }
      set
      {
        __isset.totalNum = true;
        SetProperty(ref _totalNum, value);
      }
    }

    public List<HostingEffectXQOrderIndexItem> ResultIndexItems
    {
      get
      {
        return _resultIndexItems;
      }
      set
      {
        __isset.resultIndexItems = true;
        SetProperty(ref _resultIndexItems, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool totalNum;
      public bool resultIndexItems;
    }

    public QueryEffectXQOrderIndexPage() {
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
              TotalNum = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.List) {
              {
                ResultIndexItems = new List<HostingEffectXQOrderIndexItem>();
                TList _list62 = iprot.ReadListBegin();
                for( int _i63 = 0; _i63 < _list62.Count; ++_i63)
                {
                  HostingEffectXQOrderIndexItem _elem64 = new HostingEffectXQOrderIndexItem();
                  _elem64 = new HostingEffectXQOrderIndexItem();
                  _elem64.Read(iprot);
                  ResultIndexItems.Add(_elem64);
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
      TStruct struc = new TStruct("QueryEffectXQOrderIndexPage");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.totalNum) {
        field.Name = "totalNum";
        field.Type = TType.I32;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(TotalNum);
        oprot.WriteFieldEnd();
      }
      if (ResultIndexItems != null && __isset.resultIndexItems) {
        field.Name = "resultIndexItems";
        field.Type = TType.List;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteListBegin(new TList(TType.Struct, ResultIndexItems.Count));
          foreach (HostingEffectXQOrderIndexItem _iter65 in ResultIndexItems)
          {
            _iter65.Write(oprot);
          }
          oprot.WriteListEnd();
        }
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("QueryEffectXQOrderIndexPage(");
      sb.Append("TotalNum: ");
      sb.Append(TotalNum);
      sb.Append(",ResultIndexItems: ");
      if (ResultIndexItems == null)
      {
        sb.Append("<null>");
      }
      else
      {
        sb.Append("[");
        foreach (HostingEffectXQOrderIndexItem _iter66 in ResultIndexItems)
        {
          sb.Append(_iter66.ToString());
          sb.Append(", ");
        }
        sb.Append("]");
      }
      sb.Append(")");
      return sb.ToString();
    }

  }

}
