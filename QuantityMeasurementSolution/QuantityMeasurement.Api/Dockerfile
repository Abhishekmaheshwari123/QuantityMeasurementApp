FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files first to leverage Docker layer caching for restore.
COPY ["QuantityMeasurement.Api/QuantityMeasurement.Api.csproj", "QuantityMeasurement.Api/"]
COPY ["BusinessLayer/BusinessLayer.csproj", "BusinessLayer/"]
COPY ["ModelLayer/ModelLayer.csproj", "ModelLayer/"]
COPY ["RepositoryLayer/RepositoryLayer.csproj", "RepositoryLayer/"]
COPY ["QuantityMeasurement.Domain/QuantityMeasurement.Domain.csproj", "QuantityMeasurement.Domain/"]

RUN dotnet restore "QuantityMeasurement.Api/QuantityMeasurement.Api.csproj"

# Copy source and publish the API.
COPY . .
WORKDIR "/src/QuantityMeasurement.Api"
RUN dotnet publish "QuantityMeasurement.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "QuantityMeasurement.Api.dll"]
