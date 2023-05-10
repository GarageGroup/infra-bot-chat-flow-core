using System;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GarageGroup.Infra.Bot.Builder;

internal static class TelegramActivityExtensions
{
    private static readonly TelegramChannelData channelData;

    private static readonly JsonSerializer jsonSerializer;

    static TelegramActivityExtensions()
    {
        channelData = new();
        jsonSerializer = JsonSerializer.Create(new ()
        {
            NullValueHandling = NullValueHandling.Ignore
        });
    }

    internal static IActivity InternalSetTelegramRemoveKeyboardChannelData(this IActivity activity)
    {
        activity.ChannelData = JObject.FromObject(channelData, jsonSerializer);
        return activity;
    }

    internal static bool InternalIsTelegram(this Activity? activity)
        =>
        string.Equals(activity?.ChannelId, Channels.Telegram, StringComparison.InvariantCultureIgnoreCase);
}