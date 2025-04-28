# Base Container
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base
WORKDIR /app
EXPOSE 8090 8080

#Build Container
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src/temp
COPY src ./src
COPY ./nuget.config .
WORKDIR /src/temp/src

RUN dotnet restore "Presentation.Application/Presentation.Application.csproj"
RUN dotnet build "Presentation.Application/Presentation.Application.csproj" -c Release -o /app/build

#Publish/Release Container
FROM build AS publish
RUN dotnet publish "Presentation.Application/Presentation.Application.csproj" -c Release -o /app/publish

#Release Container
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ab.Inbev.Service.dll"]