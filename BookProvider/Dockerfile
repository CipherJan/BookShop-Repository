FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "/src/BookProvider/BookProvider.csproj"
RUN dotnet publish "/src/BookProvider/BookProvider.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "BookProvider.dll"]