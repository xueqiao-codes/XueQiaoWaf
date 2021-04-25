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

namespace xueqiao.trade.hosting
{

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class HostingExecOrderStateInfo : TBase, INotifyPropertyChanged
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
    private HostingExecOrderState _currentState;
    private List<HostingExecOrderState> _historyStates;
    private string _statusMsg;
    private int _failedErrorCode;
    private int _upsideErrorCode;
    private string _upsideUsefulMsg;

    public HostingExecOrderState CurrentState
    {
      get
      {
        return _currentState;
      }
      set
      {
        __isset.currentState = true;
        SetProperty(ref _currentState, value);
      }
    }

    public List<HostingExecOrderState> HistoryStates
    {
      get
      {
        return _historyStates;
      }
      set
      {
        __isset.historyStates = true;
        SetProperty(ref _historyStates, value);
      }
    }

    public string StatusMsg
    {
      get
      {
        return _statusMsg;
      }
      set
      {
        __isset.statusMsg = true;
        SetProperty(ref _statusMsg, value);
      }
    }

    public int FailedErrorCode
    {
      get
      {
        return _failedErrorCode;
      }
      set
      {
        __isset.failedErrorCode = true;
        SetProperty(ref _failedErrorCode, value);
      }
    }

    public int UpsideErrorCode
    {
      get
      {
        return _upsideErrorCode;
      }
      set
      {
        __isset.upsideErrorCode = true;
        SetProperty(ref _upsideErrorCode, value);
      }
    }

    public string UpsideUsefulMsg
    {
      get
      {
        return _upsideUsefulMsg;
      }
      set
      {
        __isset.upsideUsefulMsg = true;
        SetProperty(ref _upsideUsefulMsg, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool currentState;
      public bool historyStates;
      public bool statusMsg;
      public bool failedErrorCode;
      public bool upsideErrorCode;
      public bool upsideUsefulMsg;
    }

    public HostingExecOrderStateInfo() {
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
              CurrentState = new HostingExecOrderState();
              CurrentState.Read(iprot);
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.List) {
              {
                HistoryStates = new List<HostingExecOrderState>();
                TList _list40 = iprot.ReadListBegin();
                for( int _i41 = 0; _i41 < _list40.Count; ++_i41)
                {
                  HostingExecOrderState _elem42 = new HostingExecOrderState();
                  _elem42 = new HostingExecOrderState();
                  _elem42.Read(iprot);
                  HistoryStates.Add(_elem42);
                }
                iprot.ReadListEnd();
              }
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.String) {
              StatusMsg = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.I32) {
              FailedErrorCode = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 5:
            if (field.Type == TType.I32) {
              UpsideErrorCode = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 6:
            if (field.Type == TType.String) {
              UpsideUsefulMsg = iprot.ReadString();
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
      TStruct struc = new TStruct("HostingExecOrderStateInfo");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (CurrentState != null && __isset.currentState) {
        field.Name = "currentState";
        field.Type = TType.Struct;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        CurrentState.Write(oprot);
        oprot.WriteFieldEnd();
      }
      if (HistoryStates != null && __isset.historyStates) {
        field.Name = "historyStates";
        field.Type = TType.List;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteListBegin(new TList(TType.Struct, HistoryStates.Count));
          foreach (HostingExecOrderState _iter43 in HistoryStates)
          {
            _iter43.Write(oprot);
          }
          oprot.WriteListEnd();
        }
        oprot.WriteFieldEnd();
      }
      if (StatusMsg != null && __isset.statusMsg) {
        field.Name = "statusMsg";
        field.Type = TType.String;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(StatusMsg);
        oprot.WriteFieldEnd();
      }
      if (__isset.failedErrorCode) {
        field.Name = "failedErrorCode";
        field.Type = TType.I32;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(FailedErrorCode);
        oprot.WriteFieldEnd();
      }
      if (__isset.upsideErrorCode) {
        field.Name = "upsideErrorCode";
        field.Type = TType.I32;
        field.ID = 5;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(UpsideErrorCode);
        oprot.WriteFieldEnd();
      }
      if (UpsideUsefulMsg != null && __isset.upsideUsefulMsg) {
        field.Name = "upsideUsefulMsg";
        field.Type = TType.String;
        field.ID = 6;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(UpsideUsefulMsg);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("HostingExecOrderStateInfo(");
      sb.Append("CurrentState: ");
      sb.Append(CurrentState== null ? "<null>" : CurrentState.ToString());
      sb.Append(",HistoryStates: ");
      if (HistoryStates == null)
      {
        sb.Append("<null>");
      }
      else
      {
        sb.Append("[");
        foreach (HostingExecOrderState _iter44 in HistoryStates)
        {
          sb.Append(_iter44.ToString());
          sb.Append(", ");
        }
        sb.Append("]");
      }
      sb.Append(",StatusMsg: ");
      sb.Append(StatusMsg);
      sb.Append(",FailedErrorCode: ");
      sb.Append(FailedErrorCode);
      sb.Append(",UpsideErrorCode: ");
      sb.Append(UpsideErrorCode);
      sb.Append(",UpsideUsefulMsg: ");
      sb.Append(UpsideUsefulMsg);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
