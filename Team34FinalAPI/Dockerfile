# Stage 1: Runtime & PDF dependencies
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Install Linux dependencies for PDF generation
RUN apt-get update && apt-get install -y \
    libgdiplus \
    libx11-6 \
    libxcb1 \
    libxext6 \
    libxrender1 \
    xfonts-75dpi \
    xfonts-base \
    wget \
    && wget https://github.com/wkhtmltopdf/packaging/releases/download/0.12.6-1/wkhtmltox_0.12.6-1.buster_amd64.deb \
    && dpkg -i wkhtmltox_0.12.6-1.buster_amd64.deb || apt-get install -f -y

# Stage 2: Build
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Team34FinalAPI.csproj", "."]
RUN dotnet restore "./Team34FinalAPI.csproj"
COPY . .
RUN dotnet build "Team34FinalAPI.csproj" -c Release -o /app/build

# Stage 3: Publish
FROM build AS publish
RUN dotnet publish "Team34FinalAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 4: Final Image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# Create Images folder for your static file hosting
RUN mkdir -p /app/Images
ENTRYPOINT ["dotnet", "Team34FinalAPI.dll"]