using Microsoft.Extensions.Options;
using mu88.Shared.OpenTelemetry;
using ScreenshotCreator.Api;
using ScreenshotCreator.Logic;
using Creator = ScreenshotCreator.Logic.ScreenshotCreator;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureOpenTelemetry("ScreenshotCreator");

builder.Services.AddHealthChecks();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Configuration
       .AddJsonFile("appsettings.secret.json", true)
       .AddKeyPerFile("/run/secrets", true);
builder.Services
       .AddOptions<ScreenshotOptions>()
       .Bind(builder.Configuration.GetSection(ScreenshotOptions.SectionName))
       .ValidateDataAnnotations()
       .ValidateOnStart();
builder.Services.AddSingleton<IScreenshotCreator, Creator>();
builder.Services.AddSingleton<IPlaywrightHelper, PlaywrightHelper>();
builder.Services.AddSingleton<ImageProcessor>();
builder.Services.AddHostedService<BackgroundScreenshotCreator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UsePathBase("/screenshotCreator");

app.MapGet("latestImage", ReturnImageOrNotFoundAsync);
app.MapGet("createImageNow",
    async (HttpContext httpContext, ImageProcessor imageProcessor, IScreenshotCreator creator, IOptions<ScreenshotOptions> options) =>
    {
        await creator.CreateScreenshotAsync(options.Value.Width, options.Value.Height);
        return await ReturnImageOrNotFoundAsync(httpContext, imageProcessor, options);
    });
app.MapGet("createImageWithSizeNow",
    async (uint width,
           uint height,
           HttpContext httpContext,
           ImageProcessor imageProcessor,
           IOptions<ScreenshotOptions> options,
           IScreenshotCreator creator) =>
    {
        await creator.CreateScreenshotAsync(width, height);
        return await ReturnImageOrNotFoundAsync(httpContext, imageProcessor, options);
    });
app.MapHealthChecks("/healthz");

await app.RunAsync();

async Task<IResult> ReturnImageOrNotFoundAsync(HttpContext httpContext,
                                               ImageProcessor imageProcessor,
                                               IOptions<ScreenshotOptions> options,
                                               bool blackAndWhite = false,
                                               bool asWaveshareBytes = false,
                                               bool addWaveshareInstructions = false)
{
    if (!File.Exists(options.Value.ScreenshotFile))
    {
        return Results.NotFound();
    }

    var processingResult = await imageProcessor.ProcessAsync(options.Value.ScreenshotFile, blackAndWhite, asWaveshareBytes);

    var result = asWaveshareBytes
                     ? Results.Bytes(processingResult.Data, processingResult.MediaType)
                     : Results.File(processingResult.Data, processingResult.MediaType);

    if (addWaveshareInstructions)
    {
        httpContext.Response.Headers.AddWaveshareInstructions(options.Value, options.Value.ScreenshotFile);
    }

    return result;
}