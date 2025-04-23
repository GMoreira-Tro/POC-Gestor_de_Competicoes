# Etapa base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 10000

# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia todos os arquivos primeiro
COPY . .

# Restaura na pasta correta onde está o .csproj
WORKDIR /src/CRUDAPI
RUN dotnet restore "CRUDAPI.csproj"

# Build
RUN dotnet build "CRUDAPI.csproj" -c Release -o /app/build

# Etapa de publicação
FROM build AS publish
WORKDIR /src/CRUDAPI
RUN dotnet publish "CRUDAPI.csproj" -c Release -o /app/publish

# Etapa final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CRUDAPI.dll"]
