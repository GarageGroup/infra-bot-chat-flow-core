using Microsoft.Bot.Builder;
using Microsoft.Extensions.Logging;

namespace GGroupp.Infra.Bot.Builder;

internal sealed class ChatFlowEngineContext : IChatFlowEngineContext
{
    public ChatFlowEngineContext(
        IChatFlowCache chatFlowCache,
        ITurnContext turnContext,
        IBotUserProvider botUserProvider,
        IBotTelemetryClient botTelemetryClient,
        ILogger logger)
    {
        ChatFlowCache = chatFlowCache;
        TurnContext = turnContext;
        BotUserProvider = botUserProvider;
        BotTelemetryClient = botTelemetryClient;
        Logger = logger;
    }

    public IChatFlowCache ChatFlowCache { get; }

    public ITurnContext TurnContext { get; }

    public IBotUserProvider BotUserProvider { get; }

    public IBotTelemetryClient BotTelemetryClient { get; }

    public ILogger Logger { get; }
}