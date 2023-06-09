ARG BASE_IMAGE=mcr.microsoft.com/dotnet/aspnet:7.0-bullseye-slim-arm64v8
FROM ${BASE_IMAGE} AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/ScreenshotCreator.Api/ScreenshotCreator.Api.csproj", "src/ScreenshotCreator.Api/"]
COPY ["src/ScreenshotCreator.Logic/ScreenshotCreator.Logic.csproj", "src/ScreenshotCreator.Logic/"]
RUN dotnet restore "src/ScreenshotCreator.Api/ScreenshotCreator.Api.csproj"
COPY . .
WORKDIR "/src/src/ScreenshotCreator.Api"
RUN dotnet build "ScreenshotCreator.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ScreenshotCreator.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN apt-get update
RUN apt-get install -y wget libssl1.1 libunwind8
RUN mkdir -p /opt/microsoft/powershell/7
RUN wget -O /tmp/powershell.tar.gz https://github.com/PowerShell/PowerShell/releases/download/v7.2.6/powershell-7.2.6-linux-arm64.tar.gz
RUN tar zxf /tmp/powershell.tar.gz -C /opt/microsoft/powershell/7
RUN chmod +x /opt/microsoft/powershell/7/pwsh
RUN ln -s /opt/microsoft/powershell/7/pwsh /usr/bin/pwsh
RUN rm /tmp/powershell.tar.gz
RUN ["pwsh", "playwright.ps1", "install", "chromium"]
RUN ["pwsh", "playwright.ps1", "install-deps", "chromium"]
ENTRYPOINT ["dotnet", "ScreenshotCreator.Api.dll"]
