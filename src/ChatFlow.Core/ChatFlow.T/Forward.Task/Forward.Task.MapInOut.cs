using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Bot.Builder;

partial class ChatFlow<T>
{
    public ChatFlow<TNext> Forward<TIn, TOut, TNext>(
        Func<T, TIn> mapFlowStateIn,
        Func<IChatFlowContext<TIn>, CancellationToken, Task<ChatFlowJump<TOut>>> forwardAsync,
        Func<T, TOut, TNext> mapFlowStateOut)
        =>
        InnerForward(
            mapFlowStateIn ?? throw new ArgumentNullException(nameof(mapFlowStateIn)),
            forwardAsync ?? throw new ArgumentNullException(nameof(forwardAsync)),
            mapFlowStateOut ?? throw new ArgumentNullException(nameof(mapFlowStateOut)));

    private ChatFlow<TNext> InnerForward<TIn, TOut, TNext>(
        Func<T, TIn> mapFlowStateIn,
        Func<IChatFlowContext<TIn>, CancellationToken, Task<ChatFlowJump<TOut>>> forwardAsync,
        Func<T, TOut, TNext> mapFlowStateOut)
        =>
        InnerForwardValue(
            async (context, token) =>
            {
                var mappedContext = context.InternalMapFlowState(mapFlowStateIn);
                var jump = await forwardAsync.Invoke(mappedContext, token).ConfigureAwait(false);
                return jump.Forward<TNext>(@out => mapFlowStateOut.Invoke(context.FlowState, @out));
            });
}