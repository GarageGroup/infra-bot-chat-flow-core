using System;
using System.Threading.Tasks;

namespace GGroupp.Infra.Bot.Builder;

partial struct ChatFlowJump<T>
{
    public ValueTask<ChatFlowJump<TResult>> MapValueAsync<TResult>(
        Func<T, ValueTask<TResult>> mapNextAsync,
        Func<object?, ValueTask<object?>> mapRepeatStateAsync,
        Func<ChatFlowBreakState, ValueTask<ChatFlowBreakState>> mapBreakStateAsync)
        =>
        InnerMapValueAsync(
            mapNextAsync ?? throw new ArgumentNullException(nameof(mapNextAsync)),
            mapRepeatStateAsync ?? throw new ArgumentNullException(nameof(mapRepeatStateAsync)),
            mapBreakStateAsync ?? throw new ArgumentNullException(nameof(mapBreakStateAsync)));

    private async ValueTask<ChatFlowJump<TResult>> InnerMapValueAsync<TResult>(
        Func<T, ValueTask<TResult>> mapNextAsync,
        Func<object?, ValueTask<object?>> mapRepeatStateAsync,
        Func<ChatFlowBreakState, ValueTask<ChatFlowBreakState>> mapBreakStateAsync)
        =>
        Tag switch
        {
            ChatFlowJumpTag.Next => new(await mapNextAsync.Invoke(nextState).ConfigureAwait(false)),
            ChatFlowJumpTag.Repeat => new(await mapRepeatStateAsync.Invoke(repeatState).ConfigureAwait(false)),
            _ => new(await mapBreakStateAsync.Invoke(breakState).ConfigureAwait(false))
        };
}