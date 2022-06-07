using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;

namespace GGroupp.Infra.Bot.Builder;

partial class ChatFlow
{
    public ChatFlow<T> Start<T>(Func<T> initialFactory)
    {
        _ = initialFactory ?? throw new ArgumentNullException(nameof(initialFactory));

        return new ChatFlowEngine<T>(chatFlowId, default, chatFlowCache, turnContext, botUserProvider, logger, InitializeFlowAsync).ToChatFlow();

        ValueTask<ChatFlowJump<T>> InitializeFlowAsync(CancellationToken cancellationToken)
        {
            botTelemetryClient.TrackDialogView(chatFlowId);
            return initialFactory.Invoke().InternalPipe(ChatFlowJump<T>.Next).InternalPipe(ValueTask.FromResult);
        }
    }
}