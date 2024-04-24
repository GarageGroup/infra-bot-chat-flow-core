using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Bot.Builder;

partial class ChatFlow<T>
{
    public ChatFlow<T> OnValue(Func<IChatFlowContext<T>, CancellationToken, ValueTask<Unit>> onAsync)
    {
        ArgumentNullException.ThrowIfNull(onAsync);
        return InnerOnValue(onAsync);
    }

    public ChatFlow<T> OnValue(Func<IChatFlowContext<T>, CancellationToken, ValueTask> onAsync)
    {
        ArgumentNullException.ThrowIfNull(onAsync);
        return InnerOnValue(onAsync);
    }

    private ChatFlow<T> InnerOnValue(Func<IChatFlowContext<T>, CancellationToken, ValueTask<Unit>> onAsync)
    {
        return InnerNextValue(InnerGetNextAsync);

        async ValueTask<T> InnerGetNextAsync(IChatFlowContext<T> context, CancellationToken cancellationToken)
        {
            _ = await onAsync.Invoke(context, cancellationToken).ConfigureAwait(false);
            return context.FlowState;
        }
    }

    private ChatFlow<T> InnerOnValue(Func<IChatFlowContext<T>, CancellationToken, ValueTask> onAsync)
    {
        return InnerNextValue(InnerGetNextAsync);

        async ValueTask<T> InnerGetNextAsync(IChatFlowContext<T> context, CancellationToken cancellationToken)
        {
            await onAsync.Invoke(context, cancellationToken).ConfigureAwait(false);
            return context.FlowState;
        }
    }
}