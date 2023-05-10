using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Bot.Builder;

partial class ChatFlow<T>
{
    public ChatFlow<TNext> Next<TNext>(Func<IChatFlowContext<T>, CancellationToken, Task<TNext>> nextAsync)
        =>
        InnerNext(
            nextAsync ?? throw new ArgumentNullException(nameof(nextAsync)));

    private ChatFlow<TNext> InnerNext<TNext>(Func<IChatFlowContext<T>, CancellationToken, Task<TNext>> nextAsync)
        =>
        chatFlowEngine.InternalForwardValue(
            async (context, token) => ChatFlowJump.Next(
                await nextAsync.Invoke(context, token).ConfigureAwait(false)))
        .ToChatFlow();
}