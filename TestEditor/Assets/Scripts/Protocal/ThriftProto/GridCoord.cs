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
  public partial class GridCoord : TBase
  {
    private int _col;
    private int _row;

    public int Col
    {
      get
      {
        return _col;
      }
      set
      {
        __isset.col = true;
        this._col = value;
      }
    }

    public int Row
    {
      get
      {
        return _row;
      }
      set
      {
        __isset.row = true;
        this._row = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool col;
      public bool row;
    }

    public GridCoord() {
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
              if (field.Type == TType.I32) {
                Col = iprot.ReadI32();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 20:
              if (field.Type == TType.I32) {
                Row = iprot.ReadI32();
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
        TStruct struc = new TStruct("GridCoord");
        oprot.WriteStructBegin(struc);
        TField field = new TField();
        if (__isset.col) {
          field.Name = "col";
          field.Type = TType.I32;
          field.ID = 10;
          oprot.WriteFieldBegin(field);
          oprot.WriteI32(Col);
          oprot.WriteFieldEnd();
        }
        if (__isset.row) {
          field.Name = "row";
          field.Type = TType.I32;
          field.ID = 20;
          oprot.WriteFieldBegin(field);
          oprot.WriteI32(Row);
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
      StringBuilder __sb = new StringBuilder("GridCoord(");
      bool __first = true;
      if (__isset.col) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Col: ");
        __sb.Append(Col);
      }
      if (__isset.row) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Row: ");
        __sb.Append(Row);
      }
      __sb.Append(")");
      return __sb.ToString();
    }

  }

}
