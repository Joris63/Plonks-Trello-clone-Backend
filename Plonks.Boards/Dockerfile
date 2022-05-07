#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Plonks.Shared/Plonks.Shared.csproj", "Plonks.Shared.csproj"]
COPY ["Plonks.Boards/Plonks.Boards.csproj", "Plonks.Boards/Plonks.Boards.csproj"]
RUN dotnet restore "Plonks.Boards/Plonks.Boards.csproj"
COPY . .
WORKDIR "/src/Plonks.Boards"
RUN dotnet build "Plonks.Boards.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Plonks.Boards.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Plonks.Boards.dll"]