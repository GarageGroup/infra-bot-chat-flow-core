using Newtonsoft.Json;

namespace GGroupp.Infra.Bot.Builder;

internal sealed record class TelegramChannelData
{
    public TelegramChannelData()
    {
        Method = "sendMessage";
        Parameters = new();
    }

    [JsonProperty("method")]
    public string? Method { get; }

    [JsonProperty("parameters")]
    public TelegramParameters Parameters { get; }
}