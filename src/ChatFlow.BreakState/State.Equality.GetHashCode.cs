using System;

namespace GarageGroup.Infra.Bot.Builder;

partial struct ChatFlowBreakState
{
    public override int GetHashCode()
        =>
        HashCode.Combine(
            EqualityContract,
            UserMessageComparer.GetHashCode(UserMessage),
            LogMessageComparer.GetHashCode(LogMessage));
}