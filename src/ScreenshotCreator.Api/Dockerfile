FROM --platform=$TARGETARCH mcr.microsoft.com/dotnet/aspnet:7.0 AS base
ARG TARGETARCH
WORKDIR /app
COPY src/ScreenshotCreator.Api/install-powershell-$TARGETARCH.sh /scripts/install-powershell-$TARGETARCH.sh
RUN chmod +x /scripts/install-powershell-$TARGETARCH.sh
RUN /scripts/install-powershell-$TARGETARCH.sh

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS sdk
WORKDIR /src_playwright
RUN dotnet new console --no-restore 
RUN dotnet add package Microsoft.Playwright
RUN dotnet build --property:PlaywrightPlatform=all

FROM base AS playwright-install
COPY --from=sdk /src_playwright/bin/Debug/net*/ /src/
ENV PLAYWRIGHT_BROWSERS_PATH=/ms-playwright
RUN mkdir /ms-playwright
RUN ["pwsh", "/src/playwright.ps1", "install", "chromium"]
RUN ["pwsh", "/src/playwright.ps1", "install-deps", "chromium"]
RUN rm -rf /src/*

FROM sdk AS build
ARG TARGETARCH
WORKDIR /src
COPY . .
RUN dotnet publish -a $TARGETARCH "/src/src/ScreenshotCreator.Api/ScreenshotCreator.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM playwright-install AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ScreenshotCreator.Api.dll"]
