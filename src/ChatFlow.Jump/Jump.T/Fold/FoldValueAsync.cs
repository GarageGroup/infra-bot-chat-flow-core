using System;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Bot.Builder;

partial struct ChatFlowJump<T>
{
    public ValueTask<TResult> FoldValueAsync<TResult>(
        Func<T, ValueTask<TResult>> mapNextStateAsync,
        Func<object?, ValueTask<TResult>> mapRepeatStateAsync,
        Func<ChatFlowBreakState, ValueTask<TResult>> mapBeakStateAsync)
        =>
        InnerFoldValueAsync(
            mapNextStateAsync ?? throw new ArgumentNullException(nameof(mapNextStateAsync)),
            mapRepeatStateAsync ?? throw new ArgumentNullException(nameof(mapRepeatStateAsync)),
            mapBeakStateAsync ?? throw new ArgumentNullException(nameof(mapBeakStateAsync)));

    private ValueTask<TResult> InnerFoldValueAsync<TResult>(
        Func<T, ValueTask<TResult>> mapNextStateAsync,
        Func<object?, ValueTask<TResult>> mapRepeatStateAsync,
        Func<ChatFlowBreakState, ValueTask<TResult>> mapBeakStateAsync)
        =>
        Tag switch
        {
            ChatFlowJumpTag.Next => mapNextStateAsync.Invoke(nextState),
            ChatFlowJumpTag.Repeat => mapRepeatStateAsync.Invoke(repeatState),
            _ => mapBeakStateAsync.Invoke(breakState)
        };
}