﻿FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build-env
WORKDIR /app
EXPOSE 80
COPY *.csproj ./
RUN dotnet restore
COPY . ./
RUN dotnet publish -c Release -o out 

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:5000
COPY --from=build-env /app/out ./
ENTRYPOINT ["dotnet", "SpaceShooter.dll"]