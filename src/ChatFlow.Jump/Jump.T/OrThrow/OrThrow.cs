using System;
using static System.FormattableString;

namespace GGroupp.Infra.Bot.Builder;

partial struct ChatFlowJump<T>
{
    private InvalidOperationException CreateUnexpectedTagException(ChatFlowJumpTag expectedTag)
        =>
        new(Invariant($"The step result tag must be {expectedTag} but it is {Tag}."));
}