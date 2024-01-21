FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Services/Withdraw/Withdraw.API/Withdraw.API.csproj", "Services/Withdraw/Withdraw.API/"]
COPY ["Services/Withdraw/Withdraw.BLL/Withdraw.BLL.csproj", "Services/Withdraw/Withdraw.BLL/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Services/Withdraw/Withdraw.DAL/Withdraw.DAL.csproj", "Services/Withdraw/Withdraw.DAL/"]
RUN dotnet restore "Services/Withdraw/Withdraw.API/Withdraw.API.csproj"
COPY . .
WORKDIR "/src/Services/Withdraw/Withdraw.API"
RUN dotnet build "Withdraw.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Withdraw.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Withdraw.API.dll"]