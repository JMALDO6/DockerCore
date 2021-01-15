#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["DockerCore/DockerCore.csproj", "DockerCore/"]
COPY ["DockerCore/DockerCore.csproj", "DockerCore.Data/"]
COPY ["DockerCore/DockerCore.csproj", "DockerCore.Cache/"]
COPY ["DockerCore/DockerCore.csproj", "DockerCore.Cross/"]
COPY ["DockerCore/DockerCore.csproj", "DockerCore.Business/"]
RUN dotnet restore "DockerCore/DockerCore.csproj"
COPY . .
WORKDIR "/src/DockerCore"
RUN dotnet build "DockerCore.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DockerCore.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DockerCore.dll"]