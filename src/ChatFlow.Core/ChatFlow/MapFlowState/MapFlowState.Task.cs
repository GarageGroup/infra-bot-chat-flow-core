using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Bot.Builder;

partial class ChatFlow<T>
{
    public ChatFlow<T> MapFlowState(Func<T, CancellationToken, Task<T>> mapFlowStateAsync)
    {
        ArgumentNullException.ThrowIfNull(mapFlowStateAsync);
        return InnerNext(InnerGetNextAsync);

        Task<T> InnerGetNextAsync(IChatFlowContext<T> context, CancellationToken cancellationToken)
            =>
            mapFlowStateAsync.Invoke(context.FlowState, cancellationToken);
    }
}