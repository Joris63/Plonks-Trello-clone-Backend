#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Plonks.Shared/Plonks.Shared.csproj", "Plonks.Shared.csproj"]
COPY ["Plonks.Cards/Plonks.Cards.csproj", "Plonks.Cards/Plonks.Cards.csproj"]
RUN dotnet restore "Plonks.Cards/Plonks.Cards.csproj"
COPY . .
WORKDIR "/src/Plonks.Cards"
RUN dotnet build "Plonks.Cards.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Plonks.Cards.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Plonks.Cards.dll"]