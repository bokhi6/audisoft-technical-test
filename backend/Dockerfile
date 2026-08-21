FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY AudiSoft.sln ./
COPY src/AudiSoft.Domain/AudiSoft.Domain.csproj src/AudiSoft.Domain/
COPY src/AudiSoft.Application/AudiSoft.Application.csproj src/AudiSoft.Application/
COPY src/AudiSoft.Infrastructure/AudiSoft.Infrastructure.csproj src/AudiSoft.Infrastructure/
COPY src/AudiSoft.WebApi/AudiSoft.WebApi.csproj src/AudiSoft.WebApi/
RUN dotnet restore src/AudiSoft.WebApi/AudiSoft.WebApi.csproj

COPY src/ src/
RUN dotnet publish src/AudiSoft.WebApi/AudiSoft.WebApi.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "AudiSoft.WebApi.dll"]
