using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Bot.Builder;

partial class ChatFlow<T>
{
    public ChatFlow<T> NextValue(Func<IChatFlowContext<T>, CancellationToken, ValueTask<T>> nextAsync)
        =>
        InnerNextValue(
            nextAsync ?? throw new ArgumentNullException(nameof(nextAsync)));

    private ChatFlow<T> InnerNextValue(Func<IChatFlowContext<T>, CancellationToken, ValueTask<T>> nextAsync)
        =>
        InnerForwardValue(
            async (context, token) => await nextAsync.Invoke(context, token).ConfigureAwait(false));
}