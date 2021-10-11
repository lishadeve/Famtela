#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
ENV ASPNETCORE_URLS=https://+:5005;http://+:5006
WORKDIR /app
EXPOSE 5005
EXPOSE 5006

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/Web/Server/Server.csproj", "src/Web/Server/"]
COPY ["src/Infrastructure/Infrastructure.Shared/Infrastructure.Shared.csproj", "src/Infrastructure/Infrastructure.Shared/"]
COPY ["src/Core/Application/Application.csproj", "src/Core/Application/"]
COPY ["src/Core/Domain/Domain.csproj", "src/Core/Domain/"]
COPY ["src/Shared/Shared.csproj", "src/Shared/"]
COPY ["src/Infrastructure/Infrastructure/Infrastructure.csproj", "src/Infrastructure/Infrastructure/"]
COPY ["src/Web/Client/Client/Client.csproj", "src/Web/Client/Client/"]
COPY ["src/Web/Client/Client.Infrastructure/Client.Infrastructure.csproj", "src/Web/Client/Client.Infrastructure/"]
RUN dotnet restore "src/Web/Server/Server.csproj"
COPY . .
WORKDIR "/src/src/Web/Server"
RUN dotnet build "Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
WORKDIR /app/Files
WORKDIR /app
ENTRYPOINT ["dotnet", "Famtela.Server.dll"]