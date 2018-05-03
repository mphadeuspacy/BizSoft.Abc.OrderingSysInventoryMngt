FROM microsoft/aspnetcore:2.0-nanoserver-1709 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0-nanoserver-1709 AS build
WORKDIR /src
COPY BizSoft.All.sln ./
COPY Ordering.WebApi/Ordering.WebApi.csproj Ordering.WebApi/
RUN dotnet restore -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/Ordering.WebApi
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Ordering.WebApi.dll"]
