FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["src/Kyc/KnowYourCustomer.Kyc.Host/KnowYourCustomer.Kyc.Host.csproj", "src/Kyc/KnowYourCustomer.Kyc.Host/"]
COPY ["src/Kyc/KnowYourCustomer.Kyc.Contracts/KnowYourCustomer.Kyc.Contracts.csproj", "src/Kyc/KnowYourCustomer.Kyc.Contracts/"]
COPY ["src/Kyc/KnowYourCustomer.Kyc.MrzProcessor.Abbyy/KnowYourCustomer.Kyc.MrzProcessor.Abbyy.csproj", "src/Kyc/KnowYourCustomer.Kyc.MrzProcessor.Abbyy/"]
COPY ["src/Kyc/KnowYourCustomer.Kyc.MrzProcessor.Contracts/KnowYourCustomer.Kyc.MrzProcessor.Contracts.csproj", "src/Kyc/KnowYourCustomer.Kyc.MrzProcessor.Contracts/"]
COPY ["src/Common/KnowYourCustomer.Common/KnowYourCustomer.Common.csproj", "src/Common/KnowYourCustomer.Common/"]
COPY ["src/Kyc/KnowYourCustomer.Kyc.Verifier.Trulioo/KnowYourCustomer.Kyc.Verifier.Trulioo.csproj", "src/Kyc/KnowYourCustomer.Kyc.Verifier.Trulioo/"]
COPY ["src/Kyc/KnowYourCustomer.Kyc.Verifier.Contracts/KnowYourCustomer.Kyc.Verifier.Contracts.csproj", "src/Kyc/KnowYourCustomer.Kyc.Verifier.Contracts/"]
COPY ["src/Kyc/KnowYourCustomer.Kyc.Data.EfCore/KnowYourCustomer.Kyc.Data.EfCore.csproj", "src/Kyc/KnowYourCustomer.Kyc.Data.EfCore/"]
COPY ["src/Kyc/KnowYourCustomer.Kyc.Data.Contracts/KnowYourCustomer.Kyc.Data.Contracts.csproj", "src/Kyc/KnowYourCustomer.Kyc.Data.Contracts/"]
COPY ["src/Kyc/KnowYourCustomer.Kyc.Contracts.Public/KnowYourCustomer.Kyc.Contracts.Public.csproj", "src/Kyc/KnowYourCustomer.Kyc.Contracts.Public/"]
COPY ["src/Common/KnowYourCustomer.Common.Messaging.Kafka/KnowYourCustomer.Common.Messaging.Kafka.csproj", "src/Common/KnowYourCustomer.Common.Messaging.Kafka/"]
COPY ["src/Common/KnowYourCustomer.Common.Messaging/KnowYourCustomer.Common.Messaging.csproj", "src/Common/KnowYourCustomer.Common.Messaging/"]
COPY ["src/Kyc/KnowYourCustomer.Kyc/KnowYourCustomer.Kyc.csproj", "src/Kyc/KnowYourCustomer.Kyc/"]
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