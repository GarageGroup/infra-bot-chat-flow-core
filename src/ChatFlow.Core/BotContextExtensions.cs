using System;

namespace GarageGroup.Infra.Bot.Builder;

public static class BotContextExtensions
{
    public static ChatFlowStarter<TFlowStateJson> CreateChatFlow<TFlowStateJson>(this IBotContext botContext, string chatFlowId)
    {
        ArgumentNullException.ThrowIfNull(botContext);

        return new(
            turnContext: botContext.TurnContext,
            botUserProvider: botContext.BotUserProvider,
            conversationState: botContext.ConversationState,
            botTelemetryClient: botContext.BotTelemetryClient,
            loggerFactory: botContext.LoggerFactory,
            chatFlowId: chatFlowId ?? string.Empty);
    }
}