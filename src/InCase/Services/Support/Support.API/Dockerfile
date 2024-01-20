FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Services/Support/Support.API/Support.API.csproj", "Services/Support/Support.API/"]
COPY ["Services/Support/Support.BLL/Support.BLL.csproj", "Services/Support/Support.BLL/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Services/Support/Support.DAL/Support.DAL.csproj", "Services/Support/Support.DAL/"]
RUN dotnet restore "Services/Support/Support.API/Support.API.csproj"
COPY . .
WORKDIR "/src/Services/Support/Support.API"
RUN dotnet build "Support.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Support.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Support.API.dll"]