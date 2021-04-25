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
  /// 推送用户消息的内容
  /// </summary>
  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class UserMsgContent : TBase, INotifyPropertyChanged
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
    private byte[] _msgBytes;

    public string MsgType { get; set; }

    public byte[] MsgBytes
    {
      get
      {
        return _msgBytes;
      }
      set
      {
        __isset.msgBytes = true;
        SetProperty(ref _msgBytes, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool msgBytes;
    }

    public UserMsgContent() {
    }

    public UserMsgContent(string msgType) : this() {
      this.MsgType = msgType;
    }

    public void Read (TProtocol iprot)
    {
      bool isset_msgType = false;
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
              MsgType = iprot.ReadString();
              isset_msgType = true;
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.String) {
              MsgBytes = iprot.ReadBinary();
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
      if (!isset_msgType)
        throw new TProtocolException(TProtocolException.INVALID_DATA);
    }

    public void Write(TProtocol oprot) {
      TStruct struc = new TStruct("UserMsgContent");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      field.Name = "msgType";
      field.Type = TType.String;
      field.ID = 1;
      oprot.WriteFieldBegin(field);
      oprot.WriteString(MsgType);
      oprot.WriteFieldEnd();
      if (MsgBytes != null && __isset.msgBytes) {
        field.Name = "msgBytes";
        field.Type = TType.String;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteBinary(MsgBytes);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("UserMsgContent(");
      sb.Append("MsgType: ");
      sb.Append(MsgType);
      sb.Append(",MsgBytes: ");
      sb.Append(MsgBytes);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
