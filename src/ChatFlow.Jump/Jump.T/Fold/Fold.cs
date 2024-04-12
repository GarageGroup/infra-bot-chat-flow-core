using System;

namespace GarageGroup.Infra.Bot.Builder;

partial struct ChatFlowJump<T>
{
    public TResult Fold<TResult>(
        Func<T, TResult> mapNextState,
        Func<T, TResult> mapRestartState,
        Func<object?, TResult> mapRepeatState,
        Func<ChatFlowBreakState, TResult> mapBreakState)
        =>
        InnerFold(
            mapNextState ?? throw new ArgumentNullException(nameof(mapNextState)),
            mapRestartState ?? throw new ArgumentNullException(nameof(mapRestartState)),
            mapRepeatState ?? throw new ArgumentNullException(nameof(mapRepeatState)),
            mapBreakState ?? throw new ArgumentNullException(nameof(mapBreakState)));

    private TResult InnerFold<TResult>(
        Func<T, TResult> mapNextState,
        Func<T, TResult> mapRestartState,
        Func<object?, TResult> mapRepeatState,
        Func<ChatFlowBreakState, TResult> mapBreakState)
        =>
        Tag switch
        {
            ChatFlowJumpTag.Next => mapNextState.Invoke(nextState),
            ChatFlowJumpTag.Restart => mapRestartState.Invoke(nextState),
            ChatFlowJumpTag.Repeat => mapRepeatState.Invoke(repeatState),
            _ => mapBreakState.Invoke(breakState)
        };
}