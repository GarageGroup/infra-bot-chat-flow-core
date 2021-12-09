using System;
using System.Collections.Generic;

namespace GGroupp.Infra.Bot.Builder;

partial struct ChatFlowBreakState
{
    private static Type EqualityContract
        =>
        typeof(ChatFlowBreakState);

    private static StringComparer UIMessageComparer
        =>
        StringComparer.Ordinal;

    private static StringComparer LogMessageComparer
        =>
        StringComparer.Ordinal;

    private static IEqualityComparer<ChatFlowBreakType> TypeComparer
        =>
        EqualityComparer<ChatFlowBreakType>.Default;
}