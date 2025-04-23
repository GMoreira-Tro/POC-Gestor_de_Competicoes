# Etapa base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 10000

# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia o projeto
COPY ./CRUDAPI ./CRUDAPI

# Entra na pasta e restaura
WORKDIR /src/CRUDAPI
RUN dotnet restore "CRUDAPI.csproj"

# Build
RUN dotnet build "CRUDAPI.csproj" -c Release -o /app/build

# Publicação
FROM build AS publish
WORKDIR /src/CRUDAPI
RUN dotnet publish "CRUDAPI.csproj" -c Release -o /app/publish

# Etapa final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CRUDAPI.dll"]
