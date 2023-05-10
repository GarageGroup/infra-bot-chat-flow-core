using System;

namespace GarageGroup.Infra.Bot.Builder;

public static class BotContextExtensions
{
    public static ChatFlow CreateChatFlow(this IBotContext botContext, string chatFlowId)
    {
        ArgumentNullException.ThrowIfNull(botContext);

        return ChatFlow.InternalCreate(
            botContext.TurnContext,
            botContext.BotUserProvider,
            botContext.ConversationState,
            botContext.BotTelemetryClient,
            botContext.LoggerFactory,
            chatFlowId ?? string.Empty);
    }
}