namespace GarageGroup.Infra.Bot.Builder;

public interface IBotUserProviderSupplier
{
    IBotUserProvider BotUserProvider { get; }
}