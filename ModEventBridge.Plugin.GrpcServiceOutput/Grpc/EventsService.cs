// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: events_service.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace ModEventBridge.Plugin.EventSource.Service {

  /// <summary>Holder for reflection information generated from events_service.proto</summary>
  public static partial class EventsServiceReflection {

    #region Descriptor
    /// <summary>File descriptor for events_service.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static EventsServiceReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChRldmVudHNfc2VydmljZS5wcm90bxIcbW9kZXZlbnRicmlkZ2UucGx1Z2lu",
            "LmV2ZW50cxofZ29vZ2xlL3Byb3RvYnVmL3RpbWVzdGFtcC5wcm90bxoMZXZl",
            "bnRzLnByb3RvIn4KDVN0cmVhbVJlcXVlc3QSGAoHdXNlcl9pZBgBIAEoCVIH",
            "dXNlcl9pZBJTCgxyZXF1ZXN0X3R5cGUYAiABKA4yLy5tb2RldmVudGJyaWRn",
            "ZS5wbHVnaW4uZXZlbnRzLlN0cmVhbVJlcXVlc3RUeXBlUgxyZXF1ZXN0X3R5",
            "cGUqYwoRU3RyZWFtUmVxdWVzdFR5cGUSHQoZVU5TRVRfU1RSRUFNX1JFUVVF",
            "U1RfVFlQRRAAEhcKE1NUQVJUX1NUUkVBTV9FVkVOVFMQARIWChJTVE9QX1NU",
            "UkVBTV9FVkVOVFMQAjJ2CgxCcmlkZ2VFdmVudHMSZgoMU3RyZWFtRXZlbnRz",
            "EisubW9kZXZlbnRicmlkZ2UucGx1Z2luLmV2ZW50cy5TdHJlYW1SZXF1ZXN0",
            "GiMubW9kZXZlbnRicmlkZ2UucGx1Z2luLmV2ZW50cy5FdmVudCIAKAEwAUIs",
            "qgIpTW9kRXZlbnRCcmlkZ2UuUGx1Z2luLkV2ZW50U291cmNlLlNlcnZpY2Vi",
            "BnByb3RvMw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Google.Protobuf.WellKnownTypes.TimestampReflection.Descriptor, global::ModEventBridge.Plugin.EventSource.EventsReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(new[] {typeof(global::ModEventBridge.Plugin.EventSource.Service.StreamRequestType), }, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::ModEventBridge.Plugin.EventSource.Service.StreamRequest), global::ModEventBridge.Plugin.EventSource.Service.StreamRequest.Parser, new[]{ "UserId", "RequestType" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Enums
  public enum StreamRequestType {
    [pbr::OriginalName("UNSET_STREAM_REQUEST_TYPE")] UnsetStreamRequestType = 0,
    [pbr::OriginalName("START_STREAM_EVENTS")] StartStreamEvents = 1,
    [pbr::OriginalName("STOP_STREAM_EVENTS")] StopStreamEvents = 2,
  }

  #endregion

  #region Messages
  public sealed partial class StreamRequest : pb::IMessage<StreamRequest> {
    private static readonly pb::MessageParser<StreamRequest> _parser = new pb::MessageParser<StreamRequest>(() => new StreamRequest());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<StreamRequest> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::ModEventBridge.Plugin.EventSource.Service.EventsServiceReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public StreamRequest() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public StreamRequest(StreamRequest other) : this() {
      userId_ = other.userId_;
      requestType_ = other.requestType_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public StreamRequest Clone() {
      return new StreamRequest(this);
    }

    /// <summary>Field number for the "user_id" field.</summary>
    public const int UserIdFieldNumber = 1;
    private string userId_ = "";
    /// <summary>
    /// UserID the user id of the user to being listening to events for
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string UserId {
      get { return userId_; }
      set {
        userId_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "request_type" field.</summary>
    public const int RequestTypeFieldNumber = 2;
    private global::ModEventBridge.Plugin.EventSource.Service.StreamRequestType requestType_ = 0;
    /// <summary>
    /// RequestType to start or stop streaming events for a user
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::ModEventBridge.Plugin.EventSource.Service.StreamRequestType RequestType {
      get { return requestType_; }
      set {
        requestType_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as StreamRequest);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(StreamRequest other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (UserId != other.UserId) return false;
      if (RequestType != other.RequestType) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (UserId.Length != 0) hash ^= UserId.GetHashCode();
      if (RequestType != 0) hash ^= RequestType.GetHashCode();
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
      if (UserId.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(UserId);
      }
      if (RequestType != 0) {
        output.WriteRawTag(16);
        output.WriteEnum((int) RequestType);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (UserId.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(UserId);
      }
      if (RequestType != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) RequestType);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(StreamRequest other) {
      if (other == null) {
        return;
      }
      if (other.UserId.Length != 0) {
        UserId = other.UserId;
      }
      if (other.RequestType != 0) {
        RequestType = other.RequestType;
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
          case 10: {
            UserId = input.ReadString();
            break;
          }
          case 16: {
            RequestType = (global::ModEventBridge.Plugin.EventSource.Service.StreamRequestType) input.ReadEnum();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
