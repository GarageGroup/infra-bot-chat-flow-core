using Microsoft.Bot.Builder;

namespace GarageGroup.Infra.Bot.Builder;

public interface IBotTelemetryClientSupplier
{
    IBotTelemetryClient BotTelemetryClient { get; }
}