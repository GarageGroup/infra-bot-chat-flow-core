using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Bot.Builder;

partial class ChatFlow<T>
{
    public ChatFlow<T> MapFlowStateValue(Func<T, CancellationToken, ValueTask<T>> mapFlowStateAsync)
        =>
        InnerMapFlowStateValue(
            mapFlowStateAsync ?? throw new ArgumentNullException(nameof(mapFlowStateAsync)));

    private ChatFlow<T> InnerMapFlowStateValue(Func<T, CancellationToken, ValueTask<T>> mapFlowStateAsync)
        =>
        InnerNextValue(
            (context, token) => mapFlowStateAsync.Invoke(context.FlowState, token));
}