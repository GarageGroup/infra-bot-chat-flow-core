using Microsoft.Bot.Builder;

namespace GarageGroup.Infra.Bot.Builder;

public interface IChatFlowStepContext : IStepStateSupplier, ITurnContext, ILoggerSupplier, IBotUserProviderSupplier, IBotTelemetryClientSupplier;