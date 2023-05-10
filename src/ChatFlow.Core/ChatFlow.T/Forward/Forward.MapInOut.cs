using System;

namespace GarageGroup.Infra.Bot.Builder;

partial class ChatFlow<T>
{
    public ChatFlow<TNext> Forward<TIn, TOut, TNext>(
        Func<T, TIn> mapFlowStateIn,
        Func<IChatFlowContext<TIn>, ChatFlowJump<TOut>> forward,
        Func<T, TOut, TNext> mapFlowStateOut)
        =>
        InnerForward(
            mapFlowStateIn ?? throw new ArgumentNullException(nameof(mapFlowStateIn)),
            forward ?? throw new ArgumentNullException(nameof(forward)),
            mapFlowStateOut ?? throw new ArgumentNullException(nameof(mapFlowStateOut)));

    private ChatFlow<TNext> InnerForward<TIn, TOut, TNext>(
        Func<T, TIn> mapFlowStateIn,
        Func<IChatFlowContext<TIn>, ChatFlowJump<TOut>> forward,
        Func<T, TOut, TNext> mapFlowStateOut)
        =>
        InnerForward(
            context => context.InternalMapFlowState(mapFlowStateIn).InternalPipe(forward).Forward<TNext>(
                @out => mapFlowStateOut.Invoke(context.FlowState, @out)));
}