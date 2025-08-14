#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Базовый образ для runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Этап сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TaskService.API/TaskService.API.csproj", "TaskService.API/"]
RUN dotnet restore "TaskService.API/TaskService.API.csproj"
COPY . .
WORKDIR "/src/TaskService.API"
RUN dotnet build "TaskService.API.csproj" -c Release -o /app/build

# Этап публикации
FROM build AS publish
RUN dotnet publish "TaskService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Финальный образ
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaskService.API.dll"]

#FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
#WORKDIR /app
#EXPOSE 80
#
#FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
#WORKDIR /src
#COPY ["TaskService.API/TaskService.API.csproj", "TaskService.API/"]
#COPY ["TaskService.Core/TaskService.Core.csproj", "TaskService.Core/"]
#RUN dotnet restore "TaskService.API/TaskService.API.csproj"
#COPY . .
#WORKDIR "/src/TaskService.API"
#RUN dotnet build "TaskService.API.csproj" -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "TaskService.API.csproj" -c Release -o /app/publish
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "TaskService.API.dll"]