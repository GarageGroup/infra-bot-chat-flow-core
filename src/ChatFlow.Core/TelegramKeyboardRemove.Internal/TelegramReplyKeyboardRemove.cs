using Newtonsoft.Json;

namespace GarageGroup.Infra.Bot.Builder;

internal sealed record class TelegramReplyKeyboardRemove
{
    public TelegramReplyKeyboardRemove()
        =>
        RemoveKeyboard = true;

    [JsonProperty("remove_keyboard")]
    public bool RemoveKeyboard { get; }
}