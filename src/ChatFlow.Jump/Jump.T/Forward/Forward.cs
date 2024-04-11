using System;

namespace GarageGroup.Infra.Bot.Builder;

partial struct ChatFlowJump<T>
{
    public ChatFlowJump<T> Forward(
        Func<T, ChatFlowJump<T>> next)
        =>
        InnerForward(
            next ?? throw new ArgumentNullException(nameof(next)));

    private ChatFlowJump<T> InnerForward(
        Func<T, ChatFlowJump<T>> next)
        =>
        Tag switch
        {
            ChatFlowJumpTag.Next => next.Invoke(nextState),
            _ => this
        };
}