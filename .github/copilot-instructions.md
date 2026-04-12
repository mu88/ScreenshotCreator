# ScreenshotCreator — Repo Context

## Project Structure
- Two projects: `ScreenshotCreator.Logic` (all business logic, all types `internal`) and `ScreenshotCreator.Api` (entry point, configuration, `Program.cs`).
- `InternalsVisibleTo("Tests")` is declared in `ScreenshotCreator.Logic.csproj` — never make Logic types `public` unless they are also consumed by `Api`.

## Testing
- Three `WebApplicationFactory<Program>` subclasses already exist: `WebApplicationFactoryForAny`, `WebApplicationFactoryForOpenHab` (integration tests), and shared helpers in `Shared.cs`. Follow these patterns for new integration tests.
- System tests are Playwright-based and live in `PlaywrightTests.cs`. They require the container image to be built first — derive the exact command from `Playwright.yml`. Do **not** run them locally without the container.

## Analyzer Pitfalls
- CA1861: inline `new[]` in attributes/method calls is forbidden — extract to `static readonly` fields. The established pattern is a dedicated `ValidationErrorMessages` class with `static readonly string[]` arrays.
- `TypedResults.ValidationProblem()` returns HTTP **400**, not 422 — write integration test assertions accordingly.
