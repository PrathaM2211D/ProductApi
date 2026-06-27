# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore
COPY src/ProductApi.Domain/ProductApi.Domain.csproj src/ProductApi.Domain/
COPY src/ProductApi.Application/ProductApi.Application.csproj src/ProductApi.Application/
COPY src/ProductApi.Infrastructure/ProductApi.Infrastructure.csproj src/ProductApi.Infrastructure/
COPY src/ProductApi.API/ProductApi.API.csproj src/ProductApi.API/
RUN dotnet restore src/ProductApi.API/ProductApi.API.csproj

# Copy everything and build
COPY . .
WORKDIR /src/src/ProductApi.API
RUN dotnet build -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "ProductApi.API.dll"]
