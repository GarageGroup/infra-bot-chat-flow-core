using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Bot.Builder;

partial class ChatFlow<T>
{
    public ChatFlow<T> AwaitChildFlow<TChildState>(
        Func<IChatFlowContext<T>, ChatFlow<TChildState>> childFlowFactory,
        Func<IChatFlowContext<T>, TChildState, T> mapFlowState)
    {
        ArgumentNullException.ThrowIfNull(childFlowFactory);
        ArgumentNullException.ThrowIfNull(mapFlowState);

        return InnerForwardValue(InnerRunChildFlowAsync);

        async ValueTask<ChatFlowJump<T>> InnerRunChildFlowAsync(IChatFlowContext<T> context, CancellationToken cancellationToken)
        {
            var childState = await childFlowFactory.Invoke(context).GetFlowStateAsync(cancellationToken).ConfigureAwait(false);

            return childState.Tag switch
            {
                ChatFlowJumpTag.Next => mapFlowState.Invoke(context, childState.NextStateOrThrow()),
                ChatFlowJumpTag.Repeat => context.RepeatSameStateJump(),
                _ => default
            };
        }
    }
}