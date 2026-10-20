using System.Net;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Networks;
using mu88.Shared.Testing.Testcontainers;

namespace Tests;

public static class Shared
{
    private const string DockerfilePath = "testData/system/Dockerfile";

    public static IContainer CreateOpenHabContainer(INetwork network, string? containerName = null)
        => new ContainerBuilder(TestcontainerImages.GetImageFromDockerfile(DockerfilePath, "openhab"))
            .WithNetwork(network)
            .WithNetworkAliases(containerName)
            .WithPortBinding(8080, true)
            .WithResourceMapping(new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "testData", "openhab", "conf")), "/openhab/conf")
            .WithResourceMapping(new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "testData", "openhab", "userdata")), "/openhab/userdata")
            .WithWaitStrategy(Wait.ForUnixContainer()
                .UntilExternalTcpPortIsAvailable(8080)
                .UntilHttpRequestIsSucceeded(strategy => strategy
                    .ForPort(8080)
                    .ForStatusCode(HttpStatusCode.OK)))
            .Build();
}
