using Discord.Ping.Data;
using Discord.Ping.System.Frent;
using Frent;
using Microsoft.Extensions.Hosting;
using NetCord;
using NetCord.Hosting.Services.ApplicationCommands;
using NetCord.Rest;

namespace Discord.Ping.System.NetCord;

public static class PingSlashCommand
{
    public static void AddPing(this IHost host, World world)
    {
        host.AddSlashCommand("ping", "Ping pong! (ECS)", () =>
        {
            var ping = world.Create(new PingRequestTag());
            PingSystem.Execute(world);
            var pong = ping.Get<PongResponse>();
            ping.Delete();
            return InteractionCallback.Message(new InteractionMessageProperties
            {
                Content = pong.Text,
                Flags = MessageFlags.Ephemeral,
            });
        });
    }
}
