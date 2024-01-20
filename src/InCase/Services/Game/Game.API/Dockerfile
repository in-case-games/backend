FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Services/Game/Game.API/Game.API.csproj", "Services/Game/Game.API/"]
COPY ["Services/Game/Game.BLL/Game.BLL.csproj", "Services/Game/Game.BLL/"]
COPY ["Services/Game/Game.DAL/Game.DAL.csproj", "Services/Game/Game.DAL/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
RUN dotnet restore "Services/Game/Game.API/Game.API.csproj"
COPY . .
WORKDIR "/src/Services/Game/Game.API"
RUN dotnet build "Game.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Game.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Game.API.dll"]