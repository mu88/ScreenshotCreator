using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Options;
using ScreenshotCreator.Api;
using ScreenshotCreator.Logic;
using Creator = ScreenshotCreator.Logic.ScreenshotCreator;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<ScreenshotOptions>(builder.Configuration.GetSection(ScreenshotOptions.SectionName));
builder.Services.AddSingleton<Creator>();
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
           async (HttpContext httpContext, ImageProcessor imageProcessor, Creator creator, IOptions<ScreenshotOptions> options) =>
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
                  Creator creator) =>
           {
               await creator.CreateScreenshotAsync(width, height);
               return await ReturnImageOrNotFoundAsync(httpContext, imageProcessor, options);
           });

app.Run();

async Task<IResult> ReturnImageOrNotFoundAsync(HttpContext httpContext,
                                               ImageProcessor imageProcessor,
                                               IOptions<ScreenshotOptions> options,
                                               bool blackAndWhite = false,
                                               bool asWaveshareBytes = false,
                                               bool addWaveshareInstructions = false)
{
    var screenshotFile = Path.Combine(Environment.CurrentDirectory, ScreenshotOptions.ScreenshotFileName);
    if (!File.Exists(screenshotFile)) return Results.NotFound();

    var processingResult = await imageProcessor.ProcessAsync(screenshotFile, blackAndWhite, asWaveshareBytes);

    var result = asWaveshareBytes
                     ? Results.Bytes(processingResult.Data, processingResult.MediaType)
                     : Results.File(processingResult.Data, processingResult.MediaType);

    if (addWaveshareInstructions) AddWaveshareInstructions(httpContext.Response.Headers, options.Value, screenshotFile);

    return result;
}

void AddWaveshareInstructions(IHeaderDictionary headers, ScreenshotOptions screenshotOptions, string screenshotFile)
{
    headers.Add("waveshare-last-modified-local-time", GetLastModifiedAsLocalTime(screenshotFile));
    headers.Add("waveshare-sleep-between-updates", CalculateSleepBetweenUpdates(screenshotOptions));
    headers.Add("waveshare-update-screen", DisplayShouldBeActive(screenshotOptions.Activity) ? true.ToString() : false.ToString());
}

string CalculateSleepBetweenUpdates(ScreenshotOptions screenshotOptions) =>
    DisplayShouldBeActive(screenshotOptions.Activity)
        ? screenshotOptions.RefreshIntervalInSeconds.ToString()
        : screenshotOptions.Activity.RefreshIntervalWhenInactiveInSeconds.ToString();

bool DisplayShouldBeActive([NotNullWhen(false)] Activity? activity)
{
    if (activity is null) return true;

    var currentLocalTime = GetCurrentLocalTime();
    return activity.ActiveFrom <= currentLocalTime && currentLocalTime <= activity.ActiveTo;
}

TimeOnly GetCurrentLocalTime() =>
    TimeOnly.FromDateTime(TimeZoneInfo
                              .ConvertTimeFromUtc(DateTime.UtcNow,
                                                  TimeZoneInfo.FindSystemTimeZoneById(Environment.GetEnvironmentVariable("TZ") ?? TimeZoneInfo.Local.Id)));

string GetLastModifiedAsLocalTime(string file) =>
    TimeZoneInfo
        .ConvertTimeFromUtc(File.GetLastWriteTimeUtc(file), TimeZoneInfo.FindSystemTimeZoneById(Environment.GetEnvironmentVariable("TZ") ?? TimeZoneInfo.Local.Id))
        .ToShortTimeString();