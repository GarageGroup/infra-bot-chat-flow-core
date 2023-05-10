namespace GarageGroup.Infra.Bot.Builder;

partial class ChatFlowEngine<T>
{
    public ChatFlow<T> ToChatFlow() => new(this);
}