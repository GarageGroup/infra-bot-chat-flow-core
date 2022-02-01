using Newtonsoft.Json;

namespace GGroupp.Infra.Bot.Builder;

internal sealed record class TelegramParameters
{
    public TelegramParameters()
        =>
        ReplyMarkup = new();

    [JsonProperty("reply_markup")]
    public TelegramReplyKeyboardRemove ReplyMarkup { get; }
}