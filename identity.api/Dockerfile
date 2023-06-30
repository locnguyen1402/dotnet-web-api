FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env

WORKDIR /app

COPY . .

RUN dotnet restore
RUN dotnet publish -c Release -o published

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build-env /app/published .
ENV ASPNETCORE_URLS=http://+:5096
ENTRYPOINT ["dotnet", "identity.api.dll"]