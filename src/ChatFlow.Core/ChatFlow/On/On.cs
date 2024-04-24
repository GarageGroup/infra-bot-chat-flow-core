using System;

namespace GarageGroup.Infra.Bot.Builder;

partial class ChatFlow<T>
{
    public ChatFlow<T> On(Func<IChatFlowContext<T>, Unit> on)
    {
        ArgumentNullException.ThrowIfNull(on);
        return InnerOn(on);
    }

    public ChatFlow<T> On(Action<IChatFlowContext<T>> on)
    {
        ArgumentNullException.ThrowIfNull(on);
        return InnerOn(on);
    }

    private ChatFlow<T> InnerOn(Func<IChatFlowContext<T>, Unit> on)
    {
        return InnerNext(InnerGetNext);

        T InnerGetNext(IChatFlowContext<T> context)
        {
            _ = on.Invoke(context);
            return context.FlowState;
        }
    }

    private ChatFlow<T> InnerOn(Action<IChatFlowContext<T>> on)
    {
        return InnerNext(InnerGetNext);

        T InnerGetNext(IChatFlowContext<T> context)
        {
            on.Invoke(context);
            return context.FlowState;
        }
    }
}