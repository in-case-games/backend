#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
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