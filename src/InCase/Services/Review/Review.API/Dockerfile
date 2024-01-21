FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Services/Review/Review.API/Review.API.csproj", "Services/Review/Review.API/"]
COPY ["Services/Review/Review.BLL/Review.BLL.csproj", "Services/Review/Review.BLL/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Services/Review/Review.DAL/Review.DAL.csproj", "Services/Review/Review.DAL/"]
RUN dotnet restore "Services/Review/Review.API/Review.API.csproj"
COPY . .
WORKDIR "/src/Services/Review/Review.API"
RUN dotnet build "Review.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Review.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Review.API.dll"]