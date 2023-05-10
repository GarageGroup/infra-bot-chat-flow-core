using System;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Bot.Builder;

partial class ChatFlow<T>
{
    public ChatFlow<TNext> Forward<TNext>(Func<IChatFlowContext<T>, ChatFlowJump<TNext>> forward)
        =>
        InnerForward(
            forward ?? throw new ArgumentNullException(nameof(forward)));

    private ChatFlow<TNext> InnerForward<TNext>(Func<IChatFlowContext<T>, ChatFlowJump<TNext>> forward)
        =>
        chatFlowEngine.InternalForwardValue(
            (context, _) => context.InternalPipe(forward).InternalPipe(ValueTask.FromResult))
        .ToChatFlow();
}