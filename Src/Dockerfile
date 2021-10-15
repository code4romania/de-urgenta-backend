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
COPY ["DeUrgenta.Group.Api/DeUrgenta.Group.Api.csproj", "DeUrgenta.Group.Api/"]
COPY ["DeUrgenta.User.Api/DeUrgenta.User.Api.csproj", "DeUrgenta.User.Api/"]
COPY ["DeUrgenta.Domain.Api/DeUrgenta.Domain.Api.csproj", "DeUrgenta.Domain.Api/"]
COPY ["DeUrgenta.Domain.Identity/DeUrgenta.Domain.Identity.csproj", "DeUrgenta.Domain.Identity/"]
COPY ["DeUrgenta.Domain.I18n/DeUrgenta.Domain.I18n.csproj", "DeUrgenta.Domain.I18n/"]
COPY ["DeUrgenta.Infra/DeUrgenta.Infra.csproj", "DeUrgenta.Infra/"]
COPY ["DeUrgenta.Common/DeUrgenta.Common.csproj", "DeUrgenta.Common/"]

COPY ["DeUrgenta.Emailing.Service/DeUrgenta.Emailing.Service.csproj", "DeUrgenta.Emailing.Service/"]
COPY ["DeUrgenta.Events.Api/DeUrgenta.Events.Api.csproj", "DeUrgenta.Events.Api/"]
COPY ["DeUrgenta.I18n.Service/DeUrgenta.I18n.Service.csproj", "DeUrgenta.I18n.Service/"]
COPY ["DeUrgenta.Invite.Api/DeUrgenta.Invite.Api.csproj", "DeUrgenta.Invite.Api/"]

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