#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["BookProvider/BookProvider.csproj", "BookProvider/"]
COPY ["BookProvider.Core/BookProvider.Core.csproj", "BookProvider.Core/"]
COPY ["BookProvider.Infrastructure/BookProvider.Infrastructure.csproj", "BookProvider.Infrastructure/"]
RUN dotnet restore "BookProvider/BookProvider.csproj"
COPY . .
WORKDIR "/src/BookProvider"
RUN dotnet build "BookProvider.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BookProvider.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_ENVIRONMENT Development
ENV ASPNETCORE_URLS http://+:80
ENTRYPOINT ["dotnet", "BookProvider.dll"]