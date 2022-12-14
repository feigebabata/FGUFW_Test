// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Develop/FGUFW/ECS/Runtime/Protobuf/FGUFW.ECS.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace FGUFW.ECS {

  /// <summary>Holder for reflection information generated from Develop/FGUFW/ECS/Runtime/Protobuf/FGUFW.ECS.proto</summary>
  public static partial class FGUFWECSReflection {

    #region Descriptor
    /// <summary>File descriptor for Develop/FGUFW/ECS/Runtime/Protobuf/FGUFW.ECS.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static FGUFWECSReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "CjJEZXZlbG9wL0ZHVUZXL0VDUy9SdW50aW1lL1Byb3RvYnVmL0ZHVUZXLkVD",
            "Uy5wcm90bxIJRkdVRlcuRUNTIjsKCFBCX0ZyYW1lEg0KBUluZGV4GAEgASgF",
            "EhIKClBsYWNlSW5kZXgYAiABKAUSDAoEQ21kcxgDIAMoDWIGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::FGUFW.ECS.PB_Frame), global::FGUFW.ECS.PB_Frame.Parser, new[]{ "Index", "PlaceIndex", "Cmds" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class PB_Frame : pb::IMessage<PB_Frame> {
    private static readonly pb::MessageParser<PB_Frame> _parser = new pb::MessageParser<PB_Frame>(() => new PB_Frame());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<PB_Frame> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::FGUFW.ECS.FGUFWECSReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public PB_Frame() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public PB_Frame(PB_Frame other) : this() {
      index_ = other.index_;
      placeIndex_ = other.placeIndex_;
      cmds_ = other.cmds_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public PB_Frame Clone() {
      return new PB_Frame(this);
    }

    /// <summary>Field number for the "Index" field.</summary>
    public const int IndexFieldNumber = 1;
    private int index_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Index {
      get { return index_; }
      set {
        index_ = value;
      }
    }

    /// <summary>Field number for the "PlaceIndex" field.</summary>
    public const int PlaceIndexFieldNumber = 2;
    private int placeIndex_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int PlaceIndex {
      get { return placeIndex_; }
      set {
        placeIndex_ = value;
      }
    }

    /// <summary>Field number for the "Cmds" field.</summary>
    public const int CmdsFieldNumber = 3;
    private static readonly pb::FieldCodec<uint> _repeated_cmds_codec
        = pb::FieldCodec.ForUInt32(26);
    private readonly pbc::RepeatedField<uint> cmds_ = new pbc::RepeatedField<uint>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<uint> Cmds {
      get { return cmds_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as PB_Frame);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(PB_Frame other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Index != other.Index) return false;
      if (PlaceIndex != other.PlaceIndex) return false;
      if(!cmds_.Equals(other.cmds_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Index != 0) hash ^= Index.GetHashCode();
      if (PlaceIndex != 0) hash ^= PlaceIndex.GetHashCode();
      hash ^= cmds_.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Index != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(Index);
      }
      if (PlaceIndex != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(PlaceIndex);
      }
      cmds_.WriteTo(output, _repeated_cmds_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Index != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Index);
      }
      if (PlaceIndex != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(PlaceIndex);
      }
      size += cmds_.CalculateSize(_repeated_cmds_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(PB_Frame other) {
      if (other == null) {
        return;
      }
      if (other.Index != 0) {
        Index = other.Index;
      }
      if (other.PlaceIndex != 0) {
        PlaceIndex = other.PlaceIndex;
      }
      cmds_.Add(other.cmds_);
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            Index = input.ReadInt32();
            break;
          }
          case 16: {
            PlaceIndex = input.ReadInt32();
            break;
          }
          case 26:
          case 24: {
            cmds_.AddEntriesFrom(input, _repeated_cmds_codec);
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
