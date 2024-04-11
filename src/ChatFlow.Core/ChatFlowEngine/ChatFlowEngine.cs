using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Extensions.Logging;

namespace GarageGroup.Infra.Bot.Builder;

internal sealed class ChatFlowEngine<T>(ChatFlowEngineContext<T> context, Func<T> initialFactory)
{
    private const int MaxRestartCount = 10;

    private const string BreakEventName = "Break";

    internal ValueTask<ChatFlowJump<T>> RunAsync(
        IReadOnlyList<Func<IChatFlowContext<T>, CancellationToken, ValueTask<ChatFlowJump<T>>>> flowSteps, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return ValueTask.FromCanceled<ChatFlowJump<T>>(cancellationToken);
        }

        return InnerRunAsync(flowSteps, cancellationToken);
    }

    private async ValueTask<ChatFlowJump<T>> InnerRunAsync(
        IReadOnlyList<Func<IChatFlowContext<T>, CancellationToken, ValueTask<ChatFlowJump<T>>>> flowSteps, CancellationToken cancellationToken)
    {
        var flowState = await context.ChatFlowStorage.GetAsync(cancellationToken).ConfigureAwait(false) ?? GetInitialFlowData();

        var currentPosition = flowState.Position;
        var currentJump = new ChatFlowJump<T>(flowState.FlowState!);

        var restartCount = 0;

        while (currentPosition < flowSteps.Count)
        {
            var flowContext = BuildChatFlowContext(flowState);
            currentJump = await InvokeOrBreakAsync(flowSteps[currentPosition], flowContext, cancellationToken).ConfigureAwait(false);

            if (currentJump.Tag is ChatFlowJumpTag.Break)
            {
                await OnBreakAsync(currentPosition, currentJump.BreakStateOrThrow(), cancellationToken).ConfigureAwait(false);
                break;
            }

            if (currentJump.Tag is ChatFlowJumpTag.Next)
            {
                currentPosition++;

                flowState = new()
                {
                    Position = currentPosition,
                    FlowState = currentJump.NextStateOrThrow(),
                    StepState = null
                };
            }
            else if (currentJump.Tag is ChatFlowJumpTag.Restart)
            {
                currentPosition = 0;
    
                flowState = new()
                {
                    Position = currentPosition,
                    FlowState = currentJump.RestartStateOrThrow(),
                    StepState = null
                };

                if (restartCount >= MaxRestartCount)
                {
                    throw new InvalidOperationException($"Maximum number of restarts exceeded: {MaxRestartCount}");
                }

                restartCount++;
            }
            else if (currentJump.Tag is ChatFlowJumpTag.Repeat)
            {
                flowState = flowState with
                {
                    StepState = currentJump.RepeatStateOrThrow()
                };

                await context.ChatFlowStorage.SetAsync(flowState, cancellationToken).ConfigureAwait(false);
                return currentJump;
            }
        };

        await context.ChatFlowStorage.DeleteAsync(cancellationToken).ConfigureAwait(false);
        return currentJump;

        ChatFlowDataJson<T> GetInitialFlowData()
            =>
            new()
            {
                Position = 0,
                FlowState = initialFactory.Invoke(),
                StepState = null
            };
    }

    private ChatFlowContextImpl<T> BuildChatFlowContext(ChatFlowDataJson<T> flowData)
        =>
        new(
            chatFlowId: context.ChatFlowId,
            sourceContext: context.TurnContext,
            botUserProvider: context.BotUserProvider,
            logger: context.Logger,
            botTelemetryClient: context.BotTelemetryClient,
            flowState: flowData.FlowState!,
            stepState: flowData.StepState,
            telegramKeyboardRemoveRule: TelegramKeyboardRemoveRule.WhenNextActivity);

    private static async ValueTask<ChatFlowJump<T>> InvokeOrBreakAsync(
        Func<IChatFlowContext<T>, CancellationToken, ValueTask<ChatFlowJump<T>>> nextAsync,
        IChatFlowContext<T> context,
        CancellationToken cancellationToken)
    {
        try
        {
            return await nextAsync.Invoke(context, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            return exception.ToChatFlowBreakState(
                userMessage: "An unexpected error has occurred. Contact your administrator or try again later",
                logMessage: $"An unexpected exception {exception.GetType().FullName} was thrown: {exception.Message}");
        }
    }

    private async Task OnBreakAsync(int stepPosition, ChatFlowBreakState breakState, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(breakState.LogMessage) is false || breakState.SourceException is not null)
        {
            TrackEvent(stepPosition, breakState.LogMessage, breakState.SourceException);
            context.Logger.LogError(breakState.SourceException, "{logMessage}", breakState.LogMessage);
        }

        if (string.IsNullOrEmpty(breakState.UserMessage) is false)
        {
            var breakMessage = MessageFactory.Text(breakState.UserMessage);

            if (context.TurnContext.Activity.InternalIsTelegram())
            {
                breakMessage.InternalSetTelegramRemoveKeyboardChannelData();
            }

            _ = await context.TurnContext.SendActivityAsync(breakMessage, cancellationToken).ConfigureAwait(false);
        }
    }

    private void TrackEvent(int stepPosition, string message, Exception? sourceException)
    {
        var properties = new Dictionary<string, string>
        {
            ["flowId"] = context.ChatFlowId,
            ["stepPosition"] = stepPosition.ToString(),
            ["event"] = BreakEventName,
            ["message"] = message
        };

        if (sourceException is not null)
        {
            properties["errorMessage"] = sourceException.Message ?? string.Empty;
            properties["errorType"] = sourceException.GetType().FullName ?? string.Empty;
            properties["stackTrace"] = sourceException.StackTrace ?? string.Empty;
        }

        context.BotTelemetryClient.TrackEvent(context.ChatFlowId + BreakEventName, properties);
    }
}