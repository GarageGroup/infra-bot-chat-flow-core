using System;
using Microsoft.Bot.Builder;
using Microsoft.Extensions.Logging;

namespace GGroupp.Infra.Bot.Builder;

public sealed partial class ChatFlow
{
    public static ChatFlow Create(ITurnContext turnContext, ConversationState conversationState, ILoggerFactory loggerFactory, string chatFlowId)
        =>
        InternalCreate(
            turnContext ?? throw new ArgumentNullException(nameof(turnContext)),
            conversationState ?? throw new ArgumentNullException(nameof(conversationState)),
            loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory)),
            chatFlowId ?? string.Empty);

    internal static ChatFlow InternalCreate(ITurnContext turnContext, ConversationState conversationState, ILoggerFactory loggerFactory, string chatFlowId)
        =>
        new(
            turnContext, conversationState, loggerFactory.CreateLogger(chatFlowId), chatFlowId);

    private readonly ITurnContext turnContext;

    private readonly IChatFlowCache chatFlowCache;

    private readonly ILogger logger;

    private readonly string chatFlowId;

    private ChatFlow(ITurnContext turnContext, ConversationState conversationState, ILogger logger, string chatFlowId)
    {
        this.turnContext = turnContext;
        chatFlowCache = new ChatFlowCacheImpl(chatFlowId, conversationState, turnContext);
        this.logger = logger;
        this.chatFlowId = chatFlowId;
    }
}