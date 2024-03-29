﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Bot.Builder;

internal sealed partial class ChatFlowEngine<T>
{
    private readonly int stepPosition;

    private readonly IChatFlowEngineContext engineContext;

    private readonly Func<CancellationToken, ValueTask<ChatFlowJump<T>>> flowStep;

    internal ChatFlowEngine(
        IChatFlowEngineContext engineContext,
        int stepPosition,
        Func<CancellationToken, ValueTask<ChatFlowJump<T>>> flowStep)
    {
        this.engineContext = engineContext;
        this.stepPosition = stepPosition;
        this.flowStep = flowStep;
    }

    private void TrackEvent(Guid instanceId, string eventName, string message, Exception? sourceException)
    {
        var properties = new Dictionary<string, string>
        {
            ["flowId"] = engineContext.ChatFlowId,
            ["instanceId"] = instanceId.ToString(),
            ["stepPosition"] = stepPosition.ToString(),
            ["event"] = eventName,
            ["message"] = message
        };

        if (sourceException is not null)
        {
            properties["errorMessage"] = sourceException.Message ?? string.Empty;
            properties["errorType"] = sourceException.GetType().FullName ?? string.Empty;
            properties["stackTrace"] = sourceException.StackTrace ?? string.Empty;
        }

        engineContext.BotTelemetryClient.TrackEvent(engineContext.ChatFlowId + eventName, properties);
    }
}