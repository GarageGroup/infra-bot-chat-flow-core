using System;

namespace GarageGroup.Infra.Bot.Builder;

partial struct ChatFlowJump<T>
{
    public T NextStateOrThrow()
        =>
        InnerNextStateOrThrow(
            CreateTagMustBeNextException);

    public T NextStateOrThrow(Func<Exception> exceptionFactory)
        =>
        InnerNextStateOrThrow(
            exceptionFactory ?? throw new ArgumentNullException(nameof(exceptionFactory)));

    private T InnerNextStateOrThrow(Func<Exception> exceptionFactory)
        =>
        Tag is ChatFlowJumpTag.Next ? nextState : throw exceptionFactory.Invoke();

    private InvalidOperationException CreateTagMustBeNextException()
        =>
        CreateUnexpectedTagException(ChatFlowJumpTag.Next);
}