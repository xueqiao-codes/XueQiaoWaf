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
  public partial class ReqChartOption : TBase, INotifyPropertyChanged
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
    private THashSet<long> _chartIds;
    private string _xiaohaObjId;
    private long _parentFolderId;
    private ChartType _chartType;
    private string _commodityName;
    private string _plate;
    private ChartState _state;
    private string _name;
    private THashSet<string> _keyWords;

    public THashSet<long> ChartIds
    {
      get
      {
        return _chartIds;
      }
      set
      {
        __isset.chartIds = true;
        SetProperty(ref _chartIds, value);
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

    /// <summary>
    /// 
    /// <seealso cref="ChartState"/>
    /// </summary>
    public ChartState State
    {
      get
      {
        return _state;
      }
      set
      {
        __isset.state = true;
        SetProperty(ref _state, value);
      }
    }

    public string Name
    {
      get
      {
        return _name;
      }
      set
      {
        __isset.name = true;
        SetProperty(ref _name, value);
      }
    }

    public THashSet<string> KeyWords
    {
      get
      {
        return _keyWords;
      }
      set
      {
        __isset.keyWords = true;
        SetProperty(ref _keyWords, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool chartIds;
      public bool xiaohaObjId;
      public bool parentFolderId;
      public bool chartType;
      public bool commodityName;
      public bool plate;
      public bool state;
      public bool name;
      public bool keyWords;
    }

    public ReqChartOption() {
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
            if (field.Type == TType.Set) {
              {
                ChartIds = new THashSet<long>();
                TSet _set10 = iprot.ReadSetBegin();
                for( int _i11 = 0; _i11 < _set10.Count; ++_i11)
                {
                  long _elem12 = 0;
                  _elem12 = iprot.ReadI64();
                  ChartIds.Add(_elem12);
                }
                iprot.ReadSetEnd();
              }
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
            if (field.Type == TType.I64) {
              ParentFolderId = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.I32) {
              ChartType = (ChartType)iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 5:
            if (field.Type == TType.String) {
              CommodityName = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 6:
            if (field.Type == TType.String) {
              Plate = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 9:
            if (field.Type == TType.I32) {
              State = (ChartState)iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 7:
            if (field.Type == TType.String) {
              Name = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 8:
            if (field.Type == TType.Set) {
              {
                KeyWords = new THashSet<string>();
                TSet _set13 = iprot.ReadSetBegin();
                for( int _i14 = 0; _i14 < _set13.Count; ++_i14)
                {
                  string _elem15 = null;
                  _elem15 = iprot.ReadString();
                  KeyWords.Add(_elem15);
                }
                iprot.ReadSetEnd();
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
      TStruct struc = new TStruct("ReqChartOption");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (ChartIds != null && __isset.chartIds) {
        field.Name = "chartIds";
        field.Type = TType.Set;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteSetBegin(new TSet(TType.I64, ChartIds.Count));
          foreach (long _iter16 in ChartIds)
          {
            oprot.WriteI64(_iter16);
          }
          oprot.WriteSetEnd();
        }
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
      if (__isset.parentFolderId) {
        field.Name = "parentFolderId";
        field.Type = TType.I64;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(ParentFolderId);
        oprot.WriteFieldEnd();
      }
      if (__isset.chartType) {
        field.Name = "chartType";
        field.Type = TType.I32;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32((int)ChartType);
        oprot.WriteFieldEnd();
      }
      if (CommodityName != null && __isset.commodityName) {
        field.Name = "commodityName";
        field.Type = TType.String;
        field.ID = 5;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(CommodityName);
        oprot.WriteFieldEnd();
      }
      if (Plate != null && __isset.plate) {
        field.Name = "plate";
        field.Type = TType.String;
        field.ID = 6;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Plate);
        oprot.WriteFieldEnd();
      }
      if (Name != null && __isset.name) {
        field.Name = "name";
        field.Type = TType.String;
        field.ID = 7;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Name);
        oprot.WriteFieldEnd();
      }
      if (KeyWords != null && __isset.keyWords) {
        field.Name = "keyWords";
        field.Type = TType.Set;
        field.ID = 8;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteSetBegin(new TSet(TType.String, KeyWords.Count));
          foreach (string _iter17 in KeyWords)
          {
            oprot.WriteString(_iter17);
          }
          oprot.WriteSetEnd();
        }
        oprot.WriteFieldEnd();
      }
      if (__isset.state) {
        field.Name = "state";
        field.Type = TType.I32;
        field.ID = 9;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32((int)State);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("ReqChartOption(");
      sb.Append("ChartIds: ");
      if (ChartIds == null)
      {
        sb.Append("<null>");
      }
      else
      {
        sb.Append("[");
        foreach (long _iter18 in ChartIds)
        {
          sb.Append(_iter18.ToString());
          sb.Append(", ");
        }
        sb.Append("]");
      }
      sb.Append(",XiaohaObjId: ");
      sb.Append(XiaohaObjId);
      sb.Append(",ParentFolderId: ");
      sb.Append(ParentFolderId);
      sb.Append(",ChartType: ");
      sb.Append(ChartType);
      sb.Append(",CommodityName: ");
      sb.Append(CommodityName);
      sb.Append(",Plate: ");
      sb.Append(Plate);
      sb.Append(",State: ");
      sb.Append(State);
      sb.Append(",Name: ");
      sb.Append(Name);
      sb.Append(",KeyWords: ");
      if (KeyWords == null)
      {
        sb.Append("<null>");
      }
      else
      {
        sb.Append("[");
        foreach (string _iter19 in KeyWords)
        {
          sb.Append(_iter19.ToString());
          sb.Append(", ");
        }
        sb.Append("]");
      }
      sb.Append(")");
      return sb.ToString();
    }

  }

}