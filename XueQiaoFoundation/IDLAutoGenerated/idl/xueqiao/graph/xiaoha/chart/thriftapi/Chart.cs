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

namespace xueqiao.graph.xiaoha.chart.thriftapi
{

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class Chart : TBase, INotifyPropertyChanged
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
    private long _chartId;
    private string _xiaohaObjId;
    private string _chartName;
    private string _shareKey;
    private string _url;
    private long _parentFolderId;
    private THashSet<string> _tags;
    private long _createTimestamp;
    private long _lastModifyTimestamp;
    private ChartType _chartType;
    private ChartState _chartState;
    private string _commodityName;
    private string _plate;

    public long ChartId
    {
      get
      {
        return _chartId;
      }
      set
      {
        __isset.chartId = true;
        SetProperty(ref _chartId, value);
      }
    }

    public string XiaohaObjId
    {
      get
      {
        return _xiaohaObjId;
      }
      set
      {
        __isset.xiaohaObjId = true;
        SetProperty(ref _xiaohaObjId, value);
      }
    }

    public string ChartName
    {
      get
      {
        return _chartName;
      }
      set
      {
        __isset.chartName = true;
        SetProperty(ref _chartName, value);
      }
    }

    public string ShareKey
    {
      get
      {
        return _shareKey;
      }
      set
      {
        __isset.shareKey = true;
        SetProperty(ref _shareKey, value);
      }
    }

    public string Url
    {
      get
      {
        return _url;
      }
      set
      {
        __isset.url = true;
        SetProperty(ref _url, value);
      }
    }

    public long ParentFolderId
    {
      get
      {
        return _parentFolderId;
      }
      set
      {
        __isset.parentFolderId = true;
        SetProperty(ref _parentFolderId, value);
      }
    }

    public THashSet<string> Tags
    {
      get
      {
        return _tags;
      }
      set
      {
        __isset.tags = true;
        SetProperty(ref _tags, value);
      }
    }

    public long CreateTimestamp
    {
      get
      {
        return _createTimestamp;
      }
      set
      {
        __isset.createTimestamp = true;
        SetProperty(ref _createTimestamp, value);
      }
    }

    public long LastModifyTimestamp
    {
      get
      {
        return _lastModifyTimestamp;
      }
      set
      {
        __isset.lastModifyTimestamp = true;
        SetProperty(ref _lastModifyTimestamp, value);
      }
    }

    /// <summary>
    /// 
    /// <seealso cref="ChartType"/>
    /// </summary>
    public ChartType ChartType
    {
      get
      {
        return _chartType;
      }
      set
      {
        __isset.chartType = true;
        SetProperty(ref _chartType, value);
      }
    }

    /// <summary>
    /// 
    /// <seealso cref="ChartState"/>
    /// </summary>
    public ChartState ChartState
    {
      get
      {
        return _chartState;
      }
      set
      {
        __isset.chartState = true;
        SetProperty(ref _chartState, value);
      }
    }

    public string CommodityName
    {
      get
      {
        return _commodityName;
      }
      set
      {
        __isset.commodityName = true;
        SetProperty(ref _commodityName, value);
      }
    }

    public string Plate
    {
      get
      {
        return _plate;
      }
      set
      {
        __isset.plate = true;
        SetProperty(ref _plate, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool chartId;
      public bool xiaohaObjId;
      public bool chartName;
      public bool shareKey;
      public bool url;
      public bool parentFolderId;
      public bool tags;
      public bool createTimestamp;
      public bool lastModifyTimestamp;
      public bool chartType;
      public bool chartState;
      public bool commodityName;
      public bool plate;
    }

    public Chart() {
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
            if (field.Type == TType.I64) {
              ChartId = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.String) {
              XiaohaObjId = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.String) {
              ChartName = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.String) {
              ShareKey = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 5:
            if (field.Type == TType.String) {
              Url = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 6:
            if (field.Type == TType.I64) {
              ParentFolderId = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 7:
            if (field.Type == TType.Set) {
              {
                Tags = new THashSet<string>();
                TSet _set0 = iprot.ReadSetBegin();
                for( int _i1 = 0; _i1 < _set0.Count; ++_i1)
                {
                  string _elem2 = null;
                  _elem2 = iprot.ReadString();
                  Tags.Add(_elem2);
                }
                iprot.ReadSetEnd();
              }
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 8:
            if (field.Type == TType.I64) {
              CreateTimestamp = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 9:
            if (field.Type == TType.I64) {
              LastModifyTimestamp = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 10:
            if (field.Type == TType.I32) {
              ChartType = (ChartType)iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 11:
            if (field.Type == TType.I32) {
              ChartState = (ChartState)iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 12:
            if (field.Type == TType.String) {
              CommodityName = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 13:
            if (field.Type == TType.String) {
              Plate = iprot.ReadString();
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
      TStruct struc = new TStruct("Chart");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.chartId) {
        field.Name = "chartId";
        field.Type = TType.I64;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(ChartId);
        oprot.WriteFieldEnd();
      }
      if (XiaohaObjId != null && __isset.xiaohaObjId) {
        field.Name = "xiaohaObjId";
        field.Type = TType.String;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(XiaohaObjId);
        oprot.WriteFieldEnd();
      }
      if (ChartName != null && __isset.chartName) {
        field.Name = "chartName";
        field.Type = TType.String;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(ChartName);
        oprot.WriteFieldEnd();
      }
      if (ShareKey != null && __isset.shareKey) {
        field.Name = "shareKey";
        field.Type = TType.String;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(ShareKey);
        oprot.WriteFieldEnd();
      }
      if (Url != null && __isset.url) {
        field.Name = "url";
        field.Type = TType.String;
        field.ID = 5;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Url);
        oprot.WriteFieldEnd();
      }
      if (__isset.parentFolderId) {
        field.Name = "parentFolderId";
        field.Type = TType.I64;
        field.ID = 6;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(ParentFolderId);
        oprot.WriteFieldEnd();
      }
      if (Tags != null && __isset.tags) {
        field.Name = "tags";
        field.Type = TType.Set;
        field.ID = 7;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteSetBegin(new TSet(TType.String, Tags.Count));
          foreach (string _iter3 in Tags)
          {
            oprot.WriteString(_iter3);
          }
          oprot.WriteSetEnd();
        }
        oprot.WriteFieldEnd();
      }
      if (__isset.createTimestamp) {
        field.Name = "createTimestamp";
        field.Type = TType.I64;
        field.ID = 8;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(CreateTimestamp);
        oprot.WriteFieldEnd();
      }
      if (__isset.lastModifyTimestamp) {
        field.Name = "lastModifyTimestamp";
        field.Type = TType.I64;
        field.ID = 9;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(LastModifyTimestamp);
        oprot.WriteFieldEnd();
      }
      if (__isset.chartType) {
        field.Name = "chartType";
        field.Type = TType.I32;
        field.ID = 10;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32((int)ChartType);
        oprot.WriteFieldEnd();
      }
      if (__isset.chartState) {
        field.Name = "chartState";
        field.Type = TType.I32;
        field.ID = 11;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32((int)ChartState);
        oprot.WriteFieldEnd();
      }
      if (CommodityName != null && __isset.commodityName) {
        field.Name = "commodityName";
        field.Type = TType.String;
        field.ID = 12;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(CommodityName);
        oprot.WriteFieldEnd();
      }
      if (Plate != null && __isset.plate) {
        field.Name = "plate";
        field.Type = TType.String;
        field.ID = 13;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Plate);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("Chart(");
      sb.Append("ChartId: ");
      sb.Append(ChartId);
      sb.Append(",XiaohaObjId: ");
      sb.Append(XiaohaObjId);
      sb.Append(",ChartName: ");
      sb.Append(ChartName);
      sb.Append(",ShareKey: ");
      sb.Append(ShareKey);
      sb.Append(",Url: ");
      sb.Append(Url);
      sb.Append(",ParentFolderId: ");
      sb.Append(ParentFolderId);
      sb.Append(",Tags: ");
      if (Tags == null)
      {
        sb.Append("<null>");
      }
      else
      {
        sb.Append("[");
        foreach (string _iter4 in Tags)
        {
          sb.Append(_iter4.ToString());
          sb.Append(", ");
        }
        sb.Append("]");
      }
      sb.Append(",CreateTimestamp: ");
      sb.Append(CreateTimestamp);
      sb.Append(",LastModifyTimestamp: ");
      sb.Append(LastModifyTimestamp);
      sb.Append(",ChartType: ");
      sb.Append(ChartType);
      sb.Append(",ChartState: ");
      sb.Append(ChartState);
      sb.Append(",CommodityName: ");
      sb.Append(CommodityName);
      sb.Append(",Plate: ");
      sb.Append(Plate);
      sb.Append(")");
      return sb.ToString();
    }

  }

}