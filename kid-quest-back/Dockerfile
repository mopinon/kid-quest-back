FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["kid-quest-back/kid-quest-back.csproj", "kid-quest-back/"]
RUN dotnet restore "kid-quest-back/kid-quest-back.csproj"
COPY . .
WORKDIR "/src/kid-quest-back"
RUN dotnet build "kid-quest-back.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "kid-quest-back.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "kid-quest-back.dll"]
