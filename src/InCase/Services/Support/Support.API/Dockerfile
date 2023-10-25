#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
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