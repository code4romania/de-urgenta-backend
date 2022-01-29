#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["DeUrgenta.Domains.Migrator/DeUrgenta.Domains.Migrator.csproj", "DeUrgenta.Domains.Migrator/"]
COPY ["DeUrgenta.Domain.Identity/DeUrgenta.Domain.Identity.csproj", "DeUrgenta.Domain.Identity/"]
COPY ["DeUrgenta.Domain.Api/DeUrgenta.Domain.Api.csproj", "DeUrgenta.Domain.Api/"]
COPY ["DeUrgenta.Domain.I18n/DeUrgenta.Domain.I18n.csproj", "DeUrgenta.Domain.I18n/"]
COPY ["DeUrgenta.I18n.Service/DeUrgenta.I18n.Service.csproj", "DeUrgenta.I18n.Service/"]
COPY ["DeUrgenta.Domain.RecurringJobs/DeUrgenta.Domain.RecurringJobs.csproj", "DeUrgenta.Domain.RecurringJobs/"]
COPY ["DeUrgenta.Infra/DeUrgenta.Infra.csproj", "DeUrgenta.Infra/"]
COPY ["DeUrgenta.Common/DeUrgenta.Common.csproj", "DeUrgenta.Common/"]

RUN dotnet restore "DeUrgenta.Domains.Migrator/DeUrgenta.Domains.Migrator.csproj"

COPY ["DeUrgenta.Api/appsettings.json", "DeUrgenta.Domains.Migrator/"]
COPY ["DeUrgenta.Domains.Migrator", "DeUrgenta.Domains.Migrator/"]
COPY ["DeUrgenta.Domain.Api", "DeUrgenta.Domain.Api/"]
COPY ["DeUrgenta.Domain.Identity", "DeUrgenta.Domain.Identity/"]
COPY ["DeUrgenta.Domain.I18n", "DeUrgenta.Domain.I18n/"]
COPY ["DeUrgenta.I18n.Service", "DeUrgenta.I18n.Service/"]
COPY ["DeUrgenta.Domain.RecurringJobs", "DeUrgenta.Domain.RecurringJobs/"]
COPY ["DeUrgenta.Infra", "DeUrgenta.Infra/"]
COPY ["DeUrgenta.Common", "DeUrgenta.Common/"]

WORKDIR "/src/DeUrgenta.Domains.Migrator"
RUN dotnet build "DeUrgenta.Domains.Migrator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DeUrgenta.Domains.Migrator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DeUrgenta.Domains.Migrator.dll"]