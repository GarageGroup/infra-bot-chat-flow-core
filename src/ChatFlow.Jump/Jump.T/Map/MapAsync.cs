using System;
using System.Threading.Tasks;

namespace GGroupp.Infra.Bot.Builder;

partial struct ChatFlowJump<T>
{
    public Task<ChatFlowJump<TResult>> MapAsync<TResult>(
        Func<T, Task<TResult>> mapNextAsync,
        Func<object?, Task<object?>> mapRepeatStateAsync,
        Func<ChatFlowBreakState, Task<ChatFlowBreakState>> mapBreakStateAsync)
        =>
        InnerMapAsync(
            mapNextAsync ?? throw new ArgumentNullException(nameof(mapNextAsync)),
            mapRepeatStateAsync ?? throw new ArgumentNullException(nameof(mapRepeatStateAsync)),
            mapBreakStateAsync ?? throw new ArgumentNullException(nameof(mapBreakStateAsync)));

    private async Task<ChatFlowJump<TResult>> InnerMapAsync<TResult>(
        Func<T, Task<TResult>> mapNextAsync,
        Func<object?, Task<object?>> mapRepeatStateAsync,
        Func<ChatFlowBreakState, Task<ChatFlowBreakState>> mapBreakStateAsync)
        =>
        Tag switch
        {
            ChatFlowJumpTag.Next => new(await mapNextAsync.Invoke(nextState).ConfigureAwait(false)),
            ChatFlowJumpTag.Repeat => new(await mapRepeatStateAsync.Invoke(repeatState).ConfigureAwait(false)),
            _ => new(await mapBreakStateAsync.Invoke(breakState).ConfigureAwait(false))
        };
}