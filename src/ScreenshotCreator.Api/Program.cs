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
                                               bool bw = false,
                                               bool wb = false,
                                               bool wi = false)
{
    var screenshotFile = Path.Combine(Environment.CurrentDirectory, ScreenshotOptions.ScreenshotFileName);
    if (!File.Exists(screenshotFile)) return Results.NotFound();

    var processingResult = await imageProcessor.ProcessAsync(screenshotFile, bw, wb);

    var result = wb
                     ? Results.Bytes(processingResult.Data, processingResult.MediaType)
                     : Results.File(processingResult.Data, processingResult.MediaType);

    if (wi) AddWaveshareInstructions(httpContext.Response.Headers, options.Value, screenshotFile);

    return result;
}

void AddWaveshareInstructions(IHeaderDictionary headers, ScreenshotOptions screenshotOptions, string screenshotFile)
{
    headers.Add("w-lt", GetLastModifiedAsLocalTime(screenshotFile));
    headers.Add("w-s", CalculateSleepBetweenUpdates(screenshotOptions));
    headers.Add("w-u", screenshotOptions.Activity.DisplayShouldBeActive() ? true.ToString() : false.ToString());
}

string CalculateSleepBetweenUpdates(ScreenshotOptions screenshotOptions) =>
    screenshotOptions.Activity.DisplayShouldBeActive()
        ? screenshotOptions.RefreshIntervalInSeconds.ToString()
        : screenshotOptions.Activity.RefreshIntervalWhenInactiveInSeconds.ToString();

string GetLastModifiedAsLocalTime(string file) =>
    TimeZoneInfo
        .ConvertTimeFromUtc(File.GetLastWriteTimeUtc(file), TimeZoneInfo.FindSystemTimeZoneById(Environment.GetEnvironmentVariable("TZ") ?? TimeZoneInfo.Local.Id))
        .ToShortTimeString();