using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using ScreenshotCreator.Logic;

namespace Tests.Integration.Api;

internal class WebApplicationFactoryForOpenHab : WebApplicationFactory<Program>
{
    private readonly int _port;

    /// <inheritdoc />
    public WebApplicationFactoryForOpenHab(int port) => _port = port;

    protected override void ConfigureWebHost(IWebHostBuilder builder) =>
        builder.ConfigureTestServices(services => services.Configure<ScreenshotOptions>(options =>
        {
            options.Url = $"http://127.0.0.1:{_port}/page/page_b0d3c939f7";
            options.Username = "admin";
            options.Password = "admin";
            options.UrlType = UrlType.OpenHab;
            options.BackgroundProcessingEnabled = false;
            options.ScreenshotFileName = $"Screenshot_{Guid.NewGuid()}.png";
            options.Activity = null;
            options.RefreshIntervalInSeconds = 1953;
        }));
}