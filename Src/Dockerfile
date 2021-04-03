#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src

COPY ["DeUrgenta.Api/DeUrgenta.Api.csproj", "DeUrgenta.Api/"]
COPY ["DeUrgenta.Backpack.Api/DeUrgenta.Backpack.Api.csproj", "DeUrgenta.Backpack.Api/"]
COPY ["DeUrgenta.Certifications.Api/DeUrgenta.Certifications.Api.csproj", "DeUrgenta.Certifications.Api/"]
COPY ["DeUrgenta.Admin.Api/DeUrgenta.Admin.Api.csproj", "DeUrgenta.Admin.Api/"]
COPY ["DeUrgenta.Domain/DeUrgenta.Domain.csproj", "DeUrgenta.Domain/"]
COPY ["DeUrgenta.Infra/DeUrgenta.Infra.csproj", "DeUrgenta.Infra/"]
COPY ["DeUrgenta.Common/DeUrgenta.Common.csproj", "DeUrgenta.Common/"]

RUN dotnet restore "DeUrgenta.Api/DeUrgenta.Api.csproj"

COPY . .
WORKDIR "/src/DeUrgenta.Api"
RUN dotnet build "DeUrgenta.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DeUrgenta.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DeUrgenta.Api.dll"]