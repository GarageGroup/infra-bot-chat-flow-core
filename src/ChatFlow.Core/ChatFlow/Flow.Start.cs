using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Bot.Builder;

partial class ChatFlow
{
    public ChatFlow<T> Start<T>(Func<T> initialFactory)
    {
        ArgumentNullException.ThrowIfNull(initialFactory);

        var engineContext = new ChatFlowEngineContext(chatFlowId, chatFlowCache, turnContext, botUserProvider, botTelemetryClient, logger);
        return new ChatFlowEngine<T>(engineContext, default, InitializeFlowAsync).ToChatFlow();

        ValueTask<ChatFlowJump<T>> InitializeFlowAsync(CancellationToken cancellationToken)
            =>
            initialFactory.Invoke().InternalPipe(ChatFlowJump<T>.Next).InternalPipe(ValueTask.FromResult);
    }
}