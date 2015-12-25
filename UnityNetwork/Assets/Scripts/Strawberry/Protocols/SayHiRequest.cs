// automatically generated, do not modify

namespace Strawberry.Protocols
{

using System;
using FlatBuffers;

public sealed class SayHiRequest : Table {
  public static SayHiRequest GetRootAsSayHiRequest(ByteBuffer _bb) { return GetRootAsSayHiRequest(_bb, new SayHiRequest()); }
  public static SayHiRequest GetRootAsSayHiRequest(ByteBuffer _bb, SayHiRequest obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public SayHiRequest __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public string Msg { get { int o = __offset(4); return o != 0 ? __string(o + bb_pos) : null; } }
  public ArraySegment<byte>? GetMsgBytes() { return __vector_as_arraysegment(4); }

  public static Offset<SayHiRequest> CreateSayHiRequest(FlatBufferBuilder builder,
      StringOffset msgOffset = default(StringOffset)) {
    builder.StartObject(1);
    SayHiRequest.AddMsg(builder, msgOffset);
    return SayHiRequest.EndSayHiRequest(builder);
  }

  public static void StartSayHiRequest(FlatBufferBuilder builder) { builder.StartObject(1); }
  public static void AddMsg(FlatBufferBuilder builder, StringOffset msgOffset) { builder.AddOffset(0, msgOffset.Value, 0); }
  public static Offset<SayHiRequest> EndSayHiRequest(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<SayHiRequest>(o);
  }
};


}
