using System;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Bot.Builder;

partial struct ChatFlowJump<T>
{
    public ValueTask<TResult> FoldValueAsync<TResult>(
        Func<T, ValueTask<TResult>> mapNextStateAsync,
        Func<T, ValueTask<TResult>> mapRestartStateAsync,
        Func<object?, ValueTask<TResult>> mapRepeatStateAsync,
        Func<ChatFlowBreakState, ValueTask<TResult>> mapBeakStateAsync)
        =>
        InnerFoldValueAsync(
            mapNextStateAsync ?? throw new ArgumentNullException(nameof(mapNextStateAsync)),
            mapRestartStateAsync ?? throw new ArgumentNullException(nameof(mapRestartStateAsync)),
            mapRepeatStateAsync ?? throw new ArgumentNullException(nameof(mapRepeatStateAsync)),
            mapBeakStateAsync ?? throw new ArgumentNullException(nameof(mapBeakStateAsync)));

    private ValueTask<TResult> InnerFoldValueAsync<TResult>(
        Func<T, ValueTask<TResult>> mapNextStateAsync,
        Func<T, ValueTask<TResult>> mapRestartStateAsync,
        Func<object?, ValueTask<TResult>> mapRepeatStateAsync,
        Func<ChatFlowBreakState, ValueTask<TResult>> mapBeakStateAsync)
        =>
        Tag switch
        {
            ChatFlowJumpTag.Next => mapNextStateAsync.Invoke(nextState),
            ChatFlowJumpTag.Restart => mapRestartStateAsync.Invoke(nextState),
            ChatFlowJumpTag.Repeat => mapRepeatStateAsync.Invoke(repeatState),
            _ => mapBeakStateAsync.Invoke(breakState)
        };
}