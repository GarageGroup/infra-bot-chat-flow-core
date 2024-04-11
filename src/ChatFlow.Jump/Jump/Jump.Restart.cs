namespace GarageGroup.Infra.Bot.Builder;

partial class ChatFlowJump
{
    public static ChatFlowJump<T> Restart<T>(T initialState)
        =>
        new(initialState, restart: true);
}