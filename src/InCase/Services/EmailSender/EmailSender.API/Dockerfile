FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Services/EmailSender/EmailSender.API/EmailSender.API.csproj", "Services/EmailSender/EmailSender.API/"]
COPY ["Services/EmailSender/EmailSender.BLL/EmailSender.BLL.csproj", "Services/EmailSender/EmailSender.BLL/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Services/EmailSender/EmailSender.DAL/EmailSender.DAL.csproj", "Services/EmailSender/EmailSender.DAL/"]
RUN dotnet restore "Services/EmailSender/EmailSender.API/EmailSender.API.csproj"
COPY . .
WORKDIR "/src/Services/EmailSender/EmailSender.API"
RUN dotnet build "EmailSender.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EmailSender.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EmailSender.API.dll"]