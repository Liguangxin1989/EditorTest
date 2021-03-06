/**
 * Autogenerated by Thrift Compiler (0.10.0)
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
using Thrift.Protocol;
using Thrift.Transport;

namespace BubbleCouple
{

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class SceneTransform : TBase
  {
    private Vector2D _translation;
    private double _rotation;

    public Vector2D Translation
    {
      get
      {
        return _translation;
      }
      set
      {
        __isset.translation = true;
        this._translation = value;
      }
    }

    public double Rotation
    {
      get
      {
        return _rotation;
      }
      set
      {
        __isset.rotation = true;
        this._rotation = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool translation;
      public bool rotation;
    }

    public SceneTransform() {
    }

    public void Read (TProtocol iprot)
    {
      iprot.IncrementRecursionDepth();
      try
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
            case 10:
              if (field.Type == TType.Struct) {
                Translation = new Vector2D();
                Translation.Read(iprot);
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 20:
              if (field.Type == TType.Double) {
                Rotation = iprot.ReadDouble();
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
      finally
      {
        iprot.DecrementRecursionDepth();
      }
    }

    public void Write(TProtocol oprot) {
      oprot.IncrementRecursionDepth();
      try
      {
        TStruct struc = new TStruct("SceneTransform");
        oprot.WriteStructBegin(struc);
        TField field = new TField();
        if (Translation != null && __isset.translation) {
          field.Name = "translation";
          field.Type = TType.Struct;
          field.ID = 10;
          oprot.WriteFieldBegin(field);
          Translation.Write(oprot);
          oprot.WriteFieldEnd();
        }
        if (__isset.rotation) {
          field.Name = "rotation";
          field.Type = TType.Double;
          field.ID = 20;
          oprot.WriteFieldBegin(field);
          oprot.WriteDouble(Rotation);
          oprot.WriteFieldEnd();
        }
        oprot.WriteFieldStop();
        oprot.WriteStructEnd();
      }
      finally
      {
        oprot.DecrementRecursionDepth();
      }
    }

    public override string ToString() {
      StringBuilder __sb = new StringBuilder("SceneTransform(");
      bool __first = true;
      if (Translation != null && __isset.translation) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Translation: ");
        __sb.Append(Translation== null ? "<null>" : Translation.ToString());
      }
      if (__isset.rotation) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Rotation: ");
        __sb.Append(Rotation);
      }
      __sb.Append(")");
      return __sb.ToString();
    }

  }

}
