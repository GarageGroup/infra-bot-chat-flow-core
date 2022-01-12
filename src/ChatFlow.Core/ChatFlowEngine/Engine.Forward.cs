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
            chatFlowId: chatFlowId,
            stepPosition: stepPosition + 1,
            chatFlowCache: chatFlowCache,
            turnContext: turnContext,
            logger: logger,
            flowStep: token => token.IsCancellationRequested ? InnerCanceledAsync<TNext>(token) : InnerGetNextAsync(nextAsync, token));

    private async ValueTask<ChatFlowJump<TNext>> InnerGetNextAsync<TNext>(
        Func<IChatFlowContext<T>, CancellationToken, ValueTask<ChatFlowJump<TNext>>> nextAsync,
        CancellationToken token)
    {
        var nextPosition = stepPosition + 1;
        var postionFromCache = await chatFlowCache.GetPositionAsync(token).ConfigureAwait(false);

        if (nextPosition < postionFromCache)
        {
            return ChatFlowJump<TNext>.Next(default!);
        }

        if (nextPosition == postionFromCache)
        {
            var cache = await chatFlowCache.GetStepCacheAsync<T>(token).ConfigureAwait(false);
            var context = new ChatFlowContextImpl<T>(turnContext, logger, cache.FlowState!, cache.StepState);
            return await InnerGetNextJumpAsync(context).ConfigureAwait(false);
        }

        var jump = await TryGetCurrentJumpAsync().ConfigureAwait(false);
        return await jump.ForwardValueAsync(InnerNextStateAsync).ConfigureAwait(false);

        ValueTask<ChatFlowJump<TNext>> InnerNextStateAsync(T nextState)
        {
            var context = new ChatFlowContextImpl<T>(turnContext, logger, nextState, default);
            return InnerGetNextJumpAsync(context);
        }

        async ValueTask<ChatFlowJump<TNext>> InnerGetNextJumpAsync(IChatFlowContext<T> context)
        {
            var nextJump = await TryGetNextJumpAsync(context).ConfigureAwait(false);

            if (nextJump.Tag is ChatFlowJumpTag.Repeat)
            {
                var cache = new ChatFlowStepCacheJson<T>
                {
                    FlowState = context.FlowState,
                    StepState = nextJump.RepeatStateOrThrow()
                };
                _ = await chatFlowCache.SetStepCacheAsync(nextPosition, cache, token).ConfigureAwait(false);
            }
            else
            {
                _ = await chatFlowCache.ClearStepCacheAsync<T>(nextPosition, token).ConfigureAwait(false);
            }

            return nextJump;
        }

        async ValueTask<ChatFlowJump<T>> TryGetCurrentJumpAsync()
        {
            try
            {
                return await flowStep.Invoke(token).ConfigureAwait(false);
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
                return await nextAsync.Invoke(context, token).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return BreakFromException(ex);
            }
        }

        ChatFlowBreakState BreakFromException(Exception exception)
        {
            logger.LogError(exception, "An unexpected exception was thrown in the chat flow {chatFlowId}", chatFlowId);
            return ChatFlowBreakState.From("Произошла непредвиденная ошибка. Обратитесь к администратору или повторите позднее");
        }
    }

    private static ValueTask<ChatFlowJump<TNext>> InnerCanceledAsync<TNext>(CancellationToken token)
        =>
        ValueTask.FromCanceled<ChatFlowJump<TNext>>(token);
}