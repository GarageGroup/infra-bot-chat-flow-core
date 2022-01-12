using System;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra.Bot.Builder;

partial class ChatFlow<T>
{
    public ValueTask<Unit> CompleteValueAsync(CancellationToken cancellationToken)
        =>
        cancellationToken.IsCancellationRequested
        ? ValueTask.FromCanceled<Unit>(cancellationToken)
        : chatFlowEngine.InternalCompleteValueAsync(cancellationToken);
}