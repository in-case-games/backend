#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
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