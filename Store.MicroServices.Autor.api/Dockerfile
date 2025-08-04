# Consulte https://aka.ms/customizecontainer para aprender a personalizar su contenedor de depuración y cómo Visual Studio usa este Dockerfile para compilar sus imágenes para una depuración más rápida.

# Esta fase se usa cuando se ejecuta desde VS en modo rápido (valor predeterminado para la configuración de depuración)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8085

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Development

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Store.MicroServices.Autor.api.csproj", "."]
RUN dotnet restore "./Store.MicroServices.Autor.api.csproj"
COPY . .
RUN dotnet build "./Store.MicroServices.Autor.api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Store.MicroServices.Autor.api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Store.MicroServices.Autor.api.dll"]


## Esta fase se usa para compilar el proyecto de servicio
#FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
#ARG BUILD_CONFIGURATION=Release
#WORKDIR /src
#COPY ["Store.MicroServices.Autor.api/Store.MicroServices.Autor.api.csproj", "Store.MicroServices.Autor.api/"]
#RUN dotnet restore "./Store.MicroServices.Autor.api/Store.MicroServices.Autor.api.csproj"
#COPY . .
#WORKDIR "/src/Store.MicroServices.Autor.api"
#RUN dotnet build "./Store.MicroServices.Autor.api.csproj" -c $BUILD_CONFIGURATION -o /app/build
#
## Esta fase se usa para publicar el proyecto de servicio que se copiará en la fase final.
#FROM build AS publish
#ARG BUILD_CONFIGURATION=Release
#RUN dotnet publish "./Store.MicroServices.Autor.api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
#
## Esta fase se usa en producción o cuando se ejecuta desde VS en modo normal (valor predeterminado cuando no se usa la configuración de depuración)
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "Store.MicroServices.Autor.api.dll"]