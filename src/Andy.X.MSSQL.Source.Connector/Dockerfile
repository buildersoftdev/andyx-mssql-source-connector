# Andy X MSSQL Source Connector DockerFile

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Andy.X.MSSQL.Source.Connector/Andy.X.MSSQL.Source.Connector.csproj", "src/Andy.X.MSSQL.Source.Connector/"]
COPY ["src/Andy.X.MSSQL.Source.Connector.Core/Andy.X.MSSQL.Source.Connector.Core.csproj", "src/Andy.X.MSSQL.Source.Connector.Core/"]
COPY ["src/Andy.X.MSSQL.Source.Connector.IO/Andy.X.MSSQL.Source.Connector.IO.csproj", "src/Andy.X.MSSQL.Source.Connector.IO/"]
RUN dotnet restore "src/Andy.X.MSSQL.Source.Connector/Andy.X.MSSQL.Source.Connector.csproj"
COPY . .
WORKDIR "/src/src/Andy.X.MSSQL.Source.Connector"
RUN dotnet build "Andy.X.MSSQL.Source.Connector.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Andy.X.MSSQL.Source.Connector.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Andy.X.MSSQL.Source.Connector.dll"]