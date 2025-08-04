# Etapa base para ejecución
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8085
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Development

# Etapa build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiamos el archivo .csproj al contexto de compilación
COPY Store.MicroServices.Autor.api/Store.MicroServices.Autor.api.csproj ./Store.MicroServices.Autor.api/
WORKDIR /src/Store.MicroServices.Autor.api
RUN dotnet restore

# Copiamos todo el contenido
COPY Store.MicroServices.Autor.api/. .

# Compilamos el proyecto
RUN dotnet build -c $BUILD_CONFIGURATION -o /app/build

# Etapa publish
FROM build AS publish
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Etapa final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Store.MicroServices.Autor.api.dll"]
