# Etapa base para execução
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 10000

# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copia o .csproj e restaura dependências
COPY ["Angular/CRUDAPI/CRUDAPI.csproj", "Angular/CRUDAPI/"]
RUN dotnet restore "Angular/CRUDAPI/CRUDAPI.csproj"

# Copia o restante do código e compila
COPY . .
WORKDIR "/src/Angular/CRUDAPI"
RUN dotnet build "CRUDAPI.csproj" -c Release -o /app/build

# Etapa de publicação
FROM build AS publish
RUN dotnet publish "CRUDAPI.csproj" -c Release -o /app/publish

# Etapa final de execução
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CRUDAPI.dll"]
