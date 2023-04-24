using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace GGroupp.Infra.Bot.Builder;

partial class ChatFlowEngine<T>
{
    internal ChatFlowEngine<TNext> InternalForwardValue<TNext>(
        Func<IChatFlowContext<T>, CancellationToken, ValueTask<ChatFlowJump<TNext>>> nextAsync)
        =>
        new(
            engineContext: engineContext,
            stepPosition: stepPosition + 1,
            flowStep: token => token.IsCancellationRequested ? InnerCanceledAsync<TNext>(token) : InnerGetNextAsync(nextAsync, token));

    private async ValueTask<ChatFlowJump<TNext>> InnerGetNextAsync<TNext>(
        Func<IChatFlowContext<T>, CancellationToken, ValueTask<ChatFlowJump<TNext>>> nextAsync,
        CancellationToken cancellationToken)
    {
        var instanceId = await engineContext.ChatFlowCache.GetInstanceIdAsync(cancellationToken).ConfigureAwait(false);

        var nextPosition = stepPosition + 1;
        var postionFromCache = await engineContext.ChatFlowCache.GetPositionAsync(cancellationToken).ConfigureAwait(false);

        if (nextPosition < postionFromCache)
        {
            return ChatFlowJump<TNext>.Next(default!);
        }

        if (nextPosition == postionFromCache)
        {
            var cache = await engineContext.ChatFlowCache.GetStepCacheAsync<T>(cancellationToken).ConfigureAwait(false);

            var context = new ChatFlowContextImpl<T>(
                engineContext.ChatFlowId,
                engineContext.TurnContext,
                engineContext.BotUserProvider,
                engineContext.Logger,
                engineContext.BotTelemetryClient,
                cache.FlowState!,
                cache.StepState,
                default);

            return await InnerGetNextJumpAsync(context).ConfigureAwait(false);
        }

        var jump = await TryGetCurrentJumpAsync().ConfigureAwait(false);
        return await jump.ForwardValueAsync(InnerNextStateAsync).ConfigureAwait(false);

        ValueTask<ChatFlowJump<TNext>> InnerNextStateAsync(T nextState)
        {
            var context = new ChatFlowContextImpl<T>(
                engineContext.ChatFlowId,
                engineContext.TurnContext,
                engineContext.BotUserProvider,
                engineContext.Logger,
                engineContext.BotTelemetryClient,
                nextState,
                default,
                TelegramKeyboardRemoveRule.WhenNextActivity);

            return InnerGetNextJumpAsync(context);
        }

        async ValueTask<ChatFlowJump<TNext>> InnerGetNextJumpAsync(IChatFlowContext<T> context)
        {
            TrackEvent(instanceId, "StepStart");
            var nextJump = await TryGetNextJumpAsync(context).ConfigureAwait(false);

            if (nextJump.Tag is ChatFlowJumpTag.Repeat)
            {
                var cache = new ChatFlowStepCacheJson<T>
                {
                    FlowState = context.FlowState,
                    StepState = nextJump.RepeatStateOrThrow()
                };
                await engineContext.ChatFlowCache.SetStepCacheAsync(nextPosition, cache, cancellationToken).ConfigureAwait(false);
                TrackEvent(instanceId, "StepToRepeat");
            }
            else
            {
                _ = await engineContext.ChatFlowCache.ClearStepCacheAsync<T>(nextPosition, cancellationToken).ConfigureAwait(false);
                TrackEvent(instanceId, "StepComplete");
            }

            return nextJump;
        }

        async ValueTask<ChatFlowJump<T>> TryGetCurrentJumpAsync()
        {
            try
            {
                return await flowStep.Invoke(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return BreakFromException(ex);
            }
        }

        async ValueTask<ChatFlowJump<TNext>> TryGetNextJumpAsync(IChatFlowContext<T> context)
        {
            try
            {
                return await nextAsync.Invoke(context, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return BreakFromException(ex);
            }
        }

        ChatFlowBreakState BreakFromException(Exception exception)
        {
            engineContext.Logger.LogError(exception, "An unexpected exception was thrown in the chat flow {chatFlowId}", engineContext.ChatFlowId);
            return ChatFlowBreakState.From(
                "Произошла непредвиденная ошибка. Обратитесь к администратору или повторите позднее",
                $"An unexpected exception {exception.GetType().FullName} was thrown: {exception.Message}");
        }
    }

    private static ValueTask<ChatFlowJump<TNext>> InnerCanceledAsync<TNext>(CancellationToken token)
        =>
        ValueTask.FromCanceled<ChatFlowJump<TNext>>(token);
}