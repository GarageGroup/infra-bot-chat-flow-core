using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Bot.Builder;

partial class ChatFlow<T>
{
    public ChatFlow<TNext> MapFlowState<TNext>(Func<T, CancellationToken, Task<TNext>> mapFlowStateAsync)
        =>
        InnerMapFlowState(
            mapFlowStateAsync ?? throw new ArgumentNullException(nameof(mapFlowStateAsync)));

    private ChatFlow<TNext> InnerMapFlowState<TNext>(Func<T, CancellationToken, Task<TNext>> mapFlowStateAsync)
        =>
        InnerNext(
            (context, token) => mapFlowStateAsync.Invoke(context.FlowState, token));
}