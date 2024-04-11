using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Bot.Builder;

partial class ChatFlow<T>
{
    public ValueTask<ChatFlowJump<T>> GetFlowStateAsync(CancellationToken cancellationToken)
        =>
        chatFlowEngine.RunAsync(flowSteps, cancellationToken);
}