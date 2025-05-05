FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["FacilityTracker/FacilityTracker.csproj", "FacilityTracker/"]
RUN #dotnet tool install --global dotnet-ef
#ENV PATH="$PATH:/root/.dotnet/tools"

RUN dotnet restore "FacilityTracker/FacilityTracker.csproj"
COPY . .
WORKDIR "/src/FacilityTracker"
RUN dotnet build "FacilityTracker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FacilityTracker.csproj" -c Release -o /app/publish
RUN #dotnet ef database update

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FacilityTracker.dll"]
