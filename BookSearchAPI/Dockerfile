#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BookSearchAPI/BookSearchAPI.csproj", "BookSearchAPI/"]
RUN dotnet restore "BookSearchAPI/BookSearchAPI.csproj"
COPY . .
WORKDIR "/src/BookSearchAPI"
RUN dotnet build "BookSearchAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BookSearchAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BookSearchAPI.dll"]