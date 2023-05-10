using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Bot.Builder;

partial class ChatFlow<T>
{
    public ChatFlow<TNext> Forward<TOut, TNext>(
        Func<IChatFlowContext<T>, CancellationToken, Task<ChatFlowJump<TOut>>> forwardAsync,
        Func<T, TOut, TNext> mapFlowStateOut)
        =>
        InnerForward(
            forwardAsync ?? throw new ArgumentNullException(nameof(forwardAsync)),
            mapFlowStateOut ?? throw new ArgumentNullException(nameof(mapFlowStateOut)));

    private ChatFlow<TNext> InnerForward<TOut, TNext>(
        Func<IChatFlowContext<T>, CancellationToken, Task<ChatFlowJump<TOut>>> forwardAsync,
        Func<T, TOut, TNext> mapFlowStateOut)
        =>
        InnerForwardValue(
            async (context, token) =>
            {
                var jump = await forwardAsync.Invoke(context, token).ConfigureAwait(false);
                return jump.Forward<TNext>(@out => mapFlowStateOut.Invoke(context.FlowState, @out));
            });
}