#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Web/DistanceSchool.Web/DistanceSchool.Web.csproj", "Web/DistanceSchool.Web/"]
COPY ["Data/DistanceSchool.Data.Models/DistanceSchool.Data.Models.csproj", "Data/DistanceSchool.Data.Models/"]
COPY ["Data/DistanceSchool.Data.Common/DistanceSchool.Data.Common.csproj", "Data/DistanceSchool.Data.Common/"]
COPY ["Web/DistanceSchool.Web.Infrastructure/DistanceSchool.Web.Infrastructure.csproj", "Web/DistanceSchool.Web.Infrastructure/"]
COPY ["Services/DistanceSchool.Services/DistanceSchool.Services.csproj", "Services/DistanceSchool.Services/"]
COPY ["Services/DistanceSchool.Services.Messaging/DistanceSchool.Services.Messaging.csproj", "Services/DistanceSchool.Services.Messaging/"]
COPY ["DistanceSchool.Common/DistanceSchool.Common.csproj", "DistanceSchool.Common/"]
COPY ["Services/DistanceSchool.Services.Data/DistanceSchool.Services.Data.csproj", "Services/DistanceSchool.Services.Data/"]
COPY ["Services/DistanceSchool.Services.Mapping/DistanceSchool.Services.Mapping.csproj", "Services/DistanceSchool.Services.Mapping/"]
COPY ["Web/DistanceSchool.Web.ViewModels/DistanceSchool.Web.ViewModels.csproj", "Web/DistanceSchool.Web.ViewModels/"]
COPY ["Data/DistanceSchool.Data/DistanceSchool.Data.csproj", "Data/DistanceSchool.Data/"]
RUN dotnet restore "Web/DistanceSchool.Web/DistanceSchool.Web.csproj"
COPY . .
WORKDIR "/src/Web/DistanceSchool.Web"
RUN dotnet build "DistanceSchool.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DistanceSchool.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "pp.dll"]