#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["DeUrgenta.IdentityServer/DeUrgenta.IdentityServer.csproj", "DeUrgenta.IdentityServer/"]
COPY ["DeUrgenta.Common/DeUrgenta.Common.csproj", "DeUrgenta.Common/"]
RUN dotnet restore "DeUrgenta.Common/DeUrgenta.Common.csproj"
RUN dotnet restore "DeUrgenta.IdentityServer/DeUrgenta.IdentityServer.csproj"
COPY . .

WORKDIR "/src/DeUrgenta.IdentityServer"
RUN dotnet build "DeUrgenta.IdentityServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DeUrgenta.IdentityServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN chmod -R 1744 /app

ENTRYPOINT ["dotnet", "DeUrgenta.IdentityServer.dll"]