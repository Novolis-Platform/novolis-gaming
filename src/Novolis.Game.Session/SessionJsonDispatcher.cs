using System.Text.Json;

namespace Novolis.Game.Session;

/// <summary>Shared JSON method dispatch for stdio / TCP / HTTP hosts.</summary>
public static class SessionJsonDispatcher
{
    public static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        WriteIndented = false,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
    };

    public static object Dispatch(IGameSession session, string? method, JsonElement root)
    {
        ArgumentNullException.ThrowIfNull(session);
        return method switch
        {
            SessionMethodNames.Hello or "hello" => session.Hello(),
            SessionMethodNames.Snapshot or "snapshot" => session.Snapshot(),
            SessionMethodNames.Actions or "actions" => session.Actions(),
            SessionMethodNames.Continue or "continue" => session.Continue(),
            SessionMethodNames.Subscribe or "subscribe" => Subscribe(session),
            SessionMethodNames.Command or "command" => ExecuteCommand(session, root),
            _ => throw new InvalidOperationException($"Unknown method '{method}'"),
        };
    }

    public static SessionCommandDto ParseCommand(JsonElement root)
    {
        var cmd = new SessionCommandDto();
        JsonElement c = default;
        var hasCommand = root.ValueKind == JsonValueKind.Object
                         && (root.TryGetProperty("command", out c) || root.TryGetProperty("args", out c));

        var source = hasCommand ? c : root;
        if (source.ValueKind != JsonValueKind.Object)
            return cmd;

        if (TryString(source, "actionId", "ActionId", out var actionId))
            cmd.ActionId = actionId ?? "";
        if (TryString(source, "destSystemId", "DestSystemId", out var dest))
            cmd.DestSystemId = dest;
        if (TryString(source, "originSystemId", "OriginSystemId", out var origin))
            cmd.OriginSystemId = origin;
        if (TryString(source, "sku", "Sku", out var sku))
            cmd.Sku = sku;
        if (TryString(source, "profile", "Profile", out var profile))
            cmd.Profile = profile;
        if (TryString(source, "label", "Label", out var label))
            cmd.Label = label;
        if (TryInt(source, "index", "Index", out var index))
            cmd.Index = index;
        if (TryDecimal(source, "qty", "Qty", out var qty))
            cmd.Qty = qty;
        if (TryString(source, "attention", "Attention", out var attention))
            cmd.Attention = attention;
        if (TryDouble(source, "speed", "Speed", out var speed))
            cmd.Speed = speed;
        if (TryBool(source, "prepare", "Prepare", out var prepare))
            cmd.Prepare = prepare;
        return cmd;
    }

    private static SessionSubscribeResponseDto Subscribe(IGameSession session)
    {
        session.Subscribe();
        return new SessionSubscribeResponseDto { Ok = true };
    }

    private static SessionCommandResultDto ExecuteCommand(IGameSession session, JsonElement root) =>
        session.Execute(ParseCommand(root));

    private static bool TryString(JsonElement el, string a, string b, out string? value)
    {
        if (el.TryGetProperty(a, out var p) || el.TryGetProperty(b, out p))
        {
            value = p.GetString();
            return true;
        }

        value = null;
        return false;
    }

    private static bool TryInt(JsonElement el, string a, string b, out int value)
    {
        if ((el.TryGetProperty(a, out var p) || el.TryGetProperty(b, out p)) && p.TryGetInt32(out value))
            return true;
        value = 0;
        return false;
    }

    private static bool TryDecimal(JsonElement el, string a, string b, out decimal value)
    {
        if ((el.TryGetProperty(a, out var p) || el.TryGetProperty(b, out p)) && p.TryGetDecimal(out value))
            return true;
        value = 0;
        return false;
    }

    private static bool TryDouble(JsonElement el, string a, string b, out double value)
    {
        if ((el.TryGetProperty(a, out var p) || el.TryGetProperty(b, out p)) && p.TryGetDouble(out value))
            return true;
        value = 0;
        return false;
    }

    private static bool TryBool(JsonElement el, string a, string b, out bool value)
    {
        if ((el.TryGetProperty(a, out var p) || el.TryGetProperty(b, out p))
            && (p.ValueKind is JsonValueKind.True or JsonValueKind.False))
        {
            value = p.GetBoolean();
            return true;
        }

        value = false;
        return false;
    }
}
