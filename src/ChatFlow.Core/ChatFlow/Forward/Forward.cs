using System;

namespace GarageGroup.Infra.Bot.Builder;

partial class ChatFlow<T>
{
    public ChatFlow<T> Forward(Func<IChatFlowContext<T>, ChatFlowJump<T>> forward)
        =>
        InnerForward(
            forward ?? throw new ArgumentNullException(nameof(forward)));

    private ChatFlow<T> InnerForward(Func<IChatFlowContext<T>, ChatFlowJump<T>> forward)
        =>
        InnerForwardValue(
            (context, _) => new(forward.Invoke(context)));
}