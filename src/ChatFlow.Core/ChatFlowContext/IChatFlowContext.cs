using System;

namespace GGroupp.Infra.Bot.Builder;

public interface IChatFlowContext<T> : IFlowStateSupplier<T>, IChatFlowStepContext
{
    public IChatFlowContext<TResult> MapFlowState<TResult>(Func<T, TResult> mapFlowState)
        =>
        InternalMapFlowState(
            mapFlowState ?? throw new ArgumentNullException(nameof(mapFlowState)));

    internal IChatFlowContext<TResult> InternalMapFlowState<TResult>(Func<T, TResult> mapFlowState);
}