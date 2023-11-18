using ScreenshotCreator.Logic;

namespace Tests.Integration.Api;

internal class WebApplicationFactoryForAny : WebApplicationFactory
{
    /// <inheritdoc />
    public WebApplicationFactoryForAny(Action<ScreenshotOptions>? configureOptions = null)
        : base(configureOptions ?? (options =>
                                       {
                                           options.Url = "https://www.google.com";
                                           options.UrlType = UrlType.Any;
                                           options.BackgroundProcessingEnabled = false;
                                           options.ScreenshotFileName = $"Screenshot_{Guid.NewGuid()}.png";
                                           options.Activity = null;
                                           options.RefreshIntervalInSeconds = 1953;
                                       }))
    {
    }
}