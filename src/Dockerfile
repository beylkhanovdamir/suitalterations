#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-nanoserver-1903 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-nanoserver-1903 AS build
WORKDIR /src
COPY ["SuitAlterations.API/SuitAlterations.API.csproj", "SuitAlterations.API/"]
COPY ["SuitAlterations.Application/SuitAlterations.Application.csproj", "SuitAlterations.Application/"]
COPY ["SuitAlterations.Domain/SuitAlterations.Domain.csproj", "SuitAlterations.Domain/"]
COPY ["SuitAlterations.Domain.Messages/SuitAlterations.Domain.Messages.csproj", "SuitAlterations.Domain.Messages/"]
COPY ["SuitAlterations.Infrastructure/SuitAlterations.Infrastructure.csproj", "SuitAlterations.Infrastructure/"]
RUN dotnet restore "SuitAlterations.API/SuitAlterations.API.csproj"
COPY . .
WORKDIR "/src/SuitAlterations.API"
RUN dotnet build "SuitAlterations.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SuitAlterations.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SuitAlterations.API.dll"]