using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Bot.Builder;

partial class ChatFlow<T>
{
    public ChatFlow<T> Next(Func<IChatFlowContext<T>, CancellationToken, Task<T>> nextAsync)
        =>
        InnerNext(
            nextAsync ?? throw new ArgumentNullException(nameof(nextAsync)));

    private ChatFlow<T> InnerNext(Func<IChatFlowContext<T>, CancellationToken, Task<T>> nextAsync)
        =>
        InnerForwardValue(
            async (context, token) => await nextAsync.Invoke(context, token).ConfigureAwait(false));
}