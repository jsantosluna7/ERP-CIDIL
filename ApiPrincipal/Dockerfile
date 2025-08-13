# Fase 1: build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar solo los archivos necesarios
COPY ApiPrincipal/ApiPrincipal.csproj ApiPrincipal/
COPY ERP.Data/ERP.Data.csproj ERP.Data/
COPY Inventario/Inventario.csproj Inventario/
COPY IoT/IoT.csproj IoT/
COPY Reservas/Reservas.csproj Reservas/
COPY Usuarios/Usuarios.csproj Usuarios/

# Restaurar solo referencias necesarias
RUN dotnet restore ApiPrincipal/ApiPrincipal.csproj

# Copiar el resto de archivos
COPY . .

# Publicar
RUN dotnet publish ApiPrincipal/ApiPrincipal.csproj -c Release -o /app/publish

# Fase 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
COPY ApiPrincipal/.env .env
EXPOSE 80
ENTRYPOINT ["dotnet", "ApiPrincipal.dll"]
