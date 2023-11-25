
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /app

COPY . .

RUN dotnet restore

RUN dotnet build -c Release -o out

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0

WORKDIR /app

COPY --from=build /app/out .

EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "backendBaseDatos.dll"]
