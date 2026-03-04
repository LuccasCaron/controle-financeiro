# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia os arquivos de projeto
COPY ["ControleFinanceiro.Api/ControleFinanceiro.Api.csproj", "ControleFinanceiro.Api/"]
COPY ["ControleFinanceiro.Application/ControleFinanceiro.Application.csproj", "ControleFinanceiro.Application/"]
COPY ["ControleFinanceiro.Domain/ControleFinanceiro.Domain.csproj", "ControleFinanceiro.Domain/"]
COPY ["ControleFinanceiro.Infrastructure/ControleFinanceiro.Infrastructure.csproj", "ControleFinanceiro.Infrastructure/"]
COPY ["ControleFinanceiro.Infrastructure.IoC/ControleFinanceiro.Infrastructure.IoC.csproj", "ControleFinanceiro.Infrastructure.IoC/"]

# Restaura dependências
RUN dotnet restore "ControleFinanceiro.Api/ControleFinanceiro.Api.csproj"

# Copia todo o código
COPY . .

# Build da aplicação
WORKDIR "/src/ControleFinanceiro.Api"
RUN dotnet build "ControleFinanceiro.Api.csproj" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "ControleFinanceiro.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Expõe a porta 8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "ControleFinanceiro.Api.dll"]
