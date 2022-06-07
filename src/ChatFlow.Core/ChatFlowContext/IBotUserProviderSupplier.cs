namespace GGroupp.Infra.Bot.Builder;

public interface IBotUserProviderSupplier
{
    IBotUserProvider BotUserProvider { get; }
}