#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 443
ENV ASPNETCORE_URLS=https://*:443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BookSearch/BookSearch.csproj", "BookSearch/"]
RUN dotnet restore "BookSearch/BookSearch.csproj"
COPY . .
WORKDIR "/src/BookSearch"
RUN dotnet build "BookSearch.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BookSearch.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BookSearch.dll"]