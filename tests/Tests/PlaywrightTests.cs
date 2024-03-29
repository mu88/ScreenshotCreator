﻿using System.Diagnostics;

namespace Tests;

public abstract class PlaywrightTests
{
    [SetUp]
    public void InstallPlaywright() =>
        Process.Start(new ProcessStartInfo("pwsh", "playwright.ps1 install chromium") { CreateNoWindow = true, UseShellExecute = false })
            ?.WaitForExit();
}