using MessagePack;

namespace Novolis.Game.Session;

[MessagePackObject]
public sealed class SessionHelloRequestDto
{
    [Key(0)] public long Sequence { get; set; }
}

[MessagePackObject]
public sealed class SessionHelloResponseDto
{
    [Key(0)] public long Sequence { get; set; }
    [Key(1)] public string ProtocolVersion { get; set; } = "1.0";
    [Key(2)] public string AppId { get; set; } = "";
    [Key(3)] public string AppTitle { get; set; } = "";
    [Key(4)] public int ProcessId { get; set; }
    [Key(5)] public string[] Capabilities { get; set; } = [];
}

[MessagePackObject]
public sealed class SessionSnapshotRequestDto
{
    [Key(0)] public long Sequence { get; set; }
}

[MessagePackObject]
public sealed class SessionActionsRequestDto
{
    [Key(0)] public long Sequence { get; set; }
}

[MessagePackObject]
public sealed class SessionSubscribeRequestDto
{
    [Key(0)] public long Sequence { get; set; }
}

[MessagePackObject]
public sealed class SessionSubscribeResponseDto
{
    [Key(0)] public long Sequence { get; set; }
    [Key(1)] public bool Ok { get; set; } = true;
}

[MessagePackObject]
public sealed class SessionContinueRequestDto
{
    [Key(0)] public long Sequence { get; set; }
}

[MessagePackObject]
public sealed class SessionCommandRequestDto
{
    [Key(0)] public long Sequence { get; set; }
    [Key(1)] public SessionCommandDto Command { get; set; } = new();
}

[MessagePackObject]
public sealed class SessionCommandDto
{
    [Key(0)] public string ActionId { get; set; } = "";
    [Key(1)] public string? DestSystemId { get; set; }
    [Key(2)] public int? Index { get; set; }
    [Key(3)] public string? Sku { get; set; }
    [Key(4)] public decimal? Qty { get; set; }
    [Key(5)] public string? OriginSystemId { get; set; }
    [Key(6)] public string? Profile { get; set; }
    [Key(7)] public string? Label { get; set; }
    /// <summary>Decision attention: runAlways | softSlow | hardPause (setClock).</summary>
    [Key(8)] public string? Attention { get; set; }
    /// <summary>Sim speed 0..1 (setClock).</summary>
    [Key(9)] public double? Speed { get; set; }
    /// <summary>When true with depart/premium, queue prepare-and-depart compound.</summary>
    [Key(10)] public bool? Prepare { get; set; }
}

[MessagePackObject]
public sealed class SessionCommandResultDto
{
    [Key(0)] public long Sequence { get; set; }
    [Key(1)] public bool Ok { get; set; }
    [Key(2)] public string ActionId { get; set; } = "";
    [Key(3)] public string Message { get; set; } = "";
    [Key(4)] public string? ErrorCode { get; set; }
    [Key(5)] public SessionSnapshotDto? Snapshot { get; set; }
}

[MessagePackObject]
public sealed class SessionActionDto
{
    [Key(0)] public string Id { get; set; } = "";
    [Key(1)] public string Label { get; set; } = "";
    [Key(2)] public bool Enabled { get; set; }
    [Key(3)] public string? DisabledReason { get; set; }
}

[MessagePackObject]
public sealed class SessionActionsResponseDto
{
    [Key(0)] public long Sequence { get; set; }
    [Key(1)] public SessionActionDto[] Actions { get; set; } = [];
}

[MessagePackObject]
public sealed class SessionLastActionDto
{
    [Key(0)] public string ActionId { get; set; } = "";
    [Key(1)] public bool Ok { get; set; }
    [Key(2)] public string Message { get; set; } = "";
    [Key(3)] public string? ErrorCode { get; set; }
}

[MessagePackObject]
public sealed class SessionBoardItemDto
{
    [Key(0)] public int Index { get; set; }
    [Key(1)] public string Id { get; set; } = "";
    [Key(2)] public string Label { get; set; } = "";
    [Key(3)] public string Detail { get; set; } = "";
    [Key(4)] public bool CanAct { get; set; }
}

[MessagePackObject]
public sealed class SessionSnapshotDto
{
    [Key(0)] public long Sequence { get; set; }
    [Key(1)] public int Day { get; set; }
    [Key(2)] public string SeedHash { get; set; } = "";
    [Key(3)] public string HubId { get; set; } = "";
    [Key(4)] public string HubName { get; set; } = "";
    [Key(5)] public string PauseReason { get; set; } = "Running";
    [Key(6)] public string VoyageLine { get; set; } = "";
    [Key(7)] public string HullLine { get; set; } = "";
    [Key(8)] public string CashLine { get; set; } = "";
    [Key(9)] public string StandingLine { get; set; } = "";
    [Key(10)] public string DecisionLine { get; set; } = "";
    [Key(11)] public string CoachLine { get; set; } = "";
    [Key(12)] public string SoftFailLine { get; set; } = "";
    [Key(13)] public string SurvivalLine { get; set; } = "";
    [Key(14)] public string MeshLine { get; set; } = "";
    [Key(15)] public string HoldLine { get; set; } = "";
    [Key(16)] public bool Underway { get; set; }
    [Key(17)] public bool DockedIdle { get; set; }
    [Key(18)] public bool Complete { get; set; }
    [Key(19)] public bool SoftFail { get; set; }
    [Key(20)] public bool StandbyOffer { get; set; }
    [Key(21)] public string? TravelTargetSystemId { get; set; }
    [Key(22)] public string[] RouteSystemIds { get; set; } = [];
    [Key(23)] public SessionBoardItemDto[] SpotFreight { get; set; } = [];
    [Key(24)] public SessionBoardItemDto[] GoodsCharters { get; set; } = [];
    [Key(25)] public SessionBoardItemDto[] MarketLots { get; set; } = [];
    [Key(26)] public string[] Manifest { get; set; } = [];
    [Key(27)] public SessionActionDto[] Actions { get; set; } = [];
    [Key(28)] public SessionLastActionDto? LastAction { get; set; }
    [Key(29)] public string Attention { get; set; } = "runAlways";
    [Key(30)] public double SimSpeedScale { get; set; } = 1.0;
    [Key(31)] public string[] IntentStack { get; set; } = [];
    [Key(32)] public double ShipMapX { get; set; }
    [Key(33)] public double ShipMapY { get; set; }
    [Key(34)] public bool ShipMapVisible { get; set; }
    /// <summary>Recent EMA: game hours simulated per real minute.</summary>
    [Key(35)] public double GameHoursPerRealMinute { get; set; }
    /// <summary>Session average game hours per real minute.</summary>
    [Key(36)] public double SessionGameHoursPerRealMinute { get; set; }
    /// <summary>Human-readable pace, e.g. ≈ 48 game h / real min.</summary>
    [Key(37)] public string PaceLine { get; set; } = "";
}

[MessagePackObject]
public sealed class SessionDecisionEventDto
{
    [Key(0)] public long Sequence { get; set; }
    [Key(1)] public int Day { get; set; }
    [Key(2)] public string HubId { get; set; } = "";
    [Key(3)] public string DecisionLine { get; set; } = "";
    [Key(4)] public SessionSnapshotDto? Snapshot { get; set; }
}

[MessagePackObject]
public sealed class SessionChangedEventDto
{
    [Key(0)] public long Sequence { get; set; }
    [Key(1)] public string Reason { get; set; } = "";
    [Key(2)] public SessionSnapshotDto? Snapshot { get; set; }
}

[MessagePackObject]
public sealed class SessionActionResultEventDto
{
    [Key(0)] public long Sequence { get; set; }
    [Key(1)] public string ActionId { get; set; } = "";
    [Key(2)] public bool Ok { get; set; }
    [Key(3)] public string Message { get; set; } = "";
    [Key(4)] public string? ErrorCode { get; set; }
    [Key(5)] public SessionSnapshotDto? Snapshot { get; set; }
}

[MessagePackObject]
public sealed class SessionFaultDto
{
    [Key(0)] public long Sequence { get; set; }
    [Key(1)] public string Message { get; set; } = "";
}
