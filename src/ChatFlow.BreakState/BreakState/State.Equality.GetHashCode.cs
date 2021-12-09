using System;

namespace GGroupp.Infra.Bot.Builder;

partial struct ChatFlowBreakState
{
    public override int GetHashCode()
        =>
        HashCode.Combine(
            EqualityContract,
            TypeComparer.GetHashCode(Type),
            UIMessageComparer.GetHashCode(UIMessage),
            LogMessageComparer.GetHashCode(LogMessage));
}