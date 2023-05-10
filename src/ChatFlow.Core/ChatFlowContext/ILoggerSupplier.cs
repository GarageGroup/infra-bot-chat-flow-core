using Microsoft.Extensions.Logging;

namespace GarageGroup.Infra.Bot.Builder;

public interface ILoggerSupplier
{
    ILogger Logger { get; }
}