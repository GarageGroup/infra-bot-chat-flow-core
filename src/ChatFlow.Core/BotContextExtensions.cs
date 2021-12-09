using System;

namespace GGroupp.Infra.Bot.Builder;

public static class BotContextExtensions
{
    public static ChatFlow CreateChatFlow(this IBotContext botContext, string chatFlowId)
        =>
        InnerCreateChatFlow(
            botContext ?? throw new ArgumentNullException(nameof(botContext)),
            chatFlowId ?? string.Empty);

    private static ChatFlow InnerCreateChatFlow(IBotContext botContext, string chatFlowId)
        =>
        ChatFlow.InternalCreate(
            botContext.TurnContext, botContext.ConversationState, botContext.LoggerFactory, chatFlowId);
}