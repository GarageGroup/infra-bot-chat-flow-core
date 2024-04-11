using System;

namespace GarageGroup.Infra.Bot.Builder;

partial class ChatFlow<T>
{
    public ChatFlow<T> Next(Func<IChatFlowContext<T>, T> nextAsync)
        =>
        InnerNext(
            nextAsync ?? throw new ArgumentNullException(nameof(nextAsync)));

    private ChatFlow<T> InnerNext(Func<IChatFlowContext<T>, T> next)
        =>
        InnerForward(
            context => context.InternalPipe(next).InternalPipe(ChatFlowJump.Next));
}