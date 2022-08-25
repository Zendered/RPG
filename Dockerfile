#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["RPG.csproj", "."]
RUN dotnet restore "./RPG.csproj"

COPY . .
WORKDIR "/src/."
RUN dotnet build "RPG.csproj" -c Release -o /app/build
RUN dotnet dev-certs https --clean
RUN dotnet dev-certs https
RUN dotnet dev-certs https --trust
RUN dotnet dev-certs https --check

FROM build AS publish
RUN dotnet publish "RPG.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RPG.dll"]