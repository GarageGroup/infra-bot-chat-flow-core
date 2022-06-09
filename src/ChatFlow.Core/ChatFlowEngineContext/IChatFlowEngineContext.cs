using Microsoft.Bot.Builder;
using Microsoft.Extensions.Logging;

namespace GGroupp.Infra.Bot.Builder;

internal interface IChatFlowEngineContext
{
    IChatFlowCache ChatFlowCache { get; }

    ITurnContext TurnContext { get; }

    IBotUserProvider BotUserProvider { get; }

    IBotTelemetryClient BotTelemetryClient { get; }

    ILogger Logger { get; }
}