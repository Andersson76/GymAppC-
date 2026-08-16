FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY GymAppC.Api/*.csproj GymAppC.Api/
COPY GymApp.Application/*.csproj GymApp.Application/
COPY GymAppC.Domain/*.csproj GymAppC.Domain/
COPY GymAppC.Infrastructure/*.csproj GymAppC.Infrastructure/
RUN dotnet restore GymAppC.Api/GymAppC.Api.csproj

COPY . .

WORKDIR /src/GymAppC.Api
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "GymAppC.Api.dll"]
