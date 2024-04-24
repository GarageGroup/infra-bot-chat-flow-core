using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Bot.Builder;

partial class ChatFlow<T>
{
    public ChatFlow<T> MapFlowStateValue(Func<T, CancellationToken, ValueTask<T>> mapFlowStateAsync)
    {
        ArgumentNullException.ThrowIfNull(mapFlowStateAsync);

        return InnerNextValue(InnerGetNextAsync);

        ValueTask<T> InnerGetNextAsync(IChatFlowContext<T> context, CancellationToken cancellationToken)
            =>
            mapFlowStateAsync.Invoke(context.FlowState, cancellationToken);
    }
}