using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using static System.FormattableString;

namespace GGroupp.Infra.Bot.Builder;

internal sealed class ChatFlowCacheImpl : IChatFlowCache
{
    private const int DefaultPosition = -1;

    private readonly string chatFlowId;

    private readonly ConversationState conversationState;

    private readonly ITurnContext turnContext;

    private readonly IStatePropertyAccessor<Guid> instanceIdAccessor;

    private readonly IStatePropertyAccessor<int> positionAccessor;

    private Guid? instanceIdCache;

    private int? positionCache;

    internal ChatFlowCacheImpl(string chatFlowId, ConversationState conversationState, ITurnContext turnContext)
    {
        this.chatFlowId = chatFlowId ?? string.Empty;
        this.conversationState = conversationState;
        this.turnContext = turnContext;
        instanceIdAccessor = conversationState.CreateProperty<Guid>($"__{chatFlowId}InstanceId");
        positionAccessor = conversationState.CreateProperty<int>($"__{chatFlowId}Position");
    }

    public async ValueTask<Guid> GetIsntanceIdAsync(CancellationToken cancellationToken)
    {
        if (instanceIdCache is null)
        {
            instanceIdCache = await instanceIdAccessor.GetAsync(turnContext, Guid.NewGuid, cancellationToken).ConfigureAwait(false);
        }

        return instanceIdCache.Value;
    }

    public Task ClearIsntanceIdAsync(CancellationToken cancellationToken)
    {
        instanceIdCache = null;
        return instanceIdAccessor.DeleteAsync(turnContext, cancellationToken);
    }

    public async ValueTask<int> GetPositionAsync(CancellationToken cancellationToken)
    {
        if (positionCache is null)
        {
            positionCache = await positionAccessor.GetAsync(turnContext, GetDefaultPosition, cancellationToken).ConfigureAwait(false);
        }

        return positionCache.Value;

        static int GetDefaultPosition() => DefaultPosition;
    }

    public Task ClearPositionAsync(CancellationToken cancellationToken)
    {
        positionCache = null;
        return positionAccessor.DeleteAsync(turnContext, cancellationToken);
    }

    public Task<ChatFlowStepCacheJson<T>> GetStepCacheAsync<T>(CancellationToken cancellationToken)
    {
        var accessor = CreateStepCacheAccessor<T>();
        return accessor.GetAsync(turnContext, GetDefaultStepCache, cancellationToken);

        static ChatFlowStepCacheJson<T> GetDefaultStepCache() => default;
    }

    public async ValueTask<Unit> ClearStepCacheAsync<T>(int position, CancellationToken cancellationToken)
    {
        var currentPosition = await GetPositionAsync(cancellationToken).ConfigureAwait(false);
        if (currentPosition != position)
        {
            return default;
        }

        var accessor = CreateStepCacheAccessor<T>();

        await accessor.DeleteAsync(turnContext, cancellationToken).ConfigureAwait(false);
        return default;
    }

    public async Task SetStepCacheAsync<T>(int position, ChatFlowStepCacheJson<T> cacheJson, CancellationToken cancellationToken)
    {
        positionCache = position;
        await positionAccessor.SetAsync(turnContext, position, cancellationToken).ConfigureAwait(false);

        var accessor = CreateStepCacheAccessor<T>();
        await accessor.SetAsync(turnContext, cacheJson, cancellationToken).ConfigureAwait(false);
    }

    private IStatePropertyAccessor<ChatFlowStepCacheJson<T>> CreateStepCacheAccessor<T>()
        =>
        conversationState.CreateProperty<ChatFlowStepCacheJson<T>>(Invariant($"__{chatFlowId}State"));
}