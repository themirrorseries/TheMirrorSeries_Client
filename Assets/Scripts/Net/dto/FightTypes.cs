// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: proto/FightTypes.txt
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
/// <summary>Holder for reflection information generated from proto/FightTypes.txt</summary>
public static partial class FightTypesReflection {

  #region Descriptor
  /// <summary>File descriptor for proto/FightTypes.txt</summary>
  public static pbr::FileDescriptor Descriptor {
    get { return descriptor; }
  }
  private static pbr::FileDescriptor descriptor;

  static FightTypesReflection() {
    byte[] descriptorData = global::System.Convert.FromBase64String(
        string.Concat(
          "ChRwcm90by9GaWdodFR5cGVzLnR4dCo8CgpGaWdodFR5cGVzEg0KCU1PVkVf",
          "Q1JFURAAEg4KClNLSUxMX0NSRVEQARIPCgtJTkZPUk1fU1JFUxACYgZwcm90",
          "bzM="));
    descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
        new pbr::FileDescriptor[] { },
        new pbr::GeneratedClrTypeInfo(new[] {typeof(global::FightTypes), }, null, null));
  }
  #endregion

}
#region Enums
public enum FightTypes {
  [pbr::OriginalName("MOVE_CREQ")] MoveCreq = 0,
  [pbr::OriginalName("SKILL_CREQ")] SkillCreq = 1,
  [pbr::OriginalName("INFORM_SRES")] InformSres = 2,
}

#endregion


#endregion Designer generated code
