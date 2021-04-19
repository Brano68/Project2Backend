#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Visma.FamilyTree.WebAPI/Visma.FamilyTree.WebAPI.csproj", "Visma.FamilyTree.WebAPI/"]
COPY ["Visma.FamilyTree.Composition/Visma.FamilyTree.Composition.csproj", "Visma.FamilyTree.Composition/"]
COPY ["Visma.FamilyTree.DbModels/Visma.FamilyTree.DbModels.csproj", "Visma.FamilyTree.DbModels/"]
COPY ["Visma.FamilyTree.RandomNumberGenerator/Visma.FamilyTree.RandomNumberGenerator.csproj", "Visma.FamilyTree.RandomNumberGenerator/"]
COPY ["Visma.FamilyTree.Repository/Visma.FamilyTree.Repository.csproj", "Visma.FamilyTree.Repository/"]
COPY ["Visma.FamilyTree.DTO/Visma.FamilyTree.DTO.csproj", "Visma.FamilyTree.DTO/"]
RUN dotnet restore "Visma.FamilyTree.WebAPI/Visma.FamilyTree.WebAPI.csproj"
COPY . .
WORKDIR "/src/Visma.FamilyTree.WebAPI"
RUN dotnet build "Visma.FamilyTree.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Visma.FamilyTree.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Visma.FamilyTree.WebAPI.dll"]