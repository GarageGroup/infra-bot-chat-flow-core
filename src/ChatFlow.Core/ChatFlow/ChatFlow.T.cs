using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Bot.Builder;

public sealed partial class ChatFlow<T>
{
    private readonly ChatFlowEngine<T> chatFlowEngine;

    private readonly List<Func<IChatFlowContext<T>, CancellationToken, ValueTask<ChatFlowJump<T>>>> flowSteps = [];

    internal ChatFlow(ChatFlowEngine<T> chatFlowEngine)
        =>
        this.chatFlowEngine = chatFlowEngine;
}