FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY src/ src/
WORKDIR /app/src/Wernher.API/

RUN dotnet restore
RUN dotnet publish -c release -o /app --no-restore


# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
RUN cp /usr/share/zoneinfo/America/Sao_Paulo /etc/localtime

EXPOSE 8080
WORKDIR /app
COPY --from=build /app ./

ENTRYPOINT ["dotnet", "Wernher.API.dll"]