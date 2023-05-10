using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Bot.Builder;

partial class ChatFlow<T>
{
    public ChatFlow<TNext> ForwardValue<TNext>(
        Func<IChatFlowContext<T>, CancellationToken, ValueTask<ChatFlowJump<TNext>>> forwardAsync)
        =>
        InnerForwardValue(
            forwardAsync ?? throw new ArgumentNullException(nameof(forwardAsync)));

    private ChatFlow<TNext> InnerForwardValue<TNext>(
        Func<IChatFlowContext<T>, CancellationToken, ValueTask<ChatFlowJump<TNext>>> forwardAsync)
        =>
        chatFlowEngine.InternalForwardValue(forwardAsync).ToChatFlow();
}