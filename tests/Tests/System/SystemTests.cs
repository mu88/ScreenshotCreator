using System.Diagnostics;
using System.Net;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Networks;
using FluentAssertions;

namespace Tests.System;

[TestFixture]
[Category("System")]
public class SystemTests
{
    [Test]
    public async Task CreateImageNowForOpenHabAndScreenshotCreatorBothRunningInDocker()
    {
        // Arrange
        await BuildDockerImageOfScreenshotCreatorAsync();
        var screenshotCreatorContainer = await StartScreenshotCreatorAndOpenHabInContainersAsync();
        var httpClient = new HttpClient { BaseAddress = GetScreenshotCreatorBaseAddress(screenshotCreatorContainer) };

        // Act
        var healthCheckResponse = await httpClient.GetAsync("healthz");
        var appResponse = await httpClient.GetAsync("createImageNow");

        // Assert
        var logValues = await screenshotCreatorContainer.GetLogsAsync();
        Console.WriteLine($"Stderr:{Environment.NewLine}{logValues.Stderr}");
        Console.WriteLine($"Stdout:{Environment.NewLine}{logValues.Stdout}");
        logValues.Stdout.Should().NotContain("warn:");
        healthCheckResponse.Should().BeSuccessful();
        (await healthCheckResponse.Content.ReadAsStringAsync()).Should().Be("Healthy");
        appResponse.Should().HaveStatusCode(HttpStatusCode.OK);
        appResponse.Content.Headers.ContentType.Should().NotBeNull();
        appResponse.Content.Headers.ContentType!.MediaType.Should().Be("image/png");
        (await appResponse.Content.ReadAsByteArrayAsync()).Length.Should().BeInRange(9000, 15000);
    }

    private static async Task BuildDockerImageOfScreenshotCreatorAsync()
    {
        var rootDirectory = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.Parent?.Parent ?? throw new NullReferenceException();
        var apiProjectFile = Path.Join(rootDirectory.FullName, "src", "ScreenshotCreator.Api", "ScreenshotCreator.Api.csproj");
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"publish {apiProjectFile} --os linux --arch amd64 /t:PublishContainer",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            }
        };
        process.Start();
        while (!process.StandardOutput.EndOfStream)
        {
            Console.WriteLine(await process.StandardOutput.ReadLineAsync());
        }

        await process.WaitForExitAsync();
        process.ExitCode.Should().Be(0);
    }

    private static async Task<IContainer> StartScreenshotCreatorAndOpenHabInContainersAsync()
    {
        Console.WriteLine("Building network and openHAB container");
        var network = new NetworkBuilder().Build();
        // when running in the same network and for container-to-container communication, the name 'openhab' MUST be used 
        var openHabContainer = Shared.CreateOpenHabContainer(network, "openhab");
        Console.WriteLine("OpenHAB container created");

        Console.WriteLine("Starting network and openHAB container");
        await network.CreateAsync();
        await openHabContainer.StartAsync();
        Console.WriteLine("Network and openHAB container started");

        Console.WriteLine("Building and starting ScreenshotCreator container");
        var screenshotCreatorContainer = BuildScreenshotCreatorContainer(network);
        await screenshotCreatorContainer.StartAsync();
        await screenshotCreatorContainer.GetLogsAsync();
        Console.WriteLine("ScreenshotCreator container started");

        return screenshotCreatorContainer;
    }

    private static IContainer BuildScreenshotCreatorContainer(INetwork network) =>
        new ContainerBuilder()
            .WithImage("mu88/screenshotcreator:latest")
            .WithNetwork(network)
            .WithEnvironment("ScreenshotOptions__Url", "http://openhab:8080/page/page_28d2e71d84") // must be hardcoded (both name and port)
            .WithEnvironment("ScreenshotOptions__UrlType", "OpenHab")
            .WithEnvironment("ScreenshotOptions__Username", "admin")
            .WithEnvironment("ScreenshotOptions__Password", "admin")
            .WithEnvironment("ScreenshotOptions__BackgroundProcessingEnabled", "false")
            .WithEnvironment("ScreenshotOptions__RefreshIntervalInSeconds", "300")
            .WithEnvironment("ScreenshotOptions__AvailabilityIndicator", "Wohnzimmer")
            .WithPortBinding(8080, true)
            .WithWaitStrategy(Wait.ForUnixContainer()
                                  .UntilPortIsAvailable(8080))
            .Build();

    private static Uri GetScreenshotCreatorBaseAddress(IContainer screenshotCreatorContainer) =>
        new($"http://{screenshotCreatorContainer.Hostname}:{screenshotCreatorContainer.GetMappedPublicPort(8080)}/screenshotCreator");
}