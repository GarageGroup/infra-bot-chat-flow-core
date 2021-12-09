using System;
using System.Collections.Generic;

namespace GGroupp.Infra.Bot.Builder;

partial struct ChatFlowJump<T>
{
    private static Type EqualityContract
        =>
        typeof(ChatFlowJump<T>);

    private static IEqualityComparer<T> NextStateComparer
        =>
        EqualityComparer<T>.Default;

    private static IEqualityComparer<object?> RepeatStateComparer
        =>
        EqualityComparer<object?>.Default;

    private static IEqualityComparer<ChatFlowBreakState> BreakStateComparer
        =>
        EqualityComparer<ChatFlowBreakState>.Default;

    private static IEqualityComparer<ChatFlowJumpTag> TagComparer
        =>
        EqualityComparer<ChatFlowJumpTag>.Default;
}