using System;

namespace GGroupp.Infra.Bot.Builder;

partial struct ChatFlowJump<T>
{
    public override int GetHashCode()
        =>
        Tag switch
        {
            ChatFlowJumpTag.Next => GetNextStateHashCode(),
            ChatFlowJumpTag.Repeat => GetRepeatStateHashCode(),
            _ => GetBreakStateHashCode()
        };

    private int GetNextStateHashCode()
        =>
        nextState is not null
        ? HashCode.Combine(EqualityContract, TagComparer.GetHashCode(Tag), NextStateComparer.GetHashCode(nextState))
        : HashCode.Combine(EqualityContract, TagComparer.GetHashCode(Tag));

    private int GetRepeatStateHashCode()
        =>
        repeatState is not null
        ? HashCode.Combine(EqualityContract, TagComparer.GetHashCode(Tag), RepeatStateComparer.GetHashCode(repeatState))
        : HashCode.Combine(EqualityContract, TagComparer.GetHashCode(Tag));

    private int GetBreakStateHashCode()
        =>
        HashCode.Combine(EqualityContract, TagComparer.GetHashCode(Tag), BreakStateComparer.GetHashCode(breakState));
}