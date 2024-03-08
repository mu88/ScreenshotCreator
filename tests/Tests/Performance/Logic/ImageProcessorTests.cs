using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using ScreenshotCreator.Logic;

namespace Tests.Performance.Logic;

[TestFixture]
[Category("Performance")]
public class ImageProcessorTests
{
    [Test]
    public void ProcessImage_ShouldNotConsumeTooMuchMemory_WhenCreatingBlackWhiteImageInWaveshareFormat()
    {
        // Arrange & Act
        var summary = BenchmarkRunner.Run<ImageProcessorBenchmarks>(new DebugInProcessConfig());

        // Assert
        summary.Reports.Should().NotBeNullOrEmpty();
        summary.Reports.First().GcStats.GetBytesAllocatedPerOperation(summary.BenchmarksCases.First()).Should().BeLessThan(500000);
    }
}

[MemoryDiagnoser]
public class ImageProcessorBenchmarks
{
    [Benchmark]
    public static async Task ProcessAsync() => await new ImageProcessor(Substitute.For<ILogger<ImageProcessor>>()).ProcessAsync("testData/Screenshot.png", true, true);
}