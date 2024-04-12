using Newtonsoft.Json;

namespace GarageGroup.Infra.Bot.Builder;

internal sealed record class ChatFlowDataJson<T>
{
    [JsonProperty("position")]
    public int Position { get; init; }

    [JsonProperty("flowState")]
    public T? FlowState { get; init; }

    [JsonProperty("stepState")]
    public object? StepState { get; init; }
}