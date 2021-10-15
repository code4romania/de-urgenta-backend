#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["DeUrgenta.RecurringJobs/DeUrgenta.RecurringJobs.csproj", "DeUrgenta.RecurringJobs/"]
COPY ["DeUrgenta.Domain.Api/DeUrgenta.Domain.Api.csproj", "DeUrgenta.Domain.Api/"]
COPY ["DeUrgenta.Infra/DeUrgenta.Infra.csproj", "DeUrgenta.Infra/"]
COPY ["DeUrgenta.Emailing.Service/DeUrgenta.Emailing.Service.csproj", "DeUrgenta.Emailing.Service/"]

RUN dotnet restore "DeUrgenta.RecurringJobs/DeUrgenta.RecurringJobs.csproj"

COPY . .
WORKDIR "/src/DeUrgenta.RecurringJobs"
RUN dotnet build "DeUrgenta.RecurringJobs.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DeUrgenta.RecurringJobs.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DeUrgenta.RecurringJobs.dll"]
