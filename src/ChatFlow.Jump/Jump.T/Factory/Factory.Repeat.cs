namespace GarageGroup.Infra.Bot.Builder;

partial struct ChatFlowJump<T>
{
    public static ChatFlowJump<T> Repeat(object? repeatState) => new(repeatState);
}