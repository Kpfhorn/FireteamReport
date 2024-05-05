FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5274

ENV ASPNETCORE_URLS=http://+:5274

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["FireteamReport/FireteamReport.csproj", "FireteamReport/"]
RUN dotnet restore "FireteamReport/FireteamReport.csproj"
COPY . .
WORKDIR "/src/FireteamReport"
RUN dotnet build "FireteamReport.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "FireteamReport.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FireteamReport.dll"]
