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
        var jump = await InnerGetNextAsync(GetUnitJumpAsync, cancellationToken).ConfigureAwait(false);
        if (jump.Tag is not ChatFlowJumpTag.Repeat)
        {
            _ = await chatFlowCache.ClearPositionAsync(cancellationToken).ConfigureAwait(false);
        }

        return await jump.FoldValueAsync(ToUnitAsync, ToUnitAsync, CompleteWithBreakAsync).ConfigureAwait(false);

        static ValueTask<ChatFlowJump<Unit>> GetUnitJumpAsync(IChatFlowContext<T> context, CancellationToken _)
            =>
            ChatFlowJump.Next(default(Unit)).InternalPipe(ValueTask.FromResult);

        static ValueTask<Unit> ToUnitAsync<TAny>(TAny _)
            =>
            default;

        async ValueTask<Unit> CompleteWithBreakAsync(ChatFlowBreakState breakState)
        {
            if (string.IsNullOrEmpty(breakState.LogMessage) is false)
            {
                var breakLogMessage = breakState.LogMessage;
                logger.LogError("{logMessage}", breakLogMessage);
            }

            if (string.IsNullOrEmpty(breakState.UserMessage) is false)
            {
                var breakMessage = MessageFactory.Text(breakState.UserMessage);

                if (turnContext.Activity.InternalIsTelegram())
                {
                    breakMessage.InternalSetTelegramRemoveKeyboardChannelData();
                }

                _ = await turnContext.SendActivityAsync(breakMessage, cancellationToken).ConfigureAwait(false);
            }

            return default;
        }
    }
}