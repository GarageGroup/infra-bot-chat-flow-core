using System;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Infra.Bot.Builder;

public readonly partial struct ChatFlowBreakState : IEquatable<ChatFlowBreakState>
{
    private readonly string? userMessage, logMessage;

    public ChatFlowBreakState(
        [AllowNull] string userMessage = default,
        [AllowNull] string logMessage = default)
    {
        this.userMessage = NullIfEmpty(userMessage);
        this.logMessage = NullIfEmpty(logMessage);
    }

    public string UserMessage => userMessage ?? string.Empty;

    public string LogMessage => logMessage ?? string.Empty;

    private static string? NullIfEmpty(string? source)
        =>
        string.IsNullOrEmpty(source) ? default : source;
}