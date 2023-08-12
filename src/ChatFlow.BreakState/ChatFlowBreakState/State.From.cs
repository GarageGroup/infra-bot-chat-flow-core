using System;
using System.Diagnostics.CodeAnalysis;

namespace GarageGroup.Infra.Bot.Builder;

public readonly partial struct ChatFlowBreakState
{
    public static ChatFlowBreakState From([AllowNull] string userMessage, [AllowNull] string logMessage)
        =>
        new(
            userMessage: userMessage,
            logMessage: logMessage);

    public static ChatFlowBreakState From([AllowNull] string userMessage)
        =>
        new(
            userMessage: userMessage,
            logMessage: default);

    public static ChatFlowBreakState From([AllowNull] string userMessage, [AllowNull] string logMessage, Exception? sourceException)
        =>
        new(userMessage, logMessage)
        {
            SourceException = sourceException
        };
}