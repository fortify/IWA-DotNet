#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0.202 AS build
WORKDIR /src
COPY ["InsecureWebApp/InsecureWebApp.csproj", "InsecureWebApp/"]
COPY ["InsecureWebApp/wwwroot", "InsecureWebApp/wwwroot"]
COPY ["InsecureWebApp/Files", "InsecureWebApp/Files"]
RUN dotnet restore "InsecureWebApp/InsecureWebApp.csproj"
COPY . .
WORKDIR "/src/InsecureWebApp"
RUN dotnet build "InsecureWebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "InsecureWebApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN mkdir -p Files/Prescriptions
ENTRYPOINT ["dotnet", "InsecureWebApp.dll"]
