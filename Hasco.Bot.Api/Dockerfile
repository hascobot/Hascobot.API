#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-nanoserver-1809 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-nanoserver-1809 AS build
WORKDIR /src
COPY ["Hasco.Bot.Api/Hasco.Bot.Api.csproj", "Hasco.Bot.Api/"]
COPY ["Hasco.Bot.Infrastructure/Hasco.Bot.Infrastructure.csproj", "Hasco.Bot.Infrastructure/"]
COPY ["Hasco.Bot.Core/Hasco.Bot.Core.csproj", "Hasco.Bot.Core/"]
COPY ["Hasco.Bot.Infrastructure.Twitch/Hasco.Bot.Infrastructure.Twitch.csproj", "Hasco.Bot.Infrastructure.Twitch/"]
RUN dotnet restore "Hasco.Bot.Api/Hasco.Bot.Api.csproj"
COPY . .
WORKDIR "/src/Hasco.Bot.Api"
RUN dotnet build "Hasco.Bot.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Hasco.Bot.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Hasco.Bot.Api.dll"]