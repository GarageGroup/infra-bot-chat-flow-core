using System;

namespace GarageGroup.Infra.Bot.Builder;

public interface IStepStateSupplier<TFlowState>
{
    public object? StepState { get; }

    public ChatFlowJump<TFlowState> RepeatSameStateJump()
        =>
        new(StepState);

    public ChatFlowJump<TFlowState> RepeatSameStateJump(Unit _)
        =>
        new(StepState);
}