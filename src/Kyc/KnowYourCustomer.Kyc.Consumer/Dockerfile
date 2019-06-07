FROM mcr.microsoft.com/dotnet/core/runtime:2.2-stretch-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["src/Kyc/KnowYourCustomer.Kyc.Consumer/KnowYourCustomer.Kyc.Consumer.csproj", "src/Kyc/KnowYourCustomer.Kyc.Consumer/"]
COPY ["src/Kyc/KnowYourCustomer.Kyc.Contracts/KnowYourCustomer.Kyc.Contracts.csproj", "src/Kyc/KnowYourCustomer.Kyc.Contracts/"]
COPY ["src/Common/KnowYourCustomer.Common.Messaging/KnowYourCustomer.Common.Messaging.csproj", "src/Common/KnowYourCustomer.Common.Messaging/"]
COPY ["src/Common/KnowYourCustomer.Common/KnowYourCustomer.Common.csproj", "src/Common/KnowYourCustomer.Common/"]
COPY ["src/Common/KnowYourCustomer.Common.Hosting/KnowYourCustomer.Common.Hosting.csproj", "src/Common/KnowYourCustomer.Common.Hosting/"]
COPY ["src/Kyc/KnowYourCustomer.Kyc.Contracts.Public/KnowYourCustomer.Kyc.Contracts.Public.csproj", "src/Kyc/KnowYourCustomer.Kyc.Contracts.Public/"]
COPY ["src/Common/KnowYourCustomer.Common.Messaging.Kafka/KnowYourCustomer.Common.Messaging.Kafka.csproj", "src/Common/KnowYourCustomer.Common.Messaging.Kafka/"]
RUN dotnet restore "src/Kyc/KnowYourCustomer.Kyc.Consumer/KnowYourCustomer.Kyc.Consumer.csproj"
COPY . .
WORKDIR "/src/src/Kyc/KnowYourCustomer.Kyc.Consumer"
RUN dotnet build "KnowYourCustomer.Kyc.Consumer.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "KnowYourCustomer.Kyc.Consumer.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "KnowYourCustomer.Kyc.Consumer.dll"]