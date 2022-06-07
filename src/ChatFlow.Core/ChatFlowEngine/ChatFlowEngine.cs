using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Extensions.Logging;

namespace GGroupp.Infra.Bot.Builder;

internal sealed partial class ChatFlowEngine<T>
{
    private readonly string chatFlowId;

    private readonly int stepPosition;

    private readonly IChatFlowCache chatFlowCache;

    private readonly ITurnContext turnContext;

    private readonly IBotUserProvider botUserProvider;

    private readonly ILogger logger;

    private readonly Func<CancellationToken, ValueTask<ChatFlowJump<T>>> flowStep;

    internal ChatFlowEngine(
        string chatFlowId,
        int stepPosition,
        IChatFlowCache chatFlowCache,
        ITurnContext turnContext,
        IBotUserProvider botUserProvider,
        ILogger logger,
        Func<CancellationToken, ValueTask<ChatFlowJump<T>>> flowStep)
    {
        this.chatFlowId = chatFlowId;
        this.stepPosition = stepPosition;
        this.chatFlowCache = chatFlowCache;
        this.turnContext = turnContext;
        this.botUserProvider = botUserProvider;
        this.logger = logger;
        this.flowStep = flowStep;
    }
}