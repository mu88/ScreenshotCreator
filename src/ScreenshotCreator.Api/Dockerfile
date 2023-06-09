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
RUN apt-get install -y wget apt-transport-https software-properties-common
RUN wget -q https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb
RUN dpkg -i packages-microsoft-prod.deb
RUN apt-get update
RUN apt-get install -y powershell
RUN ["pwsh", "playwright.ps1" ,"install"]
RUN ["pwsh", "playwright.ps1" ,"install-deps"]
ENTRYPOINT ["dotnet", "ScreenshotCreator.Api.dll"]
