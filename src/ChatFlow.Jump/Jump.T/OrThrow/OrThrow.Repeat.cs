using System;

namespace GarageGroup.Infra.Bot.Builder;

partial struct ChatFlowJump<T>
{
    public object? RepeatStateOrThrow()
        =>
        InnerRepeatStateOrThrow(
            CreateTagMustBeRepeatException);

    public object? RepeatStateOrThrow(Func<Exception> exceptionFactory)
        =>
        InnerRepeatStateOrThrow(
            exceptionFactory ?? throw new ArgumentNullException(nameof(exceptionFactory)));

    private object? InnerRepeatStateOrThrow(Func<Exception> exceptionFactory)
        =>
        Tag is ChatFlowJumpTag.Repeat ? repeatState : throw exceptionFactory.Invoke();

    private InvalidOperationException CreateTagMustBeRepeatException()
        =>
        CreateUnexpectedTagException(ChatFlowJumpTag.Repeat);
}