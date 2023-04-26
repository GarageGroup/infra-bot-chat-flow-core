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
        var instanceId = await engineContext.ChatFlowCache.GetInstanceIdAsync(cancellationToken).ConfigureAwait(false);
        var jump = await InnerGetNextAsync(GetUnitJumpAsync, cancellationToken).ConfigureAwait(false);

        if (jump.Tag is not ChatFlowJumpTag.Repeat)
        {
            var clearPositionTask = engineContext.ChatFlowCache.ClearPositionAsync(cancellationToken);
            var clearInstanceIdTask = engineContext.ChatFlowCache.ClearInstanceIdAsync(cancellationToken);

            await Task.WhenAll(clearInstanceIdTask, clearPositionTask).ConfigureAwait(false);
        }

        return await jump.FoldValueAsync(ValueTask.FromResult, ToRepeatAsync, ToBreakAsync).ConfigureAwait(false);

        static ValueTask<ChatFlowJump<Unit>> GetUnitJumpAsync(IChatFlowContext<T> context, CancellationToken _)
            =>
            ChatFlowJump.Next(default(Unit)).InternalPipe(ValueTask.FromResult);

        static ValueTask<Unit> ToRepeatAsync(object? _)
            =>
            default;

        async ValueTask<Unit> ToBreakAsync(ChatFlowBreakState breakState)
        {
            if (string.IsNullOrEmpty(breakState.LogMessage) is false)
            {
                TrackEvent(instanceId, "Break", breakState.LogMessage);
                engineContext.Logger.LogError("{logMessage}", breakState.LogMessage);
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

            return default;
        }
    }
}