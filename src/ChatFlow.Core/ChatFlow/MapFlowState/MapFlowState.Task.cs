using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Bot.Builder;

partial class ChatFlow<T>
{
    public ChatFlow<T> MapFlowState(Func<T, CancellationToken, Task<T>> mapFlowStateAsync)
        =>
        InnerMapFlowState(
            mapFlowStateAsync ?? throw new ArgumentNullException(nameof(mapFlowStateAsync)));

    private ChatFlow<T> InnerMapFlowState(Func<T, CancellationToken, Task<T>> mapFlowStateAsync)
        =>
        InnerNext(
            (context, token) => mapFlowStateAsync.Invoke(context.FlowState, token));
}