namespace GarageGroup.Infra.Bot.Builder;

partial struct ChatFlowJump<T>
{
    public static ChatFlowJump<T> Restart(T nextState)
        =>
        new(nextState, restart: true);
}