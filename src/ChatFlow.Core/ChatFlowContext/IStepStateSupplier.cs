using System;

namespace GarageGroup.Infra.Bot.Builder;

public interface IStepStateSupplier
{
    public object? StepState { get; }

    public ChatFlowJump<TNext> RepeatSameStateJump<TNext>()
        =>
        new(StepState);

    public ChatFlowJump<TNext> RepeatSameStateJump<TNext>(Unit _)
        =>
        new(StepState);
}