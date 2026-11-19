
# 1. Imagem com ferramentas para compilar
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

# 2. Diretório de trabalho
WORKDIR /src

# 3. Copiar os arquivos do projeto
COPY ["SocialNetwork.Server/SocialNetwork.Server.csproj", "SocialNetwork.Server/"]

# 4. Restaurar dependências
RUN dotnet restore "SocialNetwork.Server/SocialNetwork.Server.csproj"

# 5. Copiar o restante do código
COPY . .

# 6. Compilar e publicar
WORKDIR /src/SocialNetwork.Server
RUN dotnet publish -c Release -o /app/publish

# 7. Imagem menor para executar a aplicação
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

# 8. Diretório de execução
WORKDIR /app

# 9. Copiar os arquivos publicados
COPY --from=build /app/publish .

# 10. Definir a porta
EXPOSE 8080

# 11. Comando de inicialização
ENTRYPOINT ["dotnet", "SocialNetwork.Server.dll"]
