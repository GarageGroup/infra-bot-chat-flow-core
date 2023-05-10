namespace GarageGroup.Infra.Bot.Builder;

partial struct ChatFlowBreakState
{
    public static bool Equals(ChatFlowBreakState left, ChatFlowBreakState right)
        =>
        left.Equals(right);

    public static bool operator ==(ChatFlowBreakState left, ChatFlowBreakState right)
        =>
        left.Equals(right);

    public static bool operator !=(ChatFlowBreakState left, ChatFlowBreakState right)
        =>
        left.Equals(right) is false;

    public override bool Equals(object? obj)
        =>
        obj is ChatFlowBreakState other && Equals(other);
}