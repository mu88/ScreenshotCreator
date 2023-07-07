using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.WebUtilities;

namespace Tests.Integration.Api;

[TestFixture]
[Category("Integration")]
public class ProgramTests
{
    private HttpClient _client = null!;

    [SetUp]
    public void SetUp()
    {
        var webApplicationFactory = new CustomWebApplicationFactory();
        _client = webApplicationFactory.CreateClient();
    }

    [Test]
    public async Task CreateImageNow()
    {
        // Arrange & Act
        var result = await _client.GetAsync("createImageNow");

        // Assert
        result.Should().HaveStatusCode(HttpStatusCode.OK);
        result.Content.Headers.ContentType.Should().NotBeNull();
        result.Content.Headers.ContentType!.MediaType.Should().Be("image/png");
    }

    [Test]
    public async Task CreateImageWithSizeNow()
    {
        // Arrange & Act
        var result = await _client.GetAsync(QueryHelpers.AddQueryString("createImageWithSizeNow",
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
        await _client.GetAsync("createImageNow");

        // Act
        var result = await _client.GetAsync("latestImage");

        // Assert
        result.Should().HaveStatusCode(HttpStatusCode.OK);
        result.Content.Headers.ContentType.Should().NotBeNull();
        result.Content.Headers.ContentType!.MediaType.Should().Be("image/png");
    }

    [Test]
    public async Task LatestImage_ShouldReturnBlackWhiteImage()
    {
        // Arrange
        await _client.GetAsync("createImageNow");

        // Act
        var result = await _client.GetAsync(QueryHelpers.AddQueryString("latestImage",
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
        await _client.GetAsync("createImageNow");

        // Act
        var result = await _client.GetAsync(QueryHelpers.AddQueryString("latestImage",
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
        await _client.GetAsync("createImageNow");

        // Act
        var result = await _client.GetAsync(QueryHelpers.AddQueryString("latestImage",
                                                                        new Dictionary<string, string?> { { "addWaveshareInstructions", "true" } }));

        // Assert
        result.Should().HaveStatusCode(HttpStatusCode.OK);
        result.Headers.Should().ContainKeys("waveshare-update-screen", "waveshare-sleep-between-updates", "waveshare-last-modified-local-time");
    }

    [Test]
    public async Task LatestImage_ShouldReturn404_IfNoImageExists()
    {
        // Arrange & Act
        var result = await _client.GetAsync("latestImage");

        // Assert
        result.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }
}