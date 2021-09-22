########### BUILD ###########

FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build

WORKDIR /app

COPY . .
RUN dotnet restore Exporter.csproj

RUN dotnet publish -c Release -o out



########### RUNTIME ###########

FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine AS runtime

WORKDIR /app

EXPOSE 80
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "Exporter.dll"]
