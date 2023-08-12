namespace GarageGroup.Infra.Bot.Builder;

partial struct ChatFlowJump<T>
{
    public static implicit operator ChatFlowJump<T>(ChatFlowBreakState breakState)
        =>
        new(breakState);
}