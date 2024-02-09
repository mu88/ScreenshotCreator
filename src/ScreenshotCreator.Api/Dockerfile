FROM --platform=$TARGETARCH mu88/screenshotcreator-playwright:latest AS base
ARG TARGETARCH

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG TARGETARCH
WORKDIR /src
COPY . .
RUN dotnet publish -a $TARGETARCH "/src/src/ScreenshotCreator.Api/ScreenshotCreator.Api.csproj" -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ScreenshotCreator.Api.dll"]
