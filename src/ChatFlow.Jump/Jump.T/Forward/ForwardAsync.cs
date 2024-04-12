using System;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Bot.Builder;

partial struct ChatFlowJump<T>
{
    public Task<ChatFlowJump<T>> ForwardAsync(
        Func<T, Task<ChatFlowJump<T>>> nextAsync)
        =>
        InnerForwardAsync(
            nextAsync ?? throw new ArgumentNullException(nameof(nextAsync)));

    private Task<ChatFlowJump<T>> InnerForwardAsync(
        Func<T, Task<ChatFlowJump<T>>> nextAsync)
        =>
        Tag switch
        {
            ChatFlowJumpTag.Next => nextAsync.Invoke(nextState),
            _ => Task.FromResult(this)
        };
}