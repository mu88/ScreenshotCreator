using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
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
        var containerImageTag = GenerateContainerImageTag();
        var cancellationToken = CreateCancellationToken(TimeSpan.FromMinutes(5));
        await BuildDockerImageOfScreenshotCreatorAsync(containerImageTag, cancellationToken);
        var container = await StartScreenshotCreatorAndOpenHabInContainersAsync(containerImageTag, cancellationToken);
        var httpClient = new HttpClient { BaseAddress = GetScreenshotCreatorBaseAddress(container) };

        // Act
        var healthCheckResponse = await httpClient.GetAsync("healthz", cancellationToken);
        var appResponse = await httpClient.GetAsync("createImageNow", cancellationToken);
        var healthCheckToolResult =
            await container.ExecAsync(["dotnet", "/app/mu88.HealthCheck.dll", "http://127.0.0.1:8080/screenshotCreator/healthz"], cancellationToken);

        // Assert
        await LogsShouldNotContainWarningsAsync(container, cancellationToken);
        await HealthCheckShouldBeHealthyAsync(healthCheckResponse, cancellationToken);
        await AppShouldRunAsync(appResponse, cancellationToken);
        healthCheckToolResult.ExitCode.Should().Be(0);
    }

    private static CancellationToken CreateCancellationToken(TimeSpan timeout)
    {
        var timeoutCts = new CancellationTokenSource();
        timeoutCts.CancelAfter(timeout);
        var cancellationToken = timeoutCts.Token;

        return cancellationToken;
    }

    private static async Task BuildDockerImageOfScreenshotCreatorAsync(string containerImageTag, CancellationToken cancellationToken)
    {
        var rootDirectory = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.Parent?.Parent ?? throw new NullReferenceException();
        var apiProjectFile = Path.Join(rootDirectory.FullName, "src", "ScreenshotCreator.Api", "ScreenshotCreator.Api.csproj");
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments =
                    $"publish {apiProjectFile} --os linux --arch amd64 " +
                    $"/t:PublishContainersForMultipleFamilies " +
                    $"/p:ReleaseVersion={containerImageTag} " +
                    "/p:IsRelease=false " +
                    "/p:DoNotApplyGitHubScope=true", // ensures same behavior when run locally or in GitHub Actions
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            }
        };
        process.Start();
        while (await process.StandardOutput.ReadLineAsync(cancellationToken) is { } line)
        {
            Console.WriteLine(line);
        }

        await process.WaitForExitAsync(cancellationToken);
        process.ExitCode.Should().Be(0);
    }

    private static async Task<IContainer> StartScreenshotCreatorAndOpenHabInContainersAsync(string containerImageTag, CancellationToken cancellationToken)
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

    private static IContainer BuildScreenshotCreatorContainer(INetwork network, string containerImageTag) =>
        new ContainerBuilder()
            .WithImage($"screenshotcreator-api:{containerImageTag}")
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
                                  .UntilExternalTcpPortIsAvailable(8080))
            .Build();

    private static Uri GetScreenshotCreatorBaseAddress(IContainer screenshotCreatorContainer) =>
        new($"http://{screenshotCreatorContainer.Hostname}:{screenshotCreatorContainer.GetMappedPublicPort(8080)}/screenshotCreator");

    private static async Task AppShouldRunAsync(HttpResponseMessage appResponse, CancellationToken cancellationToken)
    {
        appResponse.Should().Be200Ok();
        appResponse.Content.Headers.ContentType.Should().NotBeNull();
        appResponse.Content.Headers.ContentType!.MediaType.Should().Be("image/png");
        (await appResponse.Content.ReadAsByteArrayAsync(cancellationToken)).Length.Should().BeInRange(9000, 15000);
    }

    private static async Task HealthCheckShouldBeHealthyAsync(HttpResponseMessage healthCheckResponse, CancellationToken cancellationToken)
    {
        healthCheckResponse.Should().Be200Ok();
        (await healthCheckResponse.Content.ReadAsStringAsync(cancellationToken)).Should().Be("Healthy");
    }

    private static async Task LogsShouldNotContainWarningsAsync(IContainer container, CancellationToken cancellationToken)
    {
        (string Stdout, string Stderr) logValues = await container.GetLogsAsync(ct: cancellationToken);
        Console.WriteLine($"Stderr:{Environment.NewLine}{logValues.Stderr}");
        Console.WriteLine($"Stdout:{Environment.NewLine}{logValues.Stdout}");
        logValues.Stdout.Should().NotContain("warn:");
    }

    [SuppressMessage("Design", "MA0076:Do not use implicit culture-sensitive ToString in interpolated strings", Justification = "Okay for me")]
    private static string GenerateContainerImageTag() => $"system-test-{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";
}