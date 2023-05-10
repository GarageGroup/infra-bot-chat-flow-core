using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Bot.Builder;

partial class ChatFlow<T>
{
    public ChatFlow<TNext> MapFlowStateValue<TNext>(Func<T, CancellationToken, ValueTask<TNext>> mapFlowStateAsync)
        =>
        InnerMapFlowStateValue(
            mapFlowStateAsync ?? throw new ArgumentNullException(nameof(mapFlowStateAsync)));

    private ChatFlow<TNext> InnerMapFlowStateValue<TNext>(Func<T, CancellationToken, ValueTask<TNext>> mapFlowStateAsync)
        =>
        InnerNextValue<TNext>(
            (context, token) => mapFlowStateAsync.Invoke(context.FlowState, token));
}