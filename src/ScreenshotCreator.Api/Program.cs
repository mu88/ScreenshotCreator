using System.Net.Mime;
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
builder.Services.AddHostedService<BackgroundScreenshotCreator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UsePathBase("/screenshotCreator");

app.MapGet("latestImage", ReturnImageOrNotFound);
app.MapGet("createImageNow",
           async (Creator creator, IOptions<ScreenshotOptions> options) =>
           {
               await creator.CreateScreenshotAsync(options.Value.Width, options.Value.Height);
               return ReturnImageOrNotFound(options);
           });

app.Run();

IResult ReturnImageOrNotFound(IOptions<ScreenshotOptions> options)
{
    var screenshotFile = Path.Combine(Environment.CurrentDirectory, options.Value.ScreenshotFileName);
    return File.Exists(screenshotFile)
               ? Results.File(screenshotFile,
                              MediaTypeNames.Image.Jpeg)
               : Results.NotFound();
}