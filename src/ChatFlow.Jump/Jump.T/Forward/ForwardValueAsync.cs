using System;
using System.Threading.Tasks;

namespace GGroupp.Infra.Bot.Builder;

partial struct ChatFlowJump<T>
{
    public ValueTask<ChatFlowJump<TNextState>> ForwardValueAsync<TNextState>(
        Func<T, ValueTask<ChatFlowJump<TNextState>>> nextAsync)
        =>
        InnerForwardValueAsync(
            nextAsync ?? throw new ArgumentNullException(nameof(nextAsync)));

    private ValueTask<ChatFlowJump<TNextState>> InnerForwardValueAsync<TNextState>(
        Func<T, ValueTask<ChatFlowJump<TNextState>>> nextAsync)
        =>
        Tag switch
        {
            ChatFlowJumpTag.Next => nextAsync.Invoke(nextState),
            ChatFlowJumpTag.Repeat => new(new ChatFlowJump<TNextState>(repeatState)),
            _ => new(new ChatFlowJump<TNextState>(breakState))
        };
}