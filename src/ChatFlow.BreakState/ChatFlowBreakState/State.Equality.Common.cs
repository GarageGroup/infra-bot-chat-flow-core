using System;
using System.Collections.Generic;

namespace GarageGroup.Infra.Bot.Builder;

partial struct ChatFlowBreakState
{
    private static StringComparer UserMessageComparer
        =>
        StringComparer.Ordinal;

    private static StringComparer LogMessageComparer
        =>
        StringComparer.Ordinal;

    private static ReferenceEqualityComparer SourceExceptionComparer
        =>
        ReferenceEqualityComparer.Instance;
}