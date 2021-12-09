namespace GGroupp.Infra.Bot.Builder;

partial struct ChatFlowJump<T>
{
    public static ChatFlowJump<T> Next(T nextState) => new(nextState);
}