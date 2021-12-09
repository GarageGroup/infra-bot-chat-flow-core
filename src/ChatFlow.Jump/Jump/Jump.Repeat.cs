namespace GGroupp.Infra.Bot.Builder;

partial class ChatFlowJump
{
    public static ChatFlowJump<T> Repeat<T>(object? repeatState) => new(repeatState);
}