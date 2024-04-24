using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Bot.Builder;

partial class ChatFlow<T>
{
    public ChatFlow<T> On(Func<IChatFlowContext<T>, CancellationToken, Task<Unit>> onAsync)
    {
        ArgumentNullException.ThrowIfNull(onAsync);
        return InnerOn(onAsync);
    }

    public ChatFlow<T> On(Func<IChatFlowContext<T>, CancellationToken, Task> onAsync)
    {
        ArgumentNullException.ThrowIfNull(onAsync);
        return InnerOn(onAsync);
    }

    private ChatFlow<T> InnerOn(Func<IChatFlowContext<T>, CancellationToken, Task<Unit>> onAsync)
    {
        return InnerNext(InnerGetNextAsync);

        async Task<T> InnerGetNextAsync(IChatFlowContext<T> context, CancellationToken cancellationToken)
        {
            _ = await onAsync.Invoke(context, cancellationToken).ConfigureAwait(false);
            return context.FlowState;
        }
    }

    private ChatFlow<T> InnerOn(Func<IChatFlowContext<T>, CancellationToken, Task> onAsync)
    {
        return InnerNext(InnerGetNextAsync);

        async Task<T> InnerGetNextAsync(IChatFlowContext<T> context, CancellationToken cancellationToken)
        {
            await onAsync.Invoke(context, cancellationToken).ConfigureAwait(false);
            return context.FlowState;
        }
    }
}