using System;
using System.Diagnostics.CodeAnalysis;

namespace GarageGroup.Infra.Bot.Builder;

partial class ChatFlowBreakStateExtensions
{
    public static ChatFlowBreakState ToChatFlowBreakState(
        [AllowNull] this Exception sourceException, [AllowNull] string userMessage, [AllowNull] string logMessage)
        =>
        new(userMessage, logMessage)
        {
            SourceException = sourceException
        };
}