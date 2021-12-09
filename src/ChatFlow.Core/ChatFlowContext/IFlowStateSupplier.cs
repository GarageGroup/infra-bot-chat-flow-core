namespace GGroupp.Infra.Bot.Builder;

public interface IFlowStateSupplier<T>
{
    public T FlowState { get; }
}