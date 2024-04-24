using System;

namespace GarageGroup.Infra.Bot.Builder;

partial class ChatFlow<T>
{
    public ChatFlow<T> MapFlowState(Func<T, T> mapFlowState)
    {
        ArgumentNullException.ThrowIfNull(mapFlowState);
        return InnerNext(InnerGetNext);

        T InnerGetNext(IChatFlowContext<T> context)
            =>
            mapFlowState.Invoke(context.FlowState);
    }
}