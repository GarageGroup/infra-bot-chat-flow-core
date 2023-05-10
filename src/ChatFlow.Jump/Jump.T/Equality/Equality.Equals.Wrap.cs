namespace GarageGroup.Infra.Bot.Builder;

partial struct ChatFlowJump<T>
{
    public static bool Equals(ChatFlowJump<T> left, ChatFlowJump<T> right)
        =>
        left.Equals(right);

    public static bool operator ==(ChatFlowJump<T> left, ChatFlowJump<T> right)
        =>
        left.Equals(right);

    public static bool operator !=(ChatFlowJump<T> left, ChatFlowJump<T> right)
        =>
        left.Equals(right) is false;

    public override bool Equals(object? obj)
        =>
        obj is ChatFlowJump<T> other && Equals(other);
}