FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5050
EXPOSE 5051
ENV ASPNETCORE_URLS=https://+:5050;http://+:5051

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ./aspnet-core ./aspnet-core
WORKDIR ./aspnet-core
WORKDIR ./ServicesWebApi
RUN dotnet restore -nowarn:msb3202,nu1503
RUN dotnet build --no-restore -c Release -o /app

FROM build AS publish
RUN dotnet publish --no-restore -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ServicesWebApi.dll"]
