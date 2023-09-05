FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# Install Node.js
RUN curl -fsSL https://deb.nodesource.com/setup_14.x | bash - \
    && apt-get install -y \
        nodejs \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /src
COPY ["group-project-thread/group-project-thread.csproj", "group-project-thread/"]
RUN dotnet restore "group-project-thread/group-project-thread.csproj"
COPY . .
WORKDIR "/src/group-project-thread"
RUN dotnet build "group-project-thread.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "group-project-thread.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "group-project-thread.dll"]
