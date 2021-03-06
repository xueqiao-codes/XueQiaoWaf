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

namespace xueqiao.contract
{

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class ReqSyncMappingTaskOption : TBase, INotifyPropertyChanged
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
    private List<int> _taskIds;
    private List<int> _targetIds;
    private xueqiao.contract.standard.TechPlatformEnv _techPlatformEnv;
    private SyncTaskType _taskType;

    public List<int> TaskIds
    {
      get
      {
        return _taskIds;
      }
      set
      {
        __isset.taskIds = true;
        SetProperty(ref _taskIds, value);
      }
    }

    public List<int> TargetIds
    {
      get
      {
        return _targetIds;
      }
      set
      {
        __isset.targetIds = true;
        SetProperty(ref _targetIds, value);
      }
    }

    /// <summary>
    /// 
    /// <seealso cref="xueqiao.contract.standard.TechPlatformEnv"/>
    /// </summary>
    public xueqiao.contract.standard.TechPlatformEnv TechPlatformEnv
    {
      get
      {
        return _techPlatformEnv;
      }
      set
      {
        __isset.techPlatformEnv = true;
        SetProperty(ref _techPlatformEnv, value);
      }
    }

    /// <summary>
    /// 
    /// <seealso cref="SyncTaskType"/>
    /// </summary>
    public SyncTaskType TaskType
    {
      get
      {
        return _taskType;
      }
      set
      {
        __isset.taskType = true;
        SetProperty(ref _taskType, value);
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool taskIds;
      public bool targetIds;
      public bool techPlatformEnv;
      public bool taskType;
    }

    public ReqSyncMappingTaskOption() {
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
            if (field.Type == TType.List) {
              {
                TaskIds = new List<int>();
                TList _list30 = iprot.ReadListBegin();
                for( int _i31 = 0; _i31 < _list30.Count; ++_i31)
                {
                  int _elem32 = 0;
                  _elem32 = iprot.ReadI32();
                  TaskIds.Add(_elem32);
                }
                iprot.ReadListEnd();
              }
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.List) {
              {
                TargetIds = new List<int>();
                TList _list33 = iprot.ReadListBegin();
                for( int _i34 = 0; _i34 < _list33.Count; ++_i34)
                {
                  int _elem35 = 0;
                  _elem35 = iprot.ReadI32();
                  TargetIds.Add(_elem35);
                }
                iprot.ReadListEnd();
              }
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.I32) {
              TechPlatformEnv = (xueqiao.contract.standard.TechPlatformEnv)iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.I32) {
              TaskType = (SyncTaskType)iprot.ReadI32();
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
      TStruct struc = new TStruct("ReqSyncMappingTaskOption");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (TaskIds != null && __isset.taskIds) {
        field.Name = "taskIds";
        field.Type = TType.List;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteListBegin(new TList(TType.I32, TaskIds.Count));
          foreach (int _iter36 in TaskIds)
          {
            oprot.WriteI32(_iter36);
          }
          oprot.WriteListEnd();
        }
        oprot.WriteFieldEnd();
      }
      if (TargetIds != null && __isset.targetIds) {
        field.Name = "targetIds";
        field.Type = TType.List;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteListBegin(new TList(TType.I32, TargetIds.Count));
          foreach (int _iter37 in TargetIds)
          {
            oprot.WriteI32(_iter37);
          }
          oprot.WriteListEnd();
        }
        oprot.WriteFieldEnd();
      }
      if (__isset.techPlatformEnv) {
        field.Name = "techPlatformEnv";
        field.Type = TType.I32;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32((int)TechPlatformEnv);
        oprot.WriteFieldEnd();
      }
      if (__isset.taskType) {
        field.Name = "taskType";
        field.Type = TType.I32;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32((int)TaskType);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("ReqSyncMappingTaskOption(");
      sb.Append("TaskIds: ");
      if (TaskIds == null)
      {
        sb.Append("<null>");
      }
      else
      {
        sb.Append("[");
        foreach (int _iter38 in TaskIds)
        {
          sb.Append(_iter38.ToString());
          sb.Append(", ");
        }
        sb.Append("]");
      }
      sb.Append(",TargetIds: ");
      if (TargetIds == null)
      {
        sb.Append("<null>");
      }
      else
      {
        sb.Append("[");
        foreach (int _iter39 in TargetIds)
        {
          sb.Append(_iter39.ToString());
          sb.Append(", ");
        }
        sb.Append("]");
      }
      sb.Append(",TechPlatformEnv: ");
      sb.Append(TechPlatformEnv);
      sb.Append(",TaskType: ");
      sb.Append(TaskType);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
