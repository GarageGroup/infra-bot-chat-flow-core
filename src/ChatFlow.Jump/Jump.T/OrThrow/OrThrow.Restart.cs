using System;

namespace GarageGroup.Infra.Bot.Builder;

partial struct ChatFlowJump<T>
{
    public T RestartStateOrThrow()
        =>
        InnerRestartStateOrThrow(
            CreateTagMustBeRestartException);

    public T RestartStateOrThrow(Func<Exception> exceptionFactory)
        =>
        InnerRestartStateOrThrow(
            exceptionFactory ?? throw new ArgumentNullException(nameof(exceptionFactory)));

    private T InnerRestartStateOrThrow(Func<Exception> exceptionFactory)
        =>
        Tag is ChatFlowJumpTag.Restart ? nextState : throw exceptionFactory.Invoke();

    private InvalidOperationException CreateTagMustBeRestartException()
        =>
        CreateUnexpectedTagException(ChatFlowJumpTag.Restart);
}