# Novolis.Game.Session

Player **decision-point session** protocol: snapshot, typed commands, push events.

## Install

```bash
dotnet add package Novolis.Game.Session
```

## Transports (same `IGameSession`)

| Transport | Enable | Default |
|-----------|--------|---------|
| **HTTP REST + SSE** | on when the server calls `SessionSurface.AttachAll` (default in Sins) | `http://127.0.0.1:18765` |
| **LocalIpc** MessagePack | on when the server calls `SessionSurface.AttachAll` (default in Sins) | pipe `novolis-game-session-sins` |
| **TCP JSONL** | on when the server calls `SessionSurface.AttachAll` (default in Sins) | `127.0.0.1:18766` |
| **Stdio JSONL** | `SessionStdioHost` | in-process |
| **In-process** | bind `IGameSession` | UI / CLI |

```csharp
// Server-side (Sins does this by default): start all "hooks" so you can takeover mid-game.
var surface = SessionSurface.AttachAll(session, preferredPipeName: SessionEndpoints.SinsPipeName);
// surface.HttpBaseUrl → curl/agents
```

### HTTP routes

| Method | Path |
|--------|------|
| GET | `/health`, `/session/hello`, `/session/snapshot`, `/session/actions` |
| POST | `/session/command`, `/session/continue`, `/session/subscribe`, `/session/rpc` |
| GET SSE | `/session/events` |

Markers: `%TEMP%/novolis-game-session.http`, `.host`, `.tcp`.

## Wire methods (v1)

`session.hello` · `snapshot` · `actions` · `command` · `continue` · `subscribe`  
events: `decision` · `changed` · `actionResult`

## Docs

- [Session protocol](https://github.com/Novolis-Platform/novolis-gaming/blob/main/docs/session-protocol.md)
