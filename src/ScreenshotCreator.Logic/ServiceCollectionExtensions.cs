using Microsoft.Extensions.DependencyInjection;

namespace ScreenshotCreator.Logic;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddScreenshotCreatorLogicServices(this IServiceCollection services)
    {
        services.AddSingleton<IScreenshotCreator, ScreenshotCreator>();
        services.AddSingleton<IPlaywrightHelper, PlaywrightHelper>();
        services.AddSingleton<IImageProcessor, ImageProcessor>();
        return services;
    }
}
