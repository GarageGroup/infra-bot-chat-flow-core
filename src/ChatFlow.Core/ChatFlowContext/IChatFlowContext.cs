using System;

namespace GarageGroup.Infra.Bot.Builder;

public interface IChatFlowContext<T> : IFlowStateSupplier<T>, IChatFlowStepContext
{
    string ChatFlowId { get; }

    public IChatFlowContext<TResult> MapFlowState<TResult>(Func<T, TResult> mapFlowState)
        =>
        InternalMapFlowState(
            mapFlowState ?? throw new ArgumentNullException(nameof(mapFlowState)));

    internal IChatFlowContext<TResult> InternalMapFlowState<TResult>(Func<T, TResult> mapFlowState);
}