
# Etapa 1: compilar a aplicação
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copia os arquivos do projeto
COPY ["SocialNetwork.Server/SocialNetwork.Server.csproj", "SocialNetwork.Server/"]

# Restaura as dependências
RUN dotnet restore "SocialNetwork.Server.csproj"

# Copia o restante do código
COPY . .

# Compila e publica a aplicação
RUN dotnet publish "SocialNetwork.Server.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore

# Etapa 2: executar a aplicação
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "SocialNetwork.Server.dll"]
