# Estágio 1: Build do Angular
FROM node:20-alpine AS build-node
WORKDIR /src
COPY lista-karaoke/ClientApp/package*.json ./
RUN npm install
COPY lista-karaoke/ClientApp/ .
RUN npm run build -- --configuration production

# Estágio 2: Build do .NET
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-dotnet
WORKDIR /src
COPY ["lista-karaoke/ListaKaraoke.Server.csproj", "lista-karaoke/"]
RUN dotnet restore "lista-karaoke/ListaKaraoke.Server.csproj"
COPY . .
WORKDIR "/src/lista-karaoke"
RUN dotnet build "ListaKaraoke.Server.csproj" -c Release -o /app/build

FROM build-dotnet AS publish
RUN dotnet publish "ListaKaraoke.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Estágio 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=publish /app/publish .
# Copia a pasta docs para que a importação de CSV funcione no container
COPY lista-karaoke/docs ./docs
# Copia o build do Angular para a pasta que o .NET espera (geralmente wwwroot ou definida no Program.cs)
# Nota: Verifique se o seu Program.cs serve arquivos estáticos de ClientApp/dist/lista-karaoke.client/browser
COPY --from=build-node /src/dist/lista-karaoke.client/browser ./wwwroot

ENTRYPOINT ["dotnet", "ListaKaraoke.Server.dll"]
