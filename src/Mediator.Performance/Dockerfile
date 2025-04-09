FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Mediator.Performance/Mediator.Performance.csproj", "src/Mediator.Performance/"]
COPY ["src/Mediator.Shared/Mediator.Shared.csproj", "src/Mediator.Shared/"]
RUN dotnet restore "src/Mediator.Performance/Mediator.Performance.csproj"
COPY . .
WORKDIR "/src/src/Mediator.Performance"
RUN dotnet build "Mediator.Performance.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Mediator.Performance.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mediator.Performance.dll"]