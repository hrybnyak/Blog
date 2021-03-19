FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 8001

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["ModeratorApp/ModeratorApp.csproj", "ModeratorApp/"]
RUN dotnet restore "ModeratorApp/ModeratorApp.csproj"
COPY . .
WORKDIR "/src/ModeratorApp"
RUN dotnet build "ModeratorApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ModeratorApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ModeratorApp.dll"]