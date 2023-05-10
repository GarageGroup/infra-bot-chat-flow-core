using Microsoft.Bot.Builder;
using Microsoft.Extensions.Logging;

namespace GarageGroup.Infra.Bot.Builder;

internal interface IChatFlowEngineContext
{
    string ChatFlowId { get; }

    IChatFlowCache ChatFlowCache { get; }

    ITurnContext TurnContext { get; }

    IBotUserProvider BotUserProvider { get; }

    IBotTelemetryClient BotTelemetryClient { get; }

    ILogger Logger { get; }
}