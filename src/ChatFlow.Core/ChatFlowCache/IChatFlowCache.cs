using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Bot.Builder;

internal interface IChatFlowCache
{
    ValueTask<Guid> GetInstanceIdAsync(CancellationToken cancellationToken);

    Task ClearInstanceIdAsync(CancellationToken cancellationToken);

    ValueTask<int> GetPositionAsync(CancellationToken cancellationToken);

    Task ClearPositionAsync(CancellationToken cancellationToken);

    Task<ChatFlowStepCacheJson<T>> GetStepCacheAsync<T>(CancellationToken cancellationToken);

    ValueTask<Unit> ClearStepCacheAsync<T>(int position, CancellationToken cancellationToken);

    Task SetStepCacheAsync<T>(int position, ChatFlowStepCacheJson<T> cacheJson, CancellationToken cancellationToken);
}