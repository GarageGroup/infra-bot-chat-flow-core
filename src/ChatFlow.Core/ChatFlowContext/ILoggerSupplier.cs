using Microsoft.Extensions.Logging;

namespace GGroupp.Infra.Bot.Builder;

public interface ILoggerSupplier
{
    ILogger Logger { get; }
}