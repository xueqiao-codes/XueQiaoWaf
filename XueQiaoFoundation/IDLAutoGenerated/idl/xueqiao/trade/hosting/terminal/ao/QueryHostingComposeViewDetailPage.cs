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
  public partial class QueryHostingComposeViewDetailPage : TBase, INotifyPropertyChanged
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
    private int _totalCount;
    private List<HostingComposeViewDetail> _resultList;

    public int TotalCount
    {
      get
      {
        return _totalCount;
      }
      set
      {
        __isset.totalCount = true;
        SetProperty(ref _totalCount, value);
      }
    }

    public List<HostingComposeViewDetail> ResultList
    {
      get
      {
        return _resultList;
      }
      set
      {
        __isset.resultList = true;
        SetProperty(ref _resultList, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool totalCount;
      public bool resultList;
    }

    public QueryHostingComposeViewDetailPage() {
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
              TotalCount = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.List) {
              {
                ResultList = new List<HostingComposeViewDetail>();
                TList _list0 = iprot.ReadListBegin();
                for( int _i1 = 0; _i1 < _list0.Count; ++_i1)
                {
                  HostingComposeViewDetail _elem2 = new HostingComposeViewDetail();
                  _elem2 = new HostingComposeViewDetail();
                  _elem2.Read(iprot);
                  ResultList.Add(_elem2);
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
      TStruct struc = new TStruct("QueryHostingComposeViewDetailPage");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.totalCount) {
        field.Name = "totalCount";
        field.Type = TType.I32;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(TotalCount);
        oprot.WriteFieldEnd();
      }
      if (ResultList != null && __isset.resultList) {
        field.Name = "resultList";
        field.Type = TType.List;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteListBegin(new TList(TType.Struct, ResultList.Count));
          foreach (HostingComposeViewDetail _iter3 in ResultList)
          {
            _iter3.Write(oprot);
          }
          oprot.WriteListEnd();
        }
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("QueryHostingComposeViewDetailPage(");
      sb.Append("TotalCount: ");
      sb.Append(TotalCount);
      sb.Append(",ResultList: ");
      if (ResultList == null)
      {
        sb.Append("<null>");
      }
      else
      {
        sb.Append("[");
        foreach (HostingComposeViewDetail _iter4 in ResultList)
        {
          sb.Append(_iter4.ToString());
          sb.Append(", ");
        }
        sb.Append("]");
      }
      sb.Append(")");
      return sb.ToString();
    }

  }

}
