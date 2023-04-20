FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Manul/Manul.csproj", "Manul/"]
RUN dotnet restore "Manul/Manul.csproj"
COPY . .
WORKDIR "/src/Manul"
RUN dotnet build "Manul.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Manul.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Manul.dll"]
