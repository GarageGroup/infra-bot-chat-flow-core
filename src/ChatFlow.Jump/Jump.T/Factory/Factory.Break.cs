namespace GarageGroup.Infra.Bot.Builder;

partial struct ChatFlowJump<T>
{
    public static ChatFlowJump<T> Break(ChatFlowBreakState breakState) => new(breakState);
}