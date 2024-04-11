using Microsoft.Bot.Builder;

namespace GarageGroup.Infra.Bot.Builder;

public interface IChatFlowStepContext<T> : IStepStateSupplier<T>, ITurnContext, ILoggerSupplier, IBotUserProviderSupplier, IBotTelemetryClientSupplier;