FROM --platform=$TARGETARCH mcr.microsoft.com/dotnet/aspnet:7.0 AS base
ARG TARGETARCH
WORKDIR /app
COPY src/ScreenshotCreator.Api/install-powershell-$TARGETARCH.sh /scripts/install-powershell-$TARGETARCH.sh
RUN chmod +x /scripts/install-powershell-$TARGETARCH.sh
RUN /scripts/install-powershell-$TARGETARCH.sh

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG TARGETARCH
WORKDIR /src
COPY ["src/ScreenshotCreator.Api/ScreenshotCreator.Api.csproj", "src/ScreenshotCreator.Api/"]
COPY ["src/ScreenshotCreator.Logic/ScreenshotCreator.Logic.csproj", "src/ScreenshotCreator.Logic/"]
RUN dotnet restore -a $TARGETARCH "src/ScreenshotCreator.Api/ScreenshotCreator.Api.csproj"
COPY . .
WORKDIR "/src/src/ScreenshotCreator.Api"
RUN dotnet build -a $TARGETARCH "ScreenshotCreator.Api.csproj" -c Release -o /app/build

FROM build AS publish
ARG TARGETARCH
RUN dotnet publish -a $TARGETARCH "ScreenshotCreator.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN ["pwsh", "playwright.ps1", "install", "chromium"]
RUN ["pwsh", "playwright.ps1", "install-deps", "chromium"]
ENTRYPOINT ["dotnet", "ScreenshotCreator.Api.dll"]
