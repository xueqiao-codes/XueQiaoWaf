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

namespace xueqiao.trade.hosting.position.fee.thriftapi
{

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class XQSpecCommissionSettingPage : TBase, INotifyPropertyChanged
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
    private List<XQSpecCommissionSettings> _XQSpecCommissionSettingList;

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

    public List<XQSpecCommissionSettings> XQSpecCommissionSettingList
    {
      get
      {
        return _XQSpecCommissionSettingList;
      }
      set
      {
        __isset.XQSpecCommissionSettingList = true;
        SetProperty(ref _XQSpecCommissionSettingList, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool totalNum;
      public bool XQSpecCommissionSettingList;
    }

    public XQSpecCommissionSettingPage() {
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
                XQSpecCommissionSettingList = new List<XQSpecCommissionSettings>();
                TList _list5 = iprot.ReadListBegin();
                for( int _i6 = 0; _i6 < _list5.Count; ++_i6)
                {
                  XQSpecCommissionSettings _elem7 = new XQSpecCommissionSettings();
                  _elem7 = new XQSpecCommissionSettings();
                  _elem7.Read(iprot);
                  XQSpecCommissionSettingList.Add(_elem7);
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
      TStruct struc = new TStruct("XQSpecCommissionSettingPage");
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
      if (XQSpecCommissionSettingList != null && __isset.XQSpecCommissionSettingList) {
        field.Name = "XQSpecCommissionSettingList";
        field.Type = TType.List;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteListBegin(new TList(TType.Struct, XQSpecCommissionSettingList.Count));
          foreach (XQSpecCommissionSettings _iter8 in XQSpecCommissionSettingList)
          {
            _iter8.Write(oprot);
          }
          oprot.WriteListEnd();
        }
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("XQSpecCommissionSettingPage(");
      sb.Append("TotalNum: ");
      sb.Append(TotalNum);
      sb.Append(",XQSpecCommissionSettingList: ");
      if (XQSpecCommissionSettingList == null)
      {
        sb.Append("<null>");
      }
      else
      {
        sb.Append("[");
        foreach (XQSpecCommissionSettings _iter9 in XQSpecCommissionSettingList)
        {
          sb.Append(_iter9.ToString());
          sb.Append(", ");
        }
        sb.Append("]");
      }
      sb.Append(")");
      return sb.ToString();
    }

  }

}
