using System;

namespace GGroupp.Infra.Bot.Builder;

public static class BotContextExtensions
{
    public static ChatFlow CreateChatFlow(this IBotContext botContext, string chatFlowId)
    {
        _ = botContext ?? throw new ArgumentNullException(nameof(botContext));

        return ChatFlow.InternalCreate(
            botContext.TurnContext,
            botContext.BotUserProvider,
            botContext.ConversationState,
            botContext.BotTelemetryClient,
            botContext.LoggerFactory,
            chatFlowId ?? string.Empty);
    }
}