using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using ScreenshotCreator.Logic;

namespace Tests.Integration.Api;

internal class WebApplicationFactoryForAny : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder) =>
        builder.ConfigureTestServices(services => services.Configure<ScreenshotOptions>(options =>
        {
            options.Url = "https://www.dynamo-dresden.de";
            options.UrlType = UrlType.Any;
            options.BackgroundProcessingEnabled = false;
            options.ScreenshotFileName = $"Screenshot_{Guid.NewGuid()}.png";
            options.Activity = null;
            options.RefreshIntervalInSeconds = 1953;
        }));
}