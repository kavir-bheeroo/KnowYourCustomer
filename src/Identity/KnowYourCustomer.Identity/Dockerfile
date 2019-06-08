FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["src/Identity/KnowYourCustomer.Identity/KnowYourCustomer.Identity.csproj", "src/Identity/KnowYourCustomer.Identity/"]
COPY ["src/Identity/KnowYourCustomer.Identity.Contracts/KnowYourCustomer.Identity.Contracts.csproj", "src/Identity/KnowYourCustomer.Identity.Contracts/"]
COPY ["src/Common/KnowYourCustomer.Common/KnowYourCustomer.Common.csproj", "src/Common/KnowYourCustomer.Common/"]
RUN dotnet restore "src/Identity/KnowYourCustomer.Identity/KnowYourCustomer.Identity.csproj"
COPY . .
WORKDIR "/src/src/Identity/KnowYourCustomer.Identity"
RUN dotnet build "KnowYourCustomer.Identity.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "KnowYourCustomer.Identity.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "KnowYourCustomer.Identity.dll"]