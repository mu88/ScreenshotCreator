using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using mu88.Shared.OpenTelemetry;
using Scalar.AspNetCore;
using ScreenshotCreator.Api;
using ScreenshotCreator.Logic;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureOpenTelemetry("screenshotcreator", builder.Configuration);

builder.Services.AddHealthChecks();
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Configuration
    .AddJsonFile("appsettings.secret.json", true)
    .AddKeyPerFile("/run/secrets", true);
builder.Services
    .AddOptions<ScreenshotOptions>()
    .Bind(builder.Configuration.GetSection(ScreenshotOptions.SectionName))
    .ValidateDataAnnotations()
    .ValidateOnStart();
builder.Services.AddScreenshotCreatorLogicServices();
builder.Services.AddHostedService<BackgroundScreenshotCreator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.UseDeveloperExceptionPage();
}

app.UsePathBase("/screenshotCreator");

app.MapGet("latestImage", ReturnImageOrNotFoundAsync);
app.MapGet("createImageNow",
    async (HttpContext httpContext, IImageProcessor imageProcessor, IScreenshotCreator creator, IOptions<ScreenshotOptions> options) =>
    {
        await creator.CreateScreenshotAsync(options.Value.Width, options.Value.Height, httpContext.RequestAborted);
        return await ReturnImageOrNotFoundAsync(httpContext, imageProcessor, options);
    });
app.MapGet("createImageWithSizeNow",
    async (
        uint width,
        uint height,
        HttpContext httpContext,
        IImageProcessor imageProcessor,
        IOptions<ScreenshotOptions> options,
        IScreenshotCreator creator) =>
    {
        if (width == 0 || height == 0)
        {
            return (IResult)TypedResults.ValidationProblem(new Dictionary<string, string[]>(StringComparer.Ordinal)
            {
                { nameof(width), ValidationErrorMessages.WidthMustBeGreaterThanZero },
                { nameof(height), ValidationErrorMessages.HeightMustBeGreaterThanZero }
            });
        }

        await creator.CreateScreenshotAsync(width, height, httpContext.RequestAborted);
        return (IResult)await ReturnImageOrNotFoundAsync(httpContext, imageProcessor, options);
    });
app.MapHealthChecks("/healthz");

await app.RunAsync();

async Task<Results<FileContentHttpResult, NotFound>> ReturnImageOrNotFoundAsync(
    HttpContext httpContext,
    IImageProcessor imageProcessor,
    IOptions<ScreenshotOptions> options,
    bool blackAndWhite = false,
    bool asWaveshareBytes = false,
    bool addWaveshareInstructions = false)
{
    if (!File.Exists(options.Value.ScreenshotFile))
    {
        return TypedResults.NotFound();
    }

    var processingResult = await imageProcessor.ProcessAsync(options.Value.ScreenshotFile, blackAndWhite, asWaveshareBytes);

    var result = asWaveshareBytes
        ? TypedResults.Bytes(processingResult.Data, processingResult.MediaType)
        : TypedResults.File(processingResult.Data, processingResult.MediaType);

    if (addWaveshareInstructions)
    {
        httpContext.Response.Headers.AddWaveshareInstructions(options.Value, options.Value.ScreenshotFile);
    }

    return result;
}
