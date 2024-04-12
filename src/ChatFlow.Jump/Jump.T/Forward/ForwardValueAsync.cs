using System;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Bot.Builder;

partial struct ChatFlowJump<T>
{
    public ValueTask<ChatFlowJump<T>> ForwardValueAsync(
        Func<T, ValueTask<ChatFlowJump<T>>> nextAsync)
        =>
        InnerForwardValueAsync(
            nextAsync ?? throw new ArgumentNullException(nameof(nextAsync)));

    private ValueTask<ChatFlowJump<T>> InnerForwardValueAsync(
        Func<T, ValueTask<ChatFlowJump<T>>> nextAsync)
        =>
        Tag switch
        {
            ChatFlowJumpTag.Next => nextAsync.Invoke(nextState),
            _ => new(this)
        };
}