using System;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Bot.Builder;

public static class ChatFlowJumpExtensions
{
    public static async Task<Unit> ToUnitTask<T>(this Task<ChatFlowJump<T>> sourceTask)
    {
        var _ = await sourceTask.ConfigureAwait(false);
        return default;
    }

    public static async ValueTask<Unit> ToUnitValueTask<T>(this ValueTask<ChatFlowJump<T>> sourceTask)
    {
        var _ = await sourceTask.ConfigureAwait(false);
        return default;
    }
}