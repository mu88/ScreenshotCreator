using System.Net;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Networks;

namespace Tests;

public static class Shared
{
    public static IContainer CreateOpenHabContainer(INetwork network, string? containerName = null) =>
        new ContainerBuilder()
            .WithImage("openhab/openhab:latest")
            .WithNetwork(network)
            .WithName(containerName)
            .WithPortBinding(8080, true)
            .WithResourceMapping(new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "testData", "openhab", "conf")), "/openhab/conf")
            .WithResourceMapping(new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "testData", "openhab", "userdata")), "/openhab/userdata")
            .WithWaitStrategy(Wait.ForUnixContainer()
                                  .UntilPortIsAvailable(8080)
                                  .UntilHttpRequestIsSucceeded(strategy => strategy
                                                                   .ForPort(8080)
                                                                   .ForStatusCode(HttpStatusCode.OK)))
            .Build();
}