using System.Net;
using DotNet.Testcontainers.Builders;
using FluentAssertions;
using Microsoft.AspNetCore.WebUtilities;

namespace Tests.Integration.Api;

[TestFixture]
[Category("Integration")]
public class ProgramTests : PlaywrightTests
{
    private HttpClient _clientForAny = null!;

    [SetUp]
    public void SetUp() => _clientForAny = new WebApplicationFactoryForAny().CreateClient();

    [TearDown]
    public void TearDown() => _clientForAny.Dispose();

    [Test]
    public async Task CreateImageNowForAny()
    {
        // Arrange & Act
        var result = await _clientForAny.GetAsync("createImageNow");

        // Assert
        result.Should().HaveStatusCode(HttpStatusCode.OK);
        result.Content.Headers.ContentType.Should().NotBeNull();
        result.Content.Headers.ContentType!.MediaType.Should().Be("image/png");
    }

    [Test]
    public async Task CreateImageNowForOpenHab()
    {
        // Arrange
        var mappedPublicPort = await StartLocalOpenHabContainerAndGetPortAsync();

        // Act
        var result = await new WebApplicationFactoryForOpenHab(mappedPublicPort).CreateClient().GetAsync("createImageNow");

        // Assert
        result.Should().HaveStatusCode(HttpStatusCode.OK);
        result.Content.Headers.ContentType.Should().NotBeNull();
        result.Content.Headers.ContentType!.MediaType.Should().Be("image/png");
        (await result.Content.ReadAsByteArrayAsync()).Length.Should().BeGreaterThan(2000).And.BeLessThan(25000);
    }

    [Test]
    public async Task CreateImageWithSizeNow()
    {
        // Arrange & Act
        var result = await _clientForAny.GetAsync(QueryHelpers.AddQueryString("createImageWithSizeNow",
                                                                              new Dictionary<string, string?> { { "width", "1024" }, { "height", "768" } }));

        // Assert
        result.Should().HaveStatusCode(HttpStatusCode.OK);
        result.Content.Headers.ContentType.Should().NotBeNull();
        result.Content.Headers.ContentType!.MediaType.Should().Be("image/png");
    }

    [Test]
    public async Task LatestImage()
    {
        // Arrange
        await _clientForAny.GetAsync("createImageNow");

        // Act
        var result = await _clientForAny.GetAsync("latestImage");

        // Assert
        result.Should().HaveStatusCode(HttpStatusCode.OK);
        result.Content.Headers.ContentType.Should().NotBeNull();
        result.Content.Headers.ContentType!.MediaType.Should().Be("image/png");
    }

    [Test]
    public async Task LatestImage_ShouldReturnBlackWhiteImage()
    {
        // Arrange
        await _clientForAny.GetAsync("createImageNow");

        // Act
        var result = await _clientForAny.GetAsync(QueryHelpers.AddQueryString("latestImage",
                                                                              new Dictionary<string, string?> { { "blackAndWhite", "true" } }));

        // Assert
        result.Should().HaveStatusCode(HttpStatusCode.OK);
        result.Content.Headers.ContentType.Should().NotBeNull();
        result.Content.Headers.ContentType!.MediaType.Should().Be("image/png");
    }

    [Test]
    public async Task LatestImage_ShouldReturnWaveshareBytes()
    {
        // Arrange
        await _clientForAny.GetAsync("createImageNow");

        // Act
        var result = await _clientForAny.GetAsync(QueryHelpers.AddQueryString("latestImage",
                                                                              new Dictionary<string, string?> { { "asWaveshareBytes", "true" } }));

        // Assert
        result.Should().HaveStatusCode(HttpStatusCode.OK);
        result.Content.Headers.ContentType.Should().NotBeNull();
        result.Content.Headers.ContentType!.MediaType.Should().Be("application/octet-stream");
    }

    [Test]
    public async Task LatestImage_ShouldAddWaveshareInstructions()
    {
        // Arrange
        await _clientForAny.GetAsync("createImageNow");

        // Act
        var result = await _clientForAny.GetAsync(QueryHelpers.AddQueryString("latestImage",
                                                                              new Dictionary<string, string?> { { "addWaveshareInstructions", "true" } }));

        // Assert
        result.Should().HaveStatusCode(HttpStatusCode.OK);
        result.Headers.Should().ContainKeys("waveshare-update-screen", "waveshare-sleep-between-updates", "waveshare-last-modified-local-time");
    }

    [Test]
    public async Task LatestImage_ShouldReturn404_IfNoImageExists()
    {
        // Arrange & Act
        var result = await _clientForAny.GetAsync("latestImage");

        // Assert
        result.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }

    private static async Task<ushort> StartLocalOpenHabContainerAndGetPortAsync()
    {
        var openHabNetwork = new NetworkBuilder().Build();
        var openHabContainer = new ContainerBuilder()
            .WithImage("openhab/openhab:latest")
            .WithNetwork(openHabNetwork)
            .WithPortBinding(8080, true)
            .WithResourceMapping(new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "testData", "openhab", "conf")), "/openhab/conf")
            .WithResourceMapping(new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "testData", "openhab", "userdata")), "/openhab/userdata")
            .WithWaitStrategy(Wait.ForUnixContainer()
                                  .UntilPortIsAvailable(8080)
                                  .UntilHttpRequestIsSucceeded(strategy => strategy
                                                                   .ForPort(8080)
                                                                   .ForStatusCode(HttpStatusCode.OK)))
            .Build();

        await openHabNetwork.CreateAsync();
        await openHabContainer.StartAsync();

        var mappedPublicPort = openHabContainer.GetMappedPublicPort(8080);

        return mappedPublicPort;
    }
}