# ======================
# BUILD STAGE
# ======================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj và restore dependencies
COPY SalesManagement.Api.csproj .
RUN dotnet restore

# Copy toàn bộ mã nguồn
COPY . .
RUN dotnet publish -c Release -o out

# ======================
# RUNTIME STAGE
# ======================
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

EXPOSE 8080

ENTRYPOINT ["dotnet", "SalesManagement.Api.dll"]
