using System;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Infra.Bot.Builder;

public readonly partial struct ChatFlowBreakState : IEquatable<ChatFlowBreakState>
{
    private readonly string? uiMessage, logMessage;

    public ChatFlowBreakState(
        [AllowNull] string uiMessage = default,
        [AllowNull] string logMessage = default,
        ChatFlowBreakType type = default)
    {
        this.uiMessage = NullIfEmpty(uiMessage);
        this.logMessage = NullIfEmpty(logMessage);
        Type = type;
    }

    public ChatFlowBreakType Type { get; }

    public string UIMessage => uiMessage ?? string.Empty;

    public string LogMessage => logMessage ?? string.Empty;

    private static string? NullIfEmpty(string? source)
        =>
        string.IsNullOrEmpty(source) ? default : source;
}