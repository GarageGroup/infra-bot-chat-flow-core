using Newtonsoft.Json;

namespace GarageGroup.Infra.Bot.Builder;

internal readonly record struct ChatFlowStepCacheJson<T>
{
    [JsonProperty("flowState")]
    public T? FlowState { get; init; }

    [JsonProperty("stepState")]
    public object? StepState { get; init; }
}