FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["src/Kyc/KnowYourCustomer.Kyc.Host/KnowYourCustomer.Kyc.Host.csproj", "src/Kyc/KnowYourCustomer.Kyc.Host/"]
RUN dotnet restore "src/Kyc/KnowYourCustomer.Kyc.Host/KnowYourCustomer.Kyc.Host.csproj"
COPY . .
WORKDIR "/src/src/Kyc/KnowYourCustomer.Kyc.Host"
RUN dotnet build "KnowYourCustomer.Kyc.Host.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "KnowYourCustomer.Kyc.Host.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "KnowYourCustomer.Kyc.Host.dll"]