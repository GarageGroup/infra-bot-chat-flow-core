﻿using System;

namespace GGroupp.Infra.Bot.Builder;

public readonly partial struct ChatFlowJump<T> : IEquatable<ChatFlowJump<T>>
{
    private readonly T nextState;

    private readonly object? repeatState;

    private readonly ChatFlowBreakState breakState;

    public ChatFlowJumpTag Tag { get; }

    public ChatFlowJump(T nextState)
    {
        this.nextState = nextState;
        repeatState = default;
        breakState = default;
        Tag = ChatFlowJumpTag.Next;
    }

    public ChatFlowJump(object? repeatState)
    {
        nextState = default!;
        this.repeatState = repeatState;
        breakState = default;
        Tag = ChatFlowJumpTag.Repeat;
    }

    public ChatFlowJump(ChatFlowBreakState breakState)
    {
        nextState = default!;
        repeatState = default;
        this.breakState = breakState;
        Tag = ChatFlowJumpTag.Break;
    }
}