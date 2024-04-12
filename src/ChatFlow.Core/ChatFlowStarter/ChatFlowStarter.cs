using System;
using Microsoft.Bot.Builder;
using Microsoft.Extensions.Logging;

namespace GarageGroup.Infra.Bot.Builder;

public sealed partial class ChatFlowStarter<TFlowStateJson>
{
    public static ChatFlowStarter<TFlowStateJson> Create(
        ITurnContext turnContext,
        IBotUserProvider botUserProvider,
        ConversationState conversationState,
        IBotTelemetryClient botTelemetryClient,
        ILoggerFactory loggerFactory,
        string chatFlowId)
        =>
        new(
            turnContext ?? throw new ArgumentNullException(nameof(turnContext)),
            botUserProvider ?? throw new ArgumentNullException(nameof(botUserProvider)),
            conversationState ?? throw new ArgumentNullException(nameof(conversationState)),
            botTelemetryClient ?? throw new ArgumentNullException(nameof(botTelemetryClient)),
            loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory)),
            chatFlowId ?? string.Empty);

    private readonly ChatFlowEngineContext<TFlowStateJson> flowEngineContext;

    internal ChatFlowStarter(
        ITurnContext turnContext,
        IBotUserProvider botUserProvider,
        ConversationState conversationState,
        IBotTelemetryClient botTelemetryClient,
        ILoggerFactory loggerFactory,
        string chatFlowId)
    {
        var logger = loggerFactory.CreateLogger<ChatFlowStarter<TFlowStateJson>>();
        var storage = new ChatFlowStorage<TFlowStateJson>(chatFlowId, conversationState, turnContext);

        flowEngineContext = new(chatFlowId, storage, turnContext, botUserProvider, botTelemetryClient, logger);
    }
}