FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src
COPY . .
RUN dotnet restore src/RodizioOrganistas.Web/RodizioOrganistas.Web.csproj
RUN dotnet publish src/RodizioOrganistas.Web/RodizioOrganistas.Web.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://0.0.0.0:5000
EXPOSE 5000
ENTRYPOINT ["dotnet", "RodizioOrganistas.Web.dll"]
