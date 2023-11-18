using ScreenshotCreator.Logic;

namespace Tests.Integration.Api;

internal class WebApplicationFactoryForOpenHab : WebApplicationFactory
{
    /// <inheritdoc />
    public WebApplicationFactoryForOpenHab(int port, Action<ScreenshotOptions>? configureOptions = null)
        : base(configureOptions ?? (options =>
                                       {
                                           options.Url = $"http://127.0.0.1:{port}/page/page_b0d3c939f7";
                                           options.Username = "admin";
                                           options.Password = "admin";
                                           options.UrlType = UrlType.OpenHab;
                                           options.BackgroundProcessingEnabled = false;
                                           options.ScreenshotFileName = $"Screenshot_{Guid.NewGuid()}.png";
                                           options.Activity = null;
                                           options.RefreshIntervalInSeconds = 1953;
                                       }))
    {
    }
}