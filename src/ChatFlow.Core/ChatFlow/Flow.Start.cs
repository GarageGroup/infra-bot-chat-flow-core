using System;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra.Bot.Builder;

partial class ChatFlow
{
    public ChatFlow<T> Start<T>(Func<T> initialFactory)
    {
        _ = initialFactory ?? throw new ArgumentNullException(nameof(initialFactory));

        var engineContext = new ChatFlowEngineContext(chatFlowCache, turnContext, botUserProvider, botTelemetryClient, logger);
        return new ChatFlowEngine<T>(chatFlowId, default, engineContext, InitializeFlowAsync).ToChatFlow();

        ValueTask<ChatFlowJump<T>> InitializeFlowAsync(CancellationToken cancellationToken)
            =>
            initialFactory.Invoke().InternalPipe(ChatFlowJump<T>.Next).InternalPipe(ValueTask.FromResult);
    }
}