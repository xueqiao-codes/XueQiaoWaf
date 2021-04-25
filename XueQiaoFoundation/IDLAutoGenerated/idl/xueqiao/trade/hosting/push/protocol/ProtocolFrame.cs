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
  /// 协议帧的数据结构
  /// </summary>
  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class ProtocolFrame : TBase, INotifyPropertyChanged
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
    private UserMsgContent _userMsg;
    private ReqContent _reqContent;
    private RespContent _respContent;
    private xueqiao.quotation.QuotationItem _quotationItem;
    private SeqMsgContent _seqMsg;

    /// <summary>
    /// 
    /// <seealso cref="ProtocolType"/>
    /// </summary>
    public ProtocolType Protocol { get; set; }

    public UserMsgContent UserMsg
    {
      get
      {
        return _userMsg;
      }
      set
      {
        __isset.userMsg = true;
        SetProperty(ref _userMsg, value);
      }
    }

    public ReqContent ReqContent
    {
      get
      {
        return _reqContent;
      }
      set
      {
        __isset.reqContent = true;
        SetProperty(ref _reqContent, value);
      }
    }

    public RespContent RespContent
    {
      get
      {
        return _respContent;
      }
      set
      {
        __isset.respContent = true;
        SetProperty(ref _respContent, value);
      }
    }

    public xueqiao.quotation.QuotationItem QuotationItem
    {
      get
      {
        return _quotationItem;
      }
      set
      {
        __isset.quotationItem = true;
        SetProperty(ref _quotationItem, value);
      }
    }

    public SeqMsgContent SeqMsg
    {
      get
      {
        return _seqMsg;
      }
      set
      {
        __isset.seqMsg = true;
        SetProperty(ref _seqMsg, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool userMsg;
      public bool reqContent;
      public bool respContent;
      public bool quotationItem;
      public bool seqMsg;
    }

    public ProtocolFrame() {
    }

    public ProtocolFrame(ProtocolType protocol) : this() {
      this.Protocol = protocol;
    }

    public void Read (TProtocol iprot)
    {
      bool isset_protocol = false;
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
              Protocol = (ProtocolType)iprot.ReadI32();
              isset_protocol = true;
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.Struct) {
              UserMsg = new UserMsgContent();
              UserMsg.Read(iprot);
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.Struct) {
              ReqContent = new ReqContent();
              ReqContent.Read(iprot);
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.Struct) {
              RespContent = new RespContent();
              RespContent.Read(iprot);
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 5:
            if (field.Type == TType.Struct) {
              QuotationItem = new xueqiao.quotation.QuotationItem();
              QuotationItem.Read(iprot);
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 6:
            if (field.Type == TType.Struct) {
              SeqMsg = new SeqMsgContent();
              SeqMsg.Read(iprot);
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
      if (!isset_protocol)
        throw new TProtocolException(TProtocolException.INVALID_DATA);
    }

    public void Write(TProtocol oprot) {
      TStruct struc = new TStruct("ProtocolFrame");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      field.Name = "protocol";
      field.Type = TType.I32;
      field.ID = 1;
      oprot.WriteFieldBegin(field);
      oprot.WriteI32((int)Protocol);
      oprot.WriteFieldEnd();
      if (UserMsg != null && __isset.userMsg) {
        field.Name = "userMsg";
        field.Type = TType.Struct;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        UserMsg.Write(oprot);
        oprot.WriteFieldEnd();
      }
      if (ReqContent != null && __isset.reqContent) {
        field.Name = "reqContent";
        field.Type = TType.Struct;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        ReqContent.Write(oprot);
        oprot.WriteFieldEnd();
      }
      if (RespContent != null && __isset.respContent) {
        field.Name = "respContent";
        field.Type = TType.Struct;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        RespContent.Write(oprot);
        oprot.WriteFieldEnd();
      }
      if (QuotationItem != null && __isset.quotationItem) {
        field.Name = "quotationItem";
        field.Type = TType.Struct;
        field.ID = 5;
        oprot.WriteFieldBegin(field);
        QuotationItem.Write(oprot);
        oprot.WriteFieldEnd();
      }
      if (SeqMsg != null && __isset.seqMsg) {
        field.Name = "seqMsg";
        field.Type = TType.Struct;
        field.ID = 6;
        oprot.WriteFieldBegin(field);
        SeqMsg.Write(oprot);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("ProtocolFrame(");
      sb.Append("Protocol: ");
      sb.Append(Protocol);
      sb.Append(",UserMsg: ");
      sb.Append(UserMsg== null ? "<null>" : UserMsg.ToString());
      sb.Append(",ReqContent: ");
      sb.Append(ReqContent== null ? "<null>" : ReqContent.ToString());
      sb.Append(",RespContent: ");
      sb.Append(RespContent== null ? "<null>" : RespContent.ToString());
      sb.Append(",QuotationItem: ");
      sb.Append(QuotationItem== null ? "<null>" : QuotationItem.ToString());
      sb.Append(",SeqMsg: ");
      sb.Append(SeqMsg== null ? "<null>" : SeqMsg.ToString());
      sb.Append(")");
      return sb.ToString();
    }

  }

}