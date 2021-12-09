using System;

namespace GGroupp.Infra.Bot.Builder;

partial struct ChatFlowJump<T>
{
    public ChatFlowJump<TNextState> Forward<TNextState>(
        Func<T, ChatFlowJump<TNextState>> next)
        =>
        InnerForward(
            next ?? throw new ArgumentNullException(nameof(next)));

    private ChatFlowJump<TNextState> InnerForward<TNextState>(
        Func<T, ChatFlowJump<TNextState>> next)
        =>
        Tag switch
        {
            ChatFlowJumpTag.Next => next.Invoke(nextState),
            ChatFlowJumpTag.Repeat => new(repeatState),
            _ => new(breakState)
        };
}