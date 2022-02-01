using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;

namespace GGroupp.Infra.Bot.Builder;

internal sealed class ChatFlowContextImpl<TFlowState> : IChatFlowContext<TFlowState>
{
    private readonly ITurnContext sourceContext;

    private TelegramKeyboardRemoveRule telegramKeyboardRemoveRule;

    internal ChatFlowContextImpl(
        ITurnContext sourceContext,
        ILogger logger,
        TFlowState flowState,
        object? stepState,
        TelegramKeyboardRemoveRule telegramKeyboardRemoveRule)
    {
        this.sourceContext = sourceContext;
        Logger = logger;
        FlowState = flowState;
        StepState = stepState;
        this.telegramKeyboardRemoveRule = telegramKeyboardRemoveRule;
    }

    IChatFlowContext<TResult> IChatFlowContext<TFlowState>.InternalMapFlowState<TResult>(Func<TFlowState, TResult> map)
        =>
        new ChatFlowContextImpl<TResult>(sourceContext, Logger, map.Invoke(FlowState), StepState, telegramKeyboardRemoveRule);

    public TFlowState FlowState { get; }

    public object? StepState { get; }

    public BotAdapter Adapter
        =>
        sourceContext.Adapter;

    public TurnContextStateCollection TurnState
        =>
        sourceContext.TurnState;

    public Activity Activity
        =>
        sourceContext.Activity;

    public bool Responded
        =>
        sourceContext.Responded;

    public ILogger Logger { get; set; }

    public Task DeleteActivityAsync(string activityId, CancellationToken cancellationToken = default)
        =>
        sourceContext.DeleteActivityAsync(activityId, cancellationToken);

    public Task DeleteActivityAsync(ConversationReference conversationReference, CancellationToken cancellationToken = default)
        =>
        sourceContext.DeleteActivityAsync(conversationReference, cancellationToken);

    public ITurnContext OnDeleteActivity(DeleteActivityHandler handler)
        =>
        sourceContext.OnDeleteActivity(handler);

    public ITurnContext OnSendActivities(SendActivitiesHandler handler)
        =>
        sourceContext.OnSendActivities(handler);

    public ITurnContext OnUpdateActivity(UpdateActivityHandler handler)
        =>
        sourceContext.OnUpdateActivity(handler);

    public Task<ResourceResponse[]> SendActivitiesAsync(IActivity[] activities, CancellationToken cancellationToken = default)
    {
        if (Activity.InternalIsTelegram() is false || telegramKeyboardRemoveRule is not TelegramKeyboardRemoveRule.WhenNextActivity)
        {
            return sourceContext.SendActivitiesAsync(activities, cancellationToken);
        }

        if (activities is null || activities.Length is 0)
        {
            return sourceContext.SendActivitiesAsync(activities, cancellationToken);
        }

        telegramKeyboardRemoveRule = TelegramKeyboardRemoveRule.None;

        var firstActivity = activities[0];
        if (firstActivity is null || firstActivity.ChannelData is not null)
        {
            return sourceContext.SendActivitiesAsync(activities, cancellationToken);
        }

        return sourceContext.SendActivitiesAsync(activities.Select(MapActivity).ToArray(), cancellationToken);

        static IActivity MapActivity(IActivity sourceActivity, int index)
            =>
            index is 0 ? sourceActivity.InternalSetTelegramRemoveKeyboardChannelData() : sourceActivity;
    }

    public Task<ResourceResponse> SendActivityAsync(
        string textReplyToSend,
        string? speak = null,
        string inputHint = "acceptingInput",
        CancellationToken cancellationToken = default)
        =>
        sourceContext.SendActivityAsync(textReplyToSend, speak, inputHint, cancellationToken);

    public Task<ResourceResponse> SendActivityAsync(IActivity activity, CancellationToken cancellationToken = default)
    {
        if (Activity.InternalIsTelegram() is false || telegramKeyboardRemoveRule is not TelegramKeyboardRemoveRule.WhenNextActivity)
        {
            return sourceContext.SendActivityAsync(activity, cancellationToken);
        }

        telegramKeyboardRemoveRule = TelegramKeyboardRemoveRule.None;

        if (activity.ChannelData is not null)
        {
            return sourceContext.SendActivityAsync(activity, cancellationToken);
        }

        return sourceContext.SendActivityAsync(activity.InternalSetTelegramRemoveKeyboardChannelData(), cancellationToken);
    }

    public Task<ResourceResponse> UpdateActivityAsync(IActivity activity, CancellationToken cancellationToken = default)
        =>
        sourceContext.UpdateActivityAsync(activity, cancellationToken);
}