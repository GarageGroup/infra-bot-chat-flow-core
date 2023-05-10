﻿using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Bot.Builder;

partial class ChatFlow
{
    public ValueTask<bool> IsStartedAsync(CancellationToken cancellationToken)
        =>
        cancellationToken.IsCancellationRequested
        ? ValueTask.FromCanceled<bool>(cancellationToken)
        : InnerIsStartedAsync(cancellationToken);

    private async ValueTask<bool> InnerIsStartedAsync(CancellationToken cancellationToken)
        =>
        await chatFlowCache.GetPositionAsync(cancellationToken).ConfigureAwait(false) >= default(int);
}