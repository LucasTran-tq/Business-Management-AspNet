#// syntax=docker/dockerfile:1
# FROM mcr.microsoft.com/dotnet/sdk:5.0
# COPY . /app
# COPY ./AppMvc.Net.csproj /app
# WORKDIR /app
# RUN ["dotnet", "restore"]
# RUN ["dotnet", "build"]
# EXPOSE 80/tcp
# RUN chmod +x ./entrypoint.sh
# CMD /bin/bash ./entrypoint.sh

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["AppMvc.Net.csproj", "./"]
RUN dotnet restore "./AppMvc.Net.csproj"
COPY . .
RUN dotnet build "AppMvc.Net.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "AppMvc.Net.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "AppMvc.Net.dll"]