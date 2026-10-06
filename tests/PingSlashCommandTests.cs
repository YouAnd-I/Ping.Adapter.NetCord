using Ecs.Client;
using NetCord;
using Ping.Data;
using Xunit;

namespace Ping.Adapter.NetCord.Tests;

public class PingSlashCommandTests
{
    private sealed class StubWorld(object response) : IWorldClient
    {
        public object? Request { get; private set; }

        public Task<TResponse> AskAsync<TRequest, TResponse>(
            TRequest request,
            CancellationToken cancellationToken = default)
        {
            Request = request;
            return Task.FromResult((TResponse)response);
        }

        public IDisposable Subscribe<TNotification>(Func<TNotification, Task> handler) =>
            throw new NotSupportedException("ping never listens for notifications");
    }

    [Fact]
    public async Task Ping_AsksTheWorld_AndRepliesOnlyToTheCaller()
    {
        var world = new StubWorld(new PingResponse { Text = "Pong You!!" });

        var reply = await PingSlashCommand.HandleAsync(world);

        Assert.IsType<PingRequest>(world.Request);
        Assert.Equal("Pong You!!", reply.Data.Content);
        Assert.Equal(MessageFlags.Ephemeral, reply.Data.Flags);
    }
}
