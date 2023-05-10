namespace GarageGroup.Infra.Bot.Builder;

partial class ChatFlowJump
{
    public static ChatFlowJump<T> Break<T>(ChatFlowBreakState breakState) => new(breakState);
}