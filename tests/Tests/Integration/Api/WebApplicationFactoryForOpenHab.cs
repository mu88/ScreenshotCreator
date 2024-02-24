using DotNet.Testcontainers.Containers;
using ScreenshotCreator.Logic;

namespace Tests.Integration.Api;

internal class WebApplicationFactoryForOpenHab : WebApplicationFactory
{
    /// <inheritdoc />
    public WebApplicationFactoryForOpenHab(IContainer openHabContainer, Action<ScreenshotOptions>? configureOptions = null)
        : base(configureOptions ?? (options =>
                                       {
                                           options.Url = $"http://{openHabContainer.Hostname}:{openHabContainer.GetMappedPublicPort(8080)}/page/page_28d2e71d84";
                                           options.Username = "admin";
                                           options.Password = "admin";
                                           options.UrlType = UrlType.OpenHab;
                                           options.BackgroundProcessingEnabled = false;
                                           options.ScreenshotFile = $"Screenshot_{Guid.NewGuid()}.png";
                                           options.AvailabilityIndicator = "Wohnzimmer";
                                           options.Activity = null;
                                           options.RefreshIntervalInSeconds = 1953;
                                       }))
    {
    }
}