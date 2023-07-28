﻿using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
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
    public async Task ProcessAsync() => await new ImageProcessor(new Mock<ILogger<ImageProcessor>>().Object).ProcessAsync("testData/Screenshot.png", true, true);
}