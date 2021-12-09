using System;
using System.Threading.Tasks;

namespace GGroupp.Infra.Bot.Builder;

partial class ChatFlow
{
    public ChatFlow<T> Start<T>() where T : new()
        =>
        InnerStart(
            () => new T());

    public ChatFlow<T> Start<T>(Func<T> initialFactory)
        =>
        InnerStart(
            initialFactory ?? throw new ArgumentNullException(nameof(initialFactory)));

    private ChatFlow<T> InnerStart<T>(Func<T> initialFactory)
        =>
        new ChatFlowEngine<T>(
            chatFlowId: chatFlowId,
            stepPosition: default,
            chatFlowCache: chatFlowCache,
            turnContext: turnContext,
            logger: logger,
            _ => initialFactory.Invoke().InternalPipe(ChatFlowJump<T>.Next).InternalPipe(ValueTask.FromResult))
        .ToChatFlow();
}