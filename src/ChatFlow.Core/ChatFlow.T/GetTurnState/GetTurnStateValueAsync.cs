using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra.Bot.Builder;

partial class ChatFlow<T>
{
    public ValueTask<TurnState> GetTurnStateValueAsync(CancellationToken cancellationToken)
        =>
        cancellationToken.IsCancellationRequested
        ? ValueTask.FromCanceled<TurnState>(cancellationToken)
        : chatFlowEngine.InternalGetTurnStateValueAsync(cancellationToken);
}