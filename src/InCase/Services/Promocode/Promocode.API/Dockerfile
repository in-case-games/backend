FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Services/Promocode/Promocode.API/Promocode.API.csproj", "Services/Promocode/Promocode.API/"]
COPY ["Services/Promocode/Promocode.BLL/Promocode.BLL.csproj", "Services/Promocode/Promocode.BLL/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Services/Promocode/Promocode.DAL/Promocode.DAL.csproj", "Services/Promocode/Promocode.DAL/"]
RUN dotnet restore "Services/Promocode/Promocode.API/Promocode.API.csproj"
COPY . .
WORKDIR "/src/Services/Promocode/Promocode.API"
RUN dotnet build "Promocode.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Promocode.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Promocode.API.dll"]