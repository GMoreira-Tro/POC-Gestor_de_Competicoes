# Usando a imagem oficial do .NET como base
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 10000

# Usando a imagem do SDK do .NET para compilar o aplicativo
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Angular/CRUDAPI.csproj", "CRUDAPI/"]
RUN dotnet restore "Angular/CRUDAPI.csproj"
COPY . .
WORKDIR "/src/CRUDAPI"
RUN dotnet build "CRUDAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CRUDAPI.csproj" -c Release -o /app/publish

# Copiar o aplicativo compilado para o contêiner
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CRUDAPI.dll"]
