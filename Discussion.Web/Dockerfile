FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY *.sln .
COPY Discussion.Web/*.csproj Discussion.Web/
COPY Discussion.Core/*.csproj Discussion.Core/

RUN dotnet restore

COPY . .
RUN dotnet publish Discussion.Web/Discussion.Web.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "Discussion.Web.dll"]