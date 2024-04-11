using System;

namespace GarageGroup.Infra.Bot.Builder;

partial class ChatFlow<T>
{
    public ChatFlow<T> MapFlowState(Func<T, T> mapFlowState)
        =>
        InnerMapFlowState(
            mapFlowState ?? throw new ArgumentNullException(nameof(mapFlowState)));

    private ChatFlow<T> InnerMapFlowState(Func<T, T> mapFlowState)
        =>
        InnerNext(
            context => mapFlowState.Invoke(context.FlowState));
}