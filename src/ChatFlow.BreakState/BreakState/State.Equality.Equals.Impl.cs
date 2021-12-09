namespace GGroupp.Infra.Bot.Builder;

partial struct ChatFlowBreakState
{
    public bool Equals(ChatFlowBreakState other)
        =>
        TypeComparer.Equals(Type, other.Type) &&
        UIMessageComparer.Equals(UIMessage, other.UIMessage) &&
        LogMessageComparer.Equals(LogMessage, other.LogMessage);
}