using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace GarageGroup.Infra.Bot.Builder;

partial struct ChatFlowBreakState
{
    public override int GetHashCode()
    {
        var builder = new HashCode();

        builder.Add(EqualityContractHashCode());
        builder.Add(UserMessageComparer.GetHashCode(UserMessage));
        builder.Add(LogMessageComparer.GetHashCode(LogMessage));

        if (SourceException is not null)
        {
            builder.Add(SourceExceptionComparer.GetHashCode(SourceException));
        }

        return builder.ToHashCode();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int EqualityContractHashCode()
        =>
        EqualityComparer<Type>.Default.GetHashCode(typeof(ChatFlowBreakState));
}