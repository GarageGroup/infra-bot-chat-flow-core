using System;
using Microsoft.Bot.Builder;
using Microsoft.Extensions.Logging;

namespace GGroupp.Infra.Bot.Builder;

public sealed partial class ChatFlow
{
    public static ChatFlow Create(
        ITurnContext turnContext,
        IBotUserProvider botUserProvider,
        ConversationState conversationState,
        IBotTelemetryClient botTelemetryClient,
        ILoggerFactory loggerFactory,
        string chatFlowId)
        =>
        InternalCreate(
            turnContext ?? throw new ArgumentNullException(nameof(turnContext)),
            botUserProvider ?? throw new ArgumentNullException(nameof(botUserProvider)),
            conversationState ?? throw new ArgumentNullException(nameof(conversationState)),
            botTelemetryClient ?? throw new ArgumentNullException(nameof(botTelemetryClient)),
            loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory)),
            chatFlowId ?? string.Empty);

    internal static ChatFlow InternalCreate(
        ITurnContext turnContext,
        IBotUserProvider botUserProvider,
        ConversationState conversationState,
        IBotTelemetryClient botTelemetryClient,
        ILoggerFactory loggerFactory,
        string chatFlowId)
        =>
        new(
            turnContext, botUserProvider, conversationState, botTelemetryClient, loggerFactory.CreateLogger(chatFlowId), chatFlowId);

    private readonly ITurnContext turnContext;

    private readonly IBotUserProvider botUserProvider;

    private readonly IChatFlowCache chatFlowCache;

    private readonly IBotTelemetryClient botTelemetryClient;

    private readonly ILogger logger;

    private readonly string chatFlowId;

    private ChatFlow(
        ITurnContext turnContext,
        IBotUserProvider botUserProvider,
        ConversationState conversationState,
        IBotTelemetryClient botTelemetryClient,
        ILogger logger,
        string chatFlowId)
    {
        this.turnContext = turnContext;
        this.botUserProvider = botUserProvider;
        chatFlowCache = new ChatFlowCacheImpl(chatFlowId, conversationState, turnContext);
        this.botTelemetryClient = botTelemetryClient;
        this.logger = logger;
        this.chatFlowId = chatFlowId;
    }
}