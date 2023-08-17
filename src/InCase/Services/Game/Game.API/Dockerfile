#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
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