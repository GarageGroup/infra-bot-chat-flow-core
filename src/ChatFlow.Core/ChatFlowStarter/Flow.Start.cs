using System;

namespace GarageGroup.Infra.Bot.Builder;

partial class ChatFlowStarter<TFlowStateJson>
{
    public ChatFlow<TFlowStateJson> Start(Func<TFlowStateJson> initialFactory)
    {
        ArgumentNullException.ThrowIfNull(initialFactory);

        return new(
            chatFlowEngine: new(flowEngineContext, initialFactory));
    }
}