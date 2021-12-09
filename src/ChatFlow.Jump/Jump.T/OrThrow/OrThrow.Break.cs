using System;

namespace GGroupp.Infra.Bot.Builder;

partial struct ChatFlowJump<T>
{
    public ChatFlowBreakState BreakStateOrThrow()
        =>
        InnerBreakStateOrThrow(
            CreateTagMustBeBreakException);

    public ChatFlowBreakState BreakStateOrThrow(Func<Exception> exceptionFactory)
        =>
        InnerBreakStateOrThrow(
            exceptionFactory ?? throw new ArgumentNullException(nameof(exceptionFactory)));

    private ChatFlowBreakState InnerBreakStateOrThrow(Func<Exception> exceptionFactory)
        =>
        Tag is ChatFlowJumpTag.Break ? breakState : throw exceptionFactory.Invoke();

    private InvalidOperationException CreateTagMustBeBreakException()
        =>
        CreateUnexpectedTagException(ChatFlowJumpTag.Break);
}