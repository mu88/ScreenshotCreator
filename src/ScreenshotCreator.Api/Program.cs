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
               return await ReturnImageOrNotFoundAsync(httpContext, imageProcessor);
           });
app.MapGet("createImageWithSizeNow",
           async (uint width,
                  uint height,
                  HttpContext httpContext,
                  ImageProcessor imageProcessor,
                  Creator creator) =>
           {
               await creator.CreateScreenshotAsync(width, height);
               return await ReturnImageOrNotFoundAsync(httpContext, imageProcessor);
           });

app.Run();

async Task<IResult> ReturnImageOrNotFoundAsync(HttpContext httpContext,
                                               ImageProcessor imageProcessor,
                                               bool blackAndWhite = false,
                                               bool asWaveshareBytes = false)
{
    var screenshotFile = Path.Combine(Environment.CurrentDirectory, ScreenshotOptions.ScreenshotFileName);
    if (!File.Exists(screenshotFile)) return Results.NotFound();

    var processingResult = await imageProcessor.ProcessAsync(screenshotFile, blackAndWhite, asWaveshareBytes);

    var result = asWaveshareBytes
                     ? Results.Bytes(processingResult.Data, processingResult.MediaType)
                     : Results.File(processingResult.Data, processingResult.MediaType);

    httpContext.Response.Headers.Add("last-modified-local-time", GetLastModifiedAsLocalTime(screenshotFile));

    return result;
}

string GetLastModifiedAsLocalTime(string file) =>
    TimeZoneInfo
        .ConvertTimeFromUtc(File.GetLastWriteTimeUtc(file), TimeZoneInfo.FindSystemTimeZoneById(Environment.GetEnvironmentVariable("TZ") ?? TimeZoneInfo.Local.Id))
        .ToShortTimeString();