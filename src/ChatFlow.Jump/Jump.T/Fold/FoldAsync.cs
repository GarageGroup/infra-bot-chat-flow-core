using System;
using System.Threading.Tasks;

namespace GGroupp.Infra.Bot.Builder;

partial struct ChatFlowJump<T>
{
    public Task<TResult> FoldAsync<TResult>(
        Func<T, Task<TResult>> mapNextStateAsync,
        Func<object?, Task<TResult>> mapRepeatStateAsync,
        Func<ChatFlowBreakState, Task<TResult>> mapBeakStateAsync)
        =>
        InnerFoldAsync(
            mapNextStateAsync ?? throw new ArgumentNullException(nameof(mapNextStateAsync)),
            mapRepeatStateAsync ?? throw new ArgumentNullException(nameof(mapRepeatStateAsync)),
            mapBeakStateAsync ?? throw new ArgumentNullException(nameof(mapBeakStateAsync)));

    private Task<TResult> InnerFoldAsync<TResult>(
        Func<T, Task<TResult>> mapNextStateAsync,
        Func<object?, Task<TResult>> mapRepeatStateAsync,
        Func<ChatFlowBreakState, Task<TResult>> mapBeakStateAsync)
        =>
        Tag switch
        {
            ChatFlowJumpTag.Next => mapNextStateAsync.Invoke(nextState),
            ChatFlowJumpTag.Repeat => mapRepeatStateAsync.Invoke(repeatState),
            _ => mapBeakStateAsync.Invoke(breakState)
        };
}