namespace GGroupp.Infra.Bot.Builder;

partial class ChatFlow<T>
{
    public ChatFlow<T> Await()
        =>
        InnerForward(
            context => context.StepState is null ? ChatFlowJump.Repeat<T>(new()) : context.FlowState);
}