using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using ScreenshotCreator.Logic;

namespace Tests.Integration.Api;

internal class WebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly Action<ScreenshotOptions> _configureOptions;

    protected WebApplicationFactory(Action<ScreenshotOptions> configureOptions) => _configureOptions = configureOptions;

    protected override void ConfigureWebHost(IWebHostBuilder builder) => builder.ConfigureTestServices(services => services.Configure(_configureOptions));
}