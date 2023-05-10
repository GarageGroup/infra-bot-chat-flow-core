namespace GarageGroup.Infra.Bot.Builder;

partial class ChatFlowJump
{
    public static ChatFlowJump<T> Next<T>(T nextState) => new(nextState);
}