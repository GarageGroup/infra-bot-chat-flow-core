using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;

namespace GarageGroup.Infra.Bot.Builder;

internal sealed class ChatFlowStorage<T>
{
    private readonly ITurnContext turnContext;

    private readonly IStatePropertyAccessor<ChatFlowDataJson<T>?> dataAccessor;

    internal ChatFlowStorage(string chatFlowId, ConversationState conversationState, ITurnContext turnContext)
    {
        this.turnContext = turnContext;
        dataAccessor = conversationState.CreateProperty<ChatFlowDataJson<T>?>($"__{chatFlowId}Data");
    }

    public Task<ChatFlowDataJson<T>?> GetAsync(CancellationToken cancellationToken)
        =>
        dataAccessor.GetAsync(turnContext, default, cancellationToken);

    public Task SetAsync(ChatFlowDataJson<T> state, CancellationToken cancellationToken)
        =>
        dataAccessor.SetAsync(turnContext, state, cancellationToken);

    public Task DeleteAsync(CancellationToken cancellationToken)
        =>
        dataAccessor.DeleteAsync(turnContext, cancellationToken);
}