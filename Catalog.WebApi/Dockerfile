FROM microsoft/aspnetcore:2.0-nanoserver-1709 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0-nanoserver-1709 AS build
WORKDIR /src
COPY Catalog.WebApi/Catalog.WebApi.csproj Catalog.WebApi/
RUN dotnet restore Catalog.WebApi/Catalog.WebApi.csproj
COPY . .
WORKDIR /src/Catalog.WebApi
RUN dotnet build Catalog.WebApi.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Catalog.WebApi.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Catalog.WebApi.dll"]
