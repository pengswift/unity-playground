// automatically generated, do not modify

namespace Strawberry.Protocols
{

using System;
using FlatBuffers;

public sealed class SayHiResponse : Table {
  public static SayHiResponse GetRootAsSayHiResponse(ByteBuffer _bb) { return GetRootAsSayHiResponse(_bb, new SayHiResponse()); }
  public static SayHiResponse GetRootAsSayHiResponse(ByteBuffer _bb, SayHiResponse obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public SayHiResponse __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public string Msg { get { int o = __offset(4); return o != 0 ? __string(o + bb_pos) : null; } }
  public ArraySegment<byte>? GetMsgBytes() { return __vector_as_arraysegment(4); }

  public static Offset<SayHiResponse> CreateSayHiResponse(FlatBufferBuilder builder,
      StringOffset msgOffset = default(StringOffset)) {
    builder.StartObject(1);
    SayHiResponse.AddMsg(builder, msgOffset);
    return SayHiResponse.EndSayHiResponse(builder);
  }

  public static void StartSayHiResponse(FlatBufferBuilder builder) { builder.StartObject(1); }
  public static void AddMsg(FlatBufferBuilder builder, StringOffset msgOffset) { builder.AddOffset(0, msgOffset.Value, 0); }
  public static Offset<SayHiResponse> EndSayHiResponse(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<SayHiResponse>(o);
  }
};


}
