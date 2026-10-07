using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Networks;
using FluentAssertions;
using FluentAssertions.Web;
using mu88.Shared.Testing.Docker;
using mu88.Shared.Testing.SystemTests;
using NUnit.Framework;

namespace Tests.System;

[TestFixture]
[Category("System")]
public class SystemTests : SystemTestsBase
{
    protected override string SubPath => "/screenshotCreator";

    protected override TimeSpan Timeout => TimeSpan.FromMinutes(5);

    [Test]
    public async Task CreateImageNowForOpenHabAndScreenshotCreatorBothRunningInDocker()
    {
        // Arrange
        var containerImageTag = DockerImageBuilder.GenerateContainerImageTag();
        await BuildDockerImageOfScreenshotCreatorAsync(containerImageTag, CancellationToken);
        Container = await StartScreenshotCreatorAndOpenHabInContainersAsync(containerImageTag, CancellationToken);

        // Act
        var appResponse = await HttpClient.GetAsync("createImageNow", CancellationToken);

        // Assert
        await LogsShouldNotContainWarningsAsync(CancellationToken);
        await HealthCheckShouldSucceedAsync(CancellationToken);
        await ScreenshotShouldBeValidAsync(appResponse);
    }

    private static async Task BuildDockerImageOfScreenshotCreatorAsync(string containerImageTag, CancellationToken cancellationToken)
    {
        var rootDirectory = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.Parent?.Parent ?? throw new NullReferenceException();
        var apiProjectFile = Path.Join(rootDirectory.FullName, "src", "ScreenshotCreator.Api", "ScreenshotCreator.Api.csproj");
        await DockerImageBuilder.BuildAsync(apiProjectFile, containerImageTag, "screenshotcreator-api", rootDirectory.FullName, cancellationToken);
    }

    private static IContainer BuildScreenshotCreatorContainer(INetwork network, string containerImageTag)
        => new ContainerBuilder($"screenshotcreator-api:{containerImageTag}")
            .WithNetwork(network)
            .WithEnvironment("ScreenshotOptions__Url", "http://openhab:8080/page/page_28d2e71d84") // must be hardcoded (both name and port)
            .WithEnvironment("ScreenshotOptions__UrlType", "OpenHab")
            .WithEnvironment("ScreenshotOptions__Username", "admin")
            .WithEnvironment("ScreenshotOptions__Password", "admin")
            .WithEnvironment("ScreenshotOptions__BackgroundProcessingEnabled", "false")
            .WithEnvironment("ScreenshotOptions__RefreshIntervalInSeconds", "300")
            .WithEnvironment("ScreenshotOptions__AvailabilityIndicator", "Wohnzimmer")
            .WithPortBinding(8080, true)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilExternalTcpPortIsAvailable(8080))
            .Build();

    private async Task<IContainer> StartScreenshotCreatorAndOpenHabInContainersAsync(string containerImageTag, CancellationToken cancellationToken)
    {
        Console.WriteLine("Building network and openHAB container");
        var network = new NetworkBuilder().Build();
        // when running in the same network and for container-to-container communication, the name 'openhab' MUST be used
        var openHabContainer = Shared.CreateOpenHabContainer(network, "openhab");
        Console.WriteLine("OpenHAB container created");

        Console.WriteLine("Starting network and openHAB container");
        await network.CreateAsync(cancellationToken);
        await openHabContainer.StartAsync(cancellationToken);
        Console.WriteLine("Network and openHAB container started");

        Console.WriteLine("Building and starting ScreenshotCreator container");
        var screenshotCreatorContainer = BuildScreenshotCreatorContainer(network, containerImageTag);
        await screenshotCreatorContainer.StartAsync(cancellationToken);
        await screenshotCreatorContainer.GetLogsAsync(ct: cancellationToken);
        Console.WriteLine("ScreenshotCreator container started");

        await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken); // give containers some time to settle

        return screenshotCreatorContainer;
    }

    private async Task ScreenshotShouldBeValidAsync(HttpResponseMessage appResponse)
    {
        appResponse.Should().Be200Ok();
        appResponse.Content.Headers.ContentType.Should().NotBeNull();
        appResponse.Content.Headers.ContentType!.MediaType.Should().Be("image/png");
        (await appResponse.Content.ReadAsByteArrayAsync(CancellationToken)).Length.Should().BeInRange(9000, 15000);
    }
}
