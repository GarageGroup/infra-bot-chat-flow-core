namespace GarageGroup.Infra.Bot.Builder;

partial struct ChatFlowJump<T>
{
    public bool Equals(ChatFlowJump<T> other)
        =>
        TagComparer.Equals(Tag, other.Tag) &&
        NextStateComparer.Equals(nextState, other.nextState) &&
        RepeatStateComparer.Equals(repeatState, other.repeatState) &&
        BreakStateComparer.Equals(breakState, other.breakState);
}