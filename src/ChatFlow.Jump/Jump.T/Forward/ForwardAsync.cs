using System;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Bot.Builder;

partial struct ChatFlowJump<T>
{
    public Task<ChatFlowJump<TNextState>> ForwardAsync<TNextState>(
        Func<T, Task<ChatFlowJump<TNextState>>> nextAsync)
        =>
        InnerForwardAsync(
            nextAsync ?? throw new ArgumentNullException(nameof(nextAsync)));

    private async Task<ChatFlowJump<TNextState>> InnerForwardAsync<TNextState>(
        Func<T, Task<ChatFlowJump<TNextState>>> nextAsync)
        =>
        Tag switch
        {
            ChatFlowJumpTag.Next => await nextAsync.Invoke(nextState).ConfigureAwait(false),
            ChatFlowJumpTag.Repeat => new(repeatState),
            _ => new(breakState)
        };
}