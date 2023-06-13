#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 20
EXPOSE 21
EXPOSE 22
EXPOSE 80
EXPOSE 443

ENV ConnectionStrings:DefaultConnection="Server=db;Database=aspnet-iwa-dev;User=sa;Password=novell@123;"
ENV FTP_USER=testuser
ENV FTP_PASS=Pa$$w0rd

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["InsecureWebApp/InsecureWebApp.csproj", "InsecureWebApp/"]
COPY ["InsecureWebApp/wwwroot", "InsecureWebApp/wwwroot"]
COPY ["InsecureWebApp/Files", "InsecureWebApp/Files"]
COPY ["InsecureWebApp/Files/Prescription/.", "InsecureWebApp/Files/Prescription/."]
RUN dotnet restore "InsecureWebApp/InsecureWebApp.csproj"
COPY . .
WORKDIR "/src/InsecureWebApp"
RUN dotnet build "InsecureWebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "InsecureWebApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=publish /app/wwwroot .
ENTRYPOINT ["dotnet", "InsecureWebApp.dll"]