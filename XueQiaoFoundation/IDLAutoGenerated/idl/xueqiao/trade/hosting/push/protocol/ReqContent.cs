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

namespace xueqiao.trade.hosting.push.protocol
{

  /// <summary>
  /// 客户端发送请求的内容
  /// </summary>
  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class ReqContent : TBase, INotifyPropertyChanged
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
    private byte[] _requestStructBytes;

    public long RequestId { get; set; }

    public string RequestStructType { get; set; }

    public byte[] RequestStructBytes
    {
      get
      {
        return _requestStructBytes;
      }
      set
      {
        __isset.requestStructBytes = true;
        SetProperty(ref _requestStructBytes, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool requestStructBytes;
    }

    public ReqContent() {
    }

    public ReqContent(long requestId, string requestStructType) : this() {
      this.RequestId = requestId;
      this.RequestStructType = requestStructType;
    }

    public void Read (TProtocol iprot)
    {
      bool isset_requestId = false;
      bool isset_requestStructType = false;
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
              RequestId = iprot.ReadI64();
              isset_requestId = true;
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.String) {
              RequestStructType = iprot.ReadString();
              isset_requestStructType = true;
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.String) {
              RequestStructBytes = iprot.ReadBinary();
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
      if (!isset_requestId)
        throw new TProtocolException(TProtocolException.INVALID_DATA);
      if (!isset_requestStructType)
        throw new TProtocolException(TProtocolException.INVALID_DATA);
    }

    public void Write(TProtocol oprot) {
      TStruct struc = new TStruct("ReqContent");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      field.Name = "requestId";
      field.Type = TType.I64;
      field.ID = 1;
      oprot.WriteFieldBegin(field);
      oprot.WriteI64(RequestId);
      oprot.WriteFieldEnd();
      field.Name = "requestStructType";
      field.Type = TType.String;
      field.ID = 2;
      oprot.WriteFieldBegin(field);
      oprot.WriteString(RequestStructType);
      oprot.WriteFieldEnd();
      if (RequestStructBytes != null && __isset.requestStructBytes) {
        field.Name = "requestStructBytes";
        field.Type = TType.String;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteBinary(RequestStructBytes);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("ReqContent(");
      sb.Append("RequestId: ");
      sb.Append(RequestId);
      sb.Append(",RequestStructType: ");
      sb.Append(RequestStructType);
      sb.Append(",RequestStructBytes: ");
      sb.Append(RequestStructBytes);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
