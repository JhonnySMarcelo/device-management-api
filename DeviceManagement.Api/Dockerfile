# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Copy project file and restore dependencies
COPY DeviceManagementApi.csproj .
RUN dotnet restore DeviceManagementApi.csproj

# Copy the rest of the source code (Dockerfile será ignorado pelo .dockerignore)
COPY . .

# Publish the app
RUN dotnet publish DeviceManagementApi.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "DeviceManagementApi.dll"]
