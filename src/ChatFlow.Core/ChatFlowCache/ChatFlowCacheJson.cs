using System;
using Newtonsoft.Json;

namespace GGroupp.Infra.Bot.Builder;

internal readonly record struct ChatFlowCacheJson
{
    [JsonProperty("instanceId")]
    public Guid? InstanceId { get; init; }

    [JsonProperty("position")]
    public int? Position { get; init; }
}