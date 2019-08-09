// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: MatchSuccessDTO.txt
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
/// <summary>Holder for reflection information generated from MatchSuccessDTO.txt</summary>
public static partial class MatchSuccessDTOReflection {

  #region Descriptor
  /// <summary>File descriptor for MatchSuccessDTO.txt</summary>
  public static pbr::FileDescriptor Descriptor {
    get { return descriptor; }
  }
  private static pbr::FileDescriptor descriptor;

  static MatchSuccessDTOReflection() {
    byte[] descriptorData = global::System.Convert.FromBase64String(
        string.Concat(
          "ChNNYXRjaFN1Y2Nlc3NEVE8udHh0Im8KD01hdGNoU3VjY2Vzc0RUTxIOCgZy",
          "b29taWQYASABKAUSCQoBeBgCIAEoAhIJCgF6GAMgASgCEg0KBXNwZWVkGAQg",
          "ASgCEg0KBWNvdW50GAUgASgFEhgKB3BsYXllcnMYBiADKAsyBy5QbGF5ZXIi",
          "RgoGUGxheWVyEhAKCHBsYXllcmlkGAEgASgFEgwKBG5hbWUYAiABKAkSDgoG",
          "cm9sZWlkGAMgASgFEgwKBHNlYXQYBCABKAViBnByb3RvMw=="));
    descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
        new pbr::FileDescriptor[] { },
        new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
          new pbr::GeneratedClrTypeInfo(typeof(global::MatchSuccessDTO), global::MatchSuccessDTO.Parser, new[]{ "Roomid", "X", "Z", "Speed", "Count", "Players" }, null, null, null, null),
          new pbr::GeneratedClrTypeInfo(typeof(global::Player), global::Player.Parser, new[]{ "Playerid", "Name", "Roleid", "Seat" }, null, null, null, null)
        }));
  }
  #endregion

}
#region Messages
public sealed partial class MatchSuccessDTO : pb::IMessage<MatchSuccessDTO> {
  private static readonly pb::MessageParser<MatchSuccessDTO> _parser = new pb::MessageParser<MatchSuccessDTO>(() => new MatchSuccessDTO());
  private pb::UnknownFieldSet _unknownFields;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public static pb::MessageParser<MatchSuccessDTO> Parser { get { return _parser; } }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public static pbr::MessageDescriptor Descriptor {
    get { return global::MatchSuccessDTOReflection.Descriptor.MessageTypes[0]; }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  pbr::MessageDescriptor pb::IMessage.Descriptor {
    get { return Descriptor; }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public MatchSuccessDTO() {
    OnConstruction();
  }

  partial void OnConstruction();

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public MatchSuccessDTO(MatchSuccessDTO other) : this() {
    roomid_ = other.roomid_;
    x_ = other.x_;
    z_ = other.z_;
    speed_ = other.speed_;
    count_ = other.count_;
    players_ = other.players_.Clone();
    _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public MatchSuccessDTO Clone() {
    return new MatchSuccessDTO(this);
  }

  /// <summary>Field number for the "roomid" field.</summary>
  public const int RoomidFieldNumber = 1;
  private int roomid_;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public int Roomid {
    get { return roomid_; }
    set {
      roomid_ = value;
    }
  }

  /// <summary>Field number for the "x" field.</summary>
  public const int XFieldNumber = 2;
  private float x_;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public float X {
    get { return x_; }
    set {
      x_ = value;
    }
  }

  /// <summary>Field number for the "z" field.</summary>
  public const int ZFieldNumber = 3;
  private float z_;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public float Z {
    get { return z_; }
    set {
      z_ = value;
    }
  }

  /// <summary>Field number for the "speed" field.</summary>
  public const int SpeedFieldNumber = 4;
  private float speed_;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public float Speed {
    get { return speed_; }
    set {
      speed_ = value;
    }
  }

  /// <summary>Field number for the "count" field.</summary>
  public const int CountFieldNumber = 5;
  private int count_;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public int Count {
    get { return count_; }
    set {
      count_ = value;
    }
  }

  /// <summary>Field number for the "players" field.</summary>
  public const int PlayersFieldNumber = 6;
  private static readonly pb::FieldCodec<global::Player> _repeated_players_codec
      = pb::FieldCodec.ForMessage(50, global::Player.Parser);
  private readonly pbc::RepeatedField<global::Player> players_ = new pbc::RepeatedField<global::Player>();
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public pbc::RepeatedField<global::Player> Players {
    get { return players_; }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public override bool Equals(object other) {
    return Equals(other as MatchSuccessDTO);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public bool Equals(MatchSuccessDTO other) {
    if (ReferenceEquals(other, null)) {
      return false;
    }
    if (ReferenceEquals(other, this)) {
      return true;
    }
    if (Roomid != other.Roomid) return false;
    if (!pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(X, other.X)) return false;
    if (!pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(Z, other.Z)) return false;
    if (!pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(Speed, other.Speed)) return false;
    if (Count != other.Count) return false;
    if(!players_.Equals(other.players_)) return false;
    return Equals(_unknownFields, other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public override int GetHashCode() {
    int hash = 1;
    if (Roomid != 0) hash ^= Roomid.GetHashCode();
    if (X != 0F) hash ^= pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(X);
    if (Z != 0F) hash ^= pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(Z);
    if (Speed != 0F) hash ^= pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(Speed);
    if (Count != 0) hash ^= Count.GetHashCode();
    hash ^= players_.GetHashCode();
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
    if (Roomid != 0) {
      output.WriteRawTag(8);
      output.WriteInt32(Roomid);
    }
    if (X != 0F) {
      output.WriteRawTag(21);
      output.WriteFloat(X);
    }
    if (Z != 0F) {
      output.WriteRawTag(29);
      output.WriteFloat(Z);
    }
    if (Speed != 0F) {
      output.WriteRawTag(37);
      output.WriteFloat(Speed);
    }
    if (Count != 0) {
      output.WriteRawTag(40);
      output.WriteInt32(Count);
    }
    players_.WriteTo(output, _repeated_players_codec);
    if (_unknownFields != null) {
      _unknownFields.WriteTo(output);
    }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public int CalculateSize() {
    int size = 0;
    if (Roomid != 0) {
      size += 1 + pb::CodedOutputStream.ComputeInt32Size(Roomid);
    }
    if (X != 0F) {
      size += 1 + 4;
    }
    if (Z != 0F) {
      size += 1 + 4;
    }
    if (Speed != 0F) {
      size += 1 + 4;
    }
    if (Count != 0) {
      size += 1 + pb::CodedOutputStream.ComputeInt32Size(Count);
    }
    size += players_.CalculateSize(_repeated_players_codec);
    if (_unknownFields != null) {
      size += _unknownFields.CalculateSize();
    }
    return size;
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public void MergeFrom(MatchSuccessDTO other) {
    if (other == null) {
      return;
    }
    if (other.Roomid != 0) {
      Roomid = other.Roomid;
    }
    if (other.X != 0F) {
      X = other.X;
    }
    if (other.Z != 0F) {
      Z = other.Z;
    }
    if (other.Speed != 0F) {
      Speed = other.Speed;
    }
    if (other.Count != 0) {
      Count = other.Count;
    }
    players_.Add(other.players_);
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
          Roomid = input.ReadInt32();
          break;
        }
        case 21: {
          X = input.ReadFloat();
          break;
        }
        case 29: {
          Z = input.ReadFloat();
          break;
        }
        case 37: {
          Speed = input.ReadFloat();
          break;
        }
        case 40: {
          Count = input.ReadInt32();
          break;
        }
        case 50: {
          players_.AddEntriesFrom(input, _repeated_players_codec);
          break;
        }
      }
    }
  }

}

public sealed partial class Player : pb::IMessage<Player> {
  private static readonly pb::MessageParser<Player> _parser = new pb::MessageParser<Player>(() => new Player());
  private pb::UnknownFieldSet _unknownFields;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public static pb::MessageParser<Player> Parser { get { return _parser; } }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public static pbr::MessageDescriptor Descriptor {
    get { return global::MatchSuccessDTOReflection.Descriptor.MessageTypes[1]; }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  pbr::MessageDescriptor pb::IMessage.Descriptor {
    get { return Descriptor; }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public Player() {
    OnConstruction();
  }

  partial void OnConstruction();

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public Player(Player other) : this() {
    playerid_ = other.playerid_;
    name_ = other.name_;
    roleid_ = other.roleid_;
    seat_ = other.seat_;
    _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public Player Clone() {
    return new Player(this);
  }

  /// <summary>Field number for the "playerid" field.</summary>
  public const int PlayeridFieldNumber = 1;
  private int playerid_;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public int Playerid {
    get { return playerid_; }
    set {
      playerid_ = value;
    }
  }

  /// <summary>Field number for the "name" field.</summary>
  public const int NameFieldNumber = 2;
  private string name_ = "";
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public string Name {
    get { return name_; }
    set {
      name_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
    }
  }

  /// <summary>Field number for the "roleid" field.</summary>
  public const int RoleidFieldNumber = 3;
  private int roleid_;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public int Roleid {
    get { return roleid_; }
    set {
      roleid_ = value;
    }
  }

  /// <summary>Field number for the "seat" field.</summary>
  public const int SeatFieldNumber = 4;
  private int seat_;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public int Seat {
    get { return seat_; }
    set {
      seat_ = value;
    }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public override bool Equals(object other) {
    return Equals(other as Player);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public bool Equals(Player other) {
    if (ReferenceEquals(other, null)) {
      return false;
    }
    if (ReferenceEquals(other, this)) {
      return true;
    }
    if (Playerid != other.Playerid) return false;
    if (Name != other.Name) return false;
    if (Roleid != other.Roleid) return false;
    if (Seat != other.Seat) return false;
    return Equals(_unknownFields, other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public override int GetHashCode() {
    int hash = 1;
    if (Playerid != 0) hash ^= Playerid.GetHashCode();
    if (Name.Length != 0) hash ^= Name.GetHashCode();
    if (Roleid != 0) hash ^= Roleid.GetHashCode();
    if (Seat != 0) hash ^= Seat.GetHashCode();
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
    if (Playerid != 0) {
      output.WriteRawTag(8);
      output.WriteInt32(Playerid);
    }
    if (Name.Length != 0) {
      output.WriteRawTag(18);
      output.WriteString(Name);
    }
    if (Roleid != 0) {
      output.WriteRawTag(24);
      output.WriteInt32(Roleid);
    }
    if (Seat != 0) {
      output.WriteRawTag(32);
      output.WriteInt32(Seat);
    }
    if (_unknownFields != null) {
      _unknownFields.WriteTo(output);
    }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public int CalculateSize() {
    int size = 0;
    if (Playerid != 0) {
      size += 1 + pb::CodedOutputStream.ComputeInt32Size(Playerid);
    }
    if (Name.Length != 0) {
      size += 1 + pb::CodedOutputStream.ComputeStringSize(Name);
    }
    if (Roleid != 0) {
      size += 1 + pb::CodedOutputStream.ComputeInt32Size(Roleid);
    }
    if (Seat != 0) {
      size += 1 + pb::CodedOutputStream.ComputeInt32Size(Seat);
    }
    if (_unknownFields != null) {
      size += _unknownFields.CalculateSize();
    }
    return size;
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public void MergeFrom(Player other) {
    if (other == null) {
      return;
    }
    if (other.Playerid != 0) {
      Playerid = other.Playerid;
    }
    if (other.Name.Length != 0) {
      Name = other.Name;
    }
    if (other.Roleid != 0) {
      Roleid = other.Roleid;
    }
    if (other.Seat != 0) {
      Seat = other.Seat;
    }
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
          Playerid = input.ReadInt32();
          break;
        }
        case 18: {
          Name = input.ReadString();
          break;
        }
        case 24: {
          Roleid = input.ReadInt32();
          break;
        }
        case 32: {
          Seat = input.ReadInt32();
          break;
        }
      }
    }
  }

}

#endregion


#endregion Designer generated code
