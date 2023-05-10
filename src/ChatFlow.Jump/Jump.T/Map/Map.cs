using System;

namespace GarageGroup.Infra.Bot.Builder;

partial struct ChatFlowJump<T>
{
    public ChatFlowJump<TResult> Map<TResult>(
        Func<T, TResult> mapNextState,
        Func<object?, object?> mapRepeatState,
        Func<ChatFlowBreakState, ChatFlowBreakState> mapBreakState)
        =>
        InnerMap(
            mapNextState ?? throw new ArgumentNullException(nameof(mapNextState)),
            mapRepeatState ?? throw new ArgumentNullException(nameof(mapRepeatState)),
            mapBreakState ?? throw new ArgumentNullException(nameof(mapBreakState)));

    private ChatFlowJump<TResult> InnerMap<TResult>(
        Func<T, TResult> mapNextState,
        Func<object?, object?> mapRepeatState,
        Func<ChatFlowBreakState, ChatFlowBreakState> mapBreakState)
        =>
        Tag switch
        {
            ChatFlowJumpTag.Next => new(mapNextState.Invoke(nextState)),
            ChatFlowJumpTag.Repeat => new(mapRepeatState.Invoke(repeatState)),
            _ => new(mapBreakState.Invoke(breakState))
        };
}