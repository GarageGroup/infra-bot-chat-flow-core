using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Bot.Builder;

partial class ChatFlow<T>
{
    public ChatFlow<T> ForwardValue(
        Func<IChatFlowContext<T>, CancellationToken, ValueTask<ChatFlowJump<T>>> forwardAsync)
        =>
        InnerForwardValue(
            forwardAsync ?? throw new ArgumentNullException(nameof(forwardAsync)));

    private ChatFlow<T> InnerForwardValue(
        Func<IChatFlowContext<T>, CancellationToken, ValueTask<ChatFlowJump<T>>> forwardAsync)
    {
        flowSteps.Add(forwardAsync);
        return this;
    }
}