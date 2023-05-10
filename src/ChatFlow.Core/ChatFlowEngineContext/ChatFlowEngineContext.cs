using Microsoft.Bot.Builder;
using Microsoft.Extensions.Logging;

namespace GarageGroup.Infra.Bot.Builder;

internal sealed class ChatFlowEngineContext : IChatFlowEngineContext
{
    internal ChatFlowEngineContext(
        string chatFlowId,
        IChatFlowCache chatFlowCache,
        ITurnContext turnContext,
        IBotUserProvider botUserProvider,
        IBotTelemetryClient botTelemetryClient,
        ILogger logger)
    {
        ChatFlowId = chatFlowId;
        ChatFlowCache = chatFlowCache;
        TurnContext = turnContext;
        BotUserProvider = botUserProvider;
        BotTelemetryClient = botTelemetryClient;
        Logger = logger;
    }

    public string ChatFlowId { get; }

    public IChatFlowCache ChatFlowCache { get; }

    public ITurnContext TurnContext { get; }

    public IBotUserProvider BotUserProvider { get; }

    public IBotTelemetryClient BotTelemetryClient { get; }

    public ILogger Logger { get; }
}