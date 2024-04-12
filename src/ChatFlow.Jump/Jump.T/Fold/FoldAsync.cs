using System;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Bot.Builder;

partial struct ChatFlowJump<T>
{
    public Task<TResult> FoldAsync<TResult>(
        Func<T, Task<TResult>> mapNextStateAsync,
        Func<T, Task<TResult>> mapRestartStateAsync,
        Func<object?, Task<TResult>> mapRepeatStateAsync,
        Func<ChatFlowBreakState, Task<TResult>> mapBeakStateAsync)
        =>
        InnerFoldAsync(
            mapNextStateAsync ?? throw new ArgumentNullException(nameof(mapNextStateAsync)),
            mapRestartStateAsync ?? throw new ArgumentNullException(nameof(mapRestartStateAsync)),
            mapRepeatStateAsync ?? throw new ArgumentNullException(nameof(mapRepeatStateAsync)),
            mapBeakStateAsync ?? throw new ArgumentNullException(nameof(mapBeakStateAsync)));

    private Task<TResult> InnerFoldAsync<TResult>(
        Func<T, Task<TResult>> mapNextStateAsync,
        Func<T, Task<TResult>> mapRestartStateAsync,
        Func<object?, Task<TResult>> mapRepeatStateAsync,
        Func<ChatFlowBreakState, Task<TResult>> mapBeakStateAsync)
        =>
        Tag switch
        {
            ChatFlowJumpTag.Next => mapNextStateAsync.Invoke(nextState),
            ChatFlowJumpTag.Restart => mapRestartStateAsync.Invoke(nextState),
            ChatFlowJumpTag.Repeat => mapRepeatStateAsync.Invoke(repeatState),
            _ => mapBeakStateAsync.Invoke(breakState)
        };
}