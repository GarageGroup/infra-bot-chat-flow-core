using System;

namespace GarageGroup.Infra.Bot.Builder;

partial struct ChatFlowBreakState
{
    private static Type EqualityContract
        =>
        typeof(ChatFlowBreakState);

    private static StringComparer UserMessageComparer
        =>
        StringComparer.Ordinal;

    private static StringComparer LogMessageComparer
        =>
        StringComparer.Ordinal;
}