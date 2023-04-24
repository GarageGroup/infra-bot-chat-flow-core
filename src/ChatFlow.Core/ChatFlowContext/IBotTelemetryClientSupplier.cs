using Microsoft.Bot.Builder;

namespace GGroupp.Infra.Bot.Builder;

public interface IBotTelemetryClientSupplier
{
    IBotTelemetryClient BotTelemetryClient { get; }
}