using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Infra.Bot.Builder;

public readonly partial struct ChatFlowBreakState
{
    public static ChatFlowBreakState From([AllowNull] string uiMessage, [AllowNull] string logMessage, ChatFlowBreakType type)
        =>
        new(
            uiMessage: uiMessage,
            logMessage: logMessage,
            type: type);

    public static ChatFlowBreakState From([AllowNull] string uiMessage, [AllowNull] string logMessage)
        =>
        new(
            uiMessage: uiMessage,
            logMessage: logMessage,
            type: default);
}