using System;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Bot.Builder;

partial struct ChatFlowJump<T>
{
    public Task<ChatFlowJump<T>> MapAsync(
        Func<T, Task<T>> mapNextAsync,
        Func<T, Task<T>>? mapRestartStateAsync = null,
        Func<object?, Task<object?>>? mapRepeatStateAsync = null,
        Func<ChatFlowBreakState, Task<ChatFlowBreakState>>? mapBreakStateAsync = null)
        =>
        InnerMapAsync(
            mapNextAsync ?? throw new ArgumentNullException(nameof(mapNextAsync)),
            mapRestartStateAsync,
            mapRepeatStateAsync,
            mapBreakStateAsync);

    private async Task<ChatFlowJump<T>> InnerMapAsync(
        Func<T, Task<T>> mapNextAsync,
        Func<T, Task<T>>? mapRestartStateAsync,
        Func<object?, Task<object?>>? mapRepeatStateAsync,
        Func<ChatFlowBreakState, Task<ChatFlowBreakState>>? mapBreakStateAsync)
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