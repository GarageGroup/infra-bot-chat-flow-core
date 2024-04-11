using Microsoft.Bot.Builder;
using Microsoft.Extensions.Logging;

namespace GarageGroup.Infra.Bot.Builder;

internal sealed class ChatFlowEngineContext<T>
{
    internal ChatFlowEngineContext(
        string chatFlowId,
        ChatFlowStorage<T> chatFlowStorage,
        ITurnContext turnContext,
        IBotUserProvider botUserProvider,
        IBotTelemetryClient botTelemetryClient,
        ILogger logger)
    {
        ChatFlowId = chatFlowId;
        ChatFlowStorage = chatFlowStorage;
        TurnContext = turnContext;
        BotUserProvider = botUserProvider;
        BotTelemetryClient = botTelemetryClient;
        Logger = logger;
    }

    public string ChatFlowId { get; }

    public ChatFlowStorage<T> ChatFlowStorage { get; }

    public ITurnContext TurnContext { get; }

    public IBotUserProvider BotUserProvider { get; }

    public IBotTelemetryClient BotTelemetryClient { get; }

    public ILogger Logger { get; }
}