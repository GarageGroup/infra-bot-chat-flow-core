using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Bot.Builder;

partial class ChatFlow<T>
{
    public ChatFlow<TNext> ForwardValue<TIn, TOut, TNext>(
        Func<T, TIn> mapFlowStateIn,
        Func<IChatFlowContext<TIn>, CancellationToken, ValueTask<ChatFlowJump<TOut>>> forwardAsync,
        Func<T, TOut, TNext> mapFlowStateOut)
        =>
        InnerForwardValue(
            mapFlowStateIn ?? throw new ArgumentNullException(nameof(mapFlowStateIn)),
            forwardAsync ?? throw new ArgumentNullException(nameof(forwardAsync)),
            mapFlowStateOut ?? throw new ArgumentNullException(nameof(mapFlowStateOut)));

    private ChatFlow<TNext> InnerForwardValue<TIn, TOut, TNext>(
        Func<T, TIn> mapFlowStateIn,
        Func<IChatFlowContext<TIn>, CancellationToken, ValueTask<ChatFlowJump<TOut>>> forwardAsync,
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