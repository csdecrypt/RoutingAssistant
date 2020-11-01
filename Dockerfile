# Prepare OSM Data
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS db-creator
WORKDIR /app

RUN wget https://download.geofabrik.de/europe/switzerland-201001.osm.pbf -O switzerland.osm.pbf
COPY RouterDbCreator ./
RUN dotnet restore RouterDbCreator.csproj

RUN dotnet publish RouterDbCreator.csproj --no-restore -o /bin
RUN dotnet /bin/RouterDbCreator.dll -i switzerland.osm.pbf -o /out

# Build Application
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app
COPY . ./
RUN dotnet publish RoutingAssisstant/RoutingAssistant.csproj -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
COPY wait-for-it.sh /wait-for-it.sh
RUN chmod +x wait-for-it.sh
WORKDIR /app
COPY --from=build-env /app/out .
COPY --from=db-creator /out .

ENTRYPOINT ["dotnet", "RoutingAssistant.dll"]