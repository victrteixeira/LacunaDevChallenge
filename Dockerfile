FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["Lacuna.Domain/Lacuna.Domain.csproj", "Lacuna.Domain/"]
COPY ["Lacuna.Application/Lacuna.Application.csproj", "Lacuna.Application/"]
COPY ["Lacuna.Infrastructure/Lacuna.Infrastructure.csproj", "Lacuna.Infrastructure/"]
COPY ["Lacuna.IoC/Lacuna.IoC.csproj", "Lacuna.IoC/"]
COPY ["Lacuna.WebApi/Lacuna.WebApi.csproj", "Lacuna.WebApi/"]

RUN dotnet restore "Lacuna.WebApi/Lacuna.WebApi.csproj"
COPY . .
RUN dotnet build "Lacuna.WebApi/Lacuna.WebApi.csproj" -c Release -o /app/build


FROM build AS publish
RUN dotnet publish "Lacuna.WebApi/Lacuna.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Lacuna.WebApi.dll"]