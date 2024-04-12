using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Bot.Builder;

partial class ChatFlowStarter<TFlowStateJson>
{
    public async ValueTask<bool> IsStartedAsync(CancellationToken cancellationToken)
        =>
        await flowEngineContext.ChatFlowStorage.GetAsync(cancellationToken).ConfigureAwait(false) is not null;
}