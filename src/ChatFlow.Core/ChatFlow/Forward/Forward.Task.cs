using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Bot.Builder;

partial class ChatFlow<T>
{
    public ChatFlow<T> Forward(Func<IChatFlowContext<T>, CancellationToken, Task<ChatFlowJump<T>>> forwardAsync)
        =>
        InnerForward(
            forwardAsync ?? throw new ArgumentNullException(nameof(forwardAsync)));

    private ChatFlow<T> InnerForward(Func<IChatFlowContext<T>, CancellationToken, Task<ChatFlowJump<T>>> forwardAsync)
        =>
        InnerForwardValue(
            (context, token) => new(forwardAsync.Invoke(context, token)));
}