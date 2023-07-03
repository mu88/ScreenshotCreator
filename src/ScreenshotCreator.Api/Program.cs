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
           async (ImageProcessor imageProcessor, Creator creator, IOptions<ScreenshotOptions> options) =>
           {
               await creator.CreateScreenshotAsync(options.Value.Width, options.Value.Height);
               return await ReturnImageOrNotFoundAsync(imageProcessor, options);
           });
app.MapGet("createImageWithSizeNow",
           async (uint width, uint height, ImageProcessor imageProcessor, Creator creator, IOptions<ScreenshotOptions> options) =>
           {
               await creator.CreateScreenshotAsync(width, height);
               return await ReturnImageOrNotFoundAsync(imageProcessor, options);
           });

app.Run();

async Task<IResult> ReturnImageOrNotFoundAsync(ImageProcessor imageProcessor,
                                               IOptions<ScreenshotOptions> options,
                                               bool blackAndWhite = false,
                                               bool asWaveshareBytes = false)
{
    var screenshotFile = Path.Combine(Environment.CurrentDirectory, ScreenshotOptions.ScreenshotFileName);
    if (File.Exists(screenshotFile))
    {
        var processingResult = await imageProcessor.ProcessAsync(screenshotFile, blackAndWhite, asWaveshareBytes);

        if (asWaveshareBytes) return Results.Bytes(processingResult.Data, processingResult.MediaType, lastModified: File.GetLastWriteTimeUtc(screenshotFile));

        return Results.File(processingResult.Data, processingResult.MediaType, lastModified: File.GetLastWriteTimeUtc(screenshotFile));
    }

    return Results.NotFound();
}