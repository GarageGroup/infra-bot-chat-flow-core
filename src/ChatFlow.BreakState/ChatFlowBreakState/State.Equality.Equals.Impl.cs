namespace GarageGroup.Infra.Bot.Builder;

partial struct ChatFlowBreakState
{
    public bool Equals(ChatFlowBreakState other)
        =>
        UserMessageComparer.Equals(UserMessage, other.UserMessage) &&
        LogMessageComparer.Equals(LogMessage, other.LogMessage) &&
        SourceExceptionComparer.Equals(SourceException, other.SourceException);
}