using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Extensions.Logging;

namespace GGroupp.Infra.Bot.Builder;

partial class ChatFlowEngine<T>
{
    internal async ValueTask<TurnState> InternalGetTurnStateValueAsync(CancellationToken cancellationToken)
    {
        var jump = await InnerGetNextAsync(GetUnitJumpAsync, cancellationToken).ConfigureAwait(false);
        if (jump.Tag is not ChatFlowJumpTag.Repeat)
        {
            _ = await chatFlowCache.ClearPositionAsync(cancellationToken).ConfigureAwait(false);
        }

        return await jump.FoldValueAsync(NextTurnStateAsync, RepeatTurnStateAsync, BreakTurnStateAsync).ConfigureAwait(false);

        static ValueTask<ChatFlowJump<Unit>> GetUnitJumpAsync(IChatFlowContext<T> context, CancellationToken _)
            =>
            ChatFlowJump.Next(default(Unit)).InternalPipe(ValueTask.FromResult);

        static ValueTask<TurnState> NextTurnStateAsync(Unit _)
            =>
            ValueTask.FromResult(TurnState.Completed);

        static ValueTask<TurnState> RepeatTurnStateAsync(object? _)
            =>
            ValueTask.FromResult(TurnState.Awaiting);

        async ValueTask<TurnState> BreakTurnStateAsync(ChatFlowBreakState breakState)
        {
            if (string.IsNullOrEmpty(breakState.LogMessage) is false)
            {
                logger.LogError(breakState.LogMessage);
            }
            if (string.IsNullOrEmpty(breakState.UIMessage) is false)
            {
                var breakMessage = MessageFactory.Text(breakState.UIMessage);
                _ = await turnContext.SendActivityAsync(breakMessage, cancellationToken).ConfigureAwait(false);
            }

            return breakState.Type is ChatFlowBreakType.Cancel ? TurnState.Canceled : TurnState.Interrupted;
        }
    }
}