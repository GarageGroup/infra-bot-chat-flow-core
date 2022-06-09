using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Extensions.Logging;

namespace GGroupp.Infra.Bot.Builder;

partial class ChatFlowEngine<T>
{
    internal async ValueTask<Unit> InternalCompleteValueAsync(CancellationToken cancellationToken)
    {
        var instanceId = await engineContext.ChatFlowCache.GetIsntanceIdAsync(cancellationToken).ConfigureAwait(false);
        var jump = await InnerGetNextAsync(GetUnitJumpAsync, cancellationToken).ConfigureAwait(false);

        if (jump.Tag is not ChatFlowJumpTag.Repeat)
        {
            var clearPositionTask = engineContext.ChatFlowCache.ClearPositionAsync(cancellationToken);
            var clearIsntanceIdTask = engineContext.ChatFlowCache.ClearIsntanceIdAsync(cancellationToken);

            await Task.WhenAll(clearIsntanceIdTask, clearPositionTask).ConfigureAwait(false);
        }

        return await jump.FoldValueAsync(ToSuccessAsync, ToRepeatAsync, ToBreakAsync).ConfigureAwait(false);

        static ValueTask<ChatFlowJump<Unit>> GetUnitJumpAsync(IChatFlowContext<T> context, CancellationToken _)
            =>
            ChatFlowJump.Next(default(Unit)).InternalPipe(ValueTask.FromResult);

        ValueTask<Unit> ToSuccessAsync(Unit success)
        {
            TrackEvent(instanceId, "Complete");
            return new(success);
        }

        static ValueTask<Unit> ToRepeatAsync(object? _)
            =>
            default;

        async ValueTask<Unit> ToBreakAsync(ChatFlowBreakState breakState)
        {
            if (string.IsNullOrEmpty(breakState.LogMessage) is false)
            {
                var breakLogMessage = breakState.LogMessage;
                engineContext.Logger.LogError("{logMessage}", breakLogMessage);
            }

            if (string.IsNullOrEmpty(breakState.UserMessage) is false)
            {
                var breakMessage = MessageFactory.Text(breakState.UserMessage);

                if (engineContext.TurnContext.Activity.InternalIsTelegram())
                {
                    breakMessage.InternalSetTelegramRemoveKeyboardChannelData();
                }

                _ = await engineContext.TurnContext.SendActivityAsync(breakMessage, cancellationToken).ConfigureAwait(false);
            }

            TrackEvent(instanceId, "Break");
            return default;
        }
    }
}