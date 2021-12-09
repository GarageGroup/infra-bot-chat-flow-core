namespace GGroupp.Infra.Bot.Builder;

partial struct ChatFlowJump<T>
{
    public static implicit operator ChatFlowJump<T>(T nextState) => new(nextState);
}