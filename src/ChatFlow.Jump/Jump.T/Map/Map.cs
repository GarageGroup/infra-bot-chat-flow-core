using System;

namespace GarageGroup.Infra.Bot.Builder;

partial struct ChatFlowJump<T>
{
    public ChatFlowJump<T> Map(
        Func<T, T> mapNextState,
        Func<T, T>? mapRestartState = null,
        Func<object?, object?>? mapRepeatState = null,
        Func<ChatFlowBreakState, ChatFlowBreakState>? mapBreakState = null)
        =>
        InnerMap(
            mapNextState ?? throw new ArgumentNullException(nameof(mapNextState)),
            mapRestartState,
            mapRepeatState,
            mapBreakState);

    private ChatFlowJump<T> InnerMap(
        Func<T, T> mapNextState,
        Func<T, T>? mapRestartState,
        Func<object?, object?>? mapRepeatState,
        Func<ChatFlowBreakState, ChatFlowBreakState>? mapBreakState)
        =>
        Tag switch
        {
            ChatFlowJumpTag.Next => new(mapNextState.Invoke(nextState)),
            ChatFlowJumpTag.Restart => mapRestartState is null ? this : new(mapRestartState.Invoke(nextState), restart: true),
            ChatFlowJumpTag.Repeat => mapRepeatState is null ? this : new(mapRepeatState.Invoke(repeatState)),
            _ => mapBreakState is null ? this : new(mapBreakState.Invoke(breakState))
        };
}