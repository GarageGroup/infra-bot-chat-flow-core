using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra.Bot.Builder;

internal sealed partial class ChatFlowEngine<T>
{
    private readonly string chatFlowId;

    private readonly int stepPosition;

    private readonly IChatFlowEngineContext engineContext;

    private readonly Func<CancellationToken, ValueTask<ChatFlowJump<T>>> flowStep;

    internal ChatFlowEngine(
        string chatFlowId,
        int stepPosition,
        IChatFlowEngineContext engineContext,
        Func<CancellationToken, ValueTask<ChatFlowJump<T>>> flowStep)
    {
        this.chatFlowId = chatFlowId;
        this.stepPosition = stepPosition;
        this.engineContext = engineContext;
        this.flowStep = flowStep;
    }

    private void TrackEvent(Guid instanceId, string eventName)
    {
        var properties = new Dictionary<string, string>
        {
            { "FlowId", chatFlowId },
            { "InstanceId", instanceId.ToString() },
            { "StepPosition", stepPosition.ToString() }
        };

        engineContext.BotTelemetryClient.TrackEvent(chatFlowId + eventName, properties);
    }
}