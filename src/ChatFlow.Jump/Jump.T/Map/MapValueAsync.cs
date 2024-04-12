using System;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Bot.Builder;

partial struct ChatFlowJump<T>
{
    public ValueTask<ChatFlowJump<T>> MapValueAsync(
        Func<T, ValueTask<T>> mapNextAsync,
        Func<T, ValueTask<T>>? mapRestartStateAsync = null,
        Func<object?, ValueTask<object?>>? mapRepeatStateAsync = null,
        Func<ChatFlowBreakState, ValueTask<ChatFlowBreakState>>? mapBreakStateAsync = null)
        =>
        InnerMapValueAsync(
            mapNextAsync ?? throw new ArgumentNullException(nameof(mapNextAsync)),
            mapRestartStateAsync,
            mapRepeatStateAsync,
            mapBreakStateAsync);

    private async ValueTask<ChatFlowJump<T>> InnerMapValueAsync(
        Func<T, ValueTask<T>> mapNextAsync,
        Func<T, ValueTask<T>>? mapRestartStateAsync,
        Func<object?, ValueTask<object?>>? mapRepeatStateAsync,
        Func<ChatFlowBreakState, ValueTask<ChatFlowBreakState>>? mapBreakStateAsync)
    {
        if (Tag is ChatFlowJumpTag.Next)
        {
            return new(await mapNextAsync.Invoke(nextState).ConfigureAwait(false));
        }

        if (Tag is ChatFlowJumpTag.Restart)
        {
            return mapRestartStateAsync is null ? this : new(await mapRestartStateAsync.Invoke(nextState).ConfigureAwait(false));
        }

        if (Tag is ChatFlowJumpTag.Repeat)
        {
            return mapRepeatStateAsync is null ? this : new(await mapRepeatStateAsync.Invoke(repeatState).ConfigureAwait(false));
        }

        return mapBreakStateAsync is null ? this : new(await mapBreakStateAsync.Invoke(breakState).ConfigureAwait(false));
    }
}