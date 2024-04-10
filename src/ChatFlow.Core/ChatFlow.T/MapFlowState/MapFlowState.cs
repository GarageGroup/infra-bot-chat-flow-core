using System;

namespace GarageGroup.Infra.Bot.Builder;

partial class ChatFlow<T>
{
    public ChatFlow<TNext> MapFlowState<TNext>(Func<T, TNext> mapFlowState)
        =>
        InnerMapFlowState(
            mapFlowState ?? throw new ArgumentNullException(nameof(mapFlowState)));

    private ChatFlow<TNext> InnerMapFlowState<TNext>(Func<T, TNext> mapFlowState)
        =>
        InnerNext(
            context => mapFlowState.Invoke(context.FlowState));
}