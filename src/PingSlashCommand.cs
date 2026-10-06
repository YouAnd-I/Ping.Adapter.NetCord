using Ecs.Client;
using Microsoft.Extensions.Hosting;
using NetCord;
using NetCord.Hosting.Services.ApplicationCommands;
using NetCord.Rest;
using Ping.Data;

namespace Ping.Adapter.NetCord;

public static class PingSlashCommand
{
    public static void AddPing(this IHost host, IWorldClient world) =>
        host.AddSlashCommand("ping", "Ping pong! (ECS)", () => HandleAsync(world));

    public static async Task<InteractionCallbackProperties<InteractionMessageProperties>> HandleAsync(
        IWorldClient world)
    {
        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(2));
        var pong = await world.AskAsync<PingRequest, PingResponse>(new PingRequest(), timeout.Token);

        return InteractionCallback.Message(new InteractionMessageProperties
        {
            Content = pong.Text,
            Flags = MessageFlags.Ephemeral,
        });
    }
}
