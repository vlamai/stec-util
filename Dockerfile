﻿FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["stec-util.csproj", "./"]
RUN dotnet restore "stec-util.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "stec-util.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "stec-util.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "stec-util.dll"]
