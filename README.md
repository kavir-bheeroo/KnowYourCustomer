# KnowYourCustomer

- Passport biometric page is sent to Abbyy OCR and an XML response is received back.
- Send parsed user details from Abbyy to a KYC check at Trulioo.
- AutoMapper used for mapping.
- IHttpClientFactory used to create HttpClient objects.
- Registration and Login using ASP.NET Core Identity.
- API Authentication using IdentityServer4.
- Messages dispatched using Kafka.
- User details stored in SQL Server using EF Core.
- Unit tests using xUnit.

## Technologies used
- ASP.NET Core
- SQL Server
- EF Core
- Docker
- IdentityServer4
- Kafka
- xUnit

## How to
- To test the application, please download the Postman collection on the following link - [![Run in Postman](https://run.pstmn.io/button.svg)](https://app.getpostman.com/run-collection/7770a90db99fccc7b506)
- Follow the steps below in order to perform a KYC call:
1. Using Docker, bring up the whole system using `docker-compose up`. Wait some time until all containers are up and running.
2. Register a new user - Run the Register request on Postman.
3. Switch to the KYC request and click on the Authorization tab.
4. Choose OAuth 2.0 as the type.
5. Click on Get New Access Token and fill with the following details if not already set up.
	- Grant Type: Password Credentials
	- Access Token URL: http://localhost:5000/connect/token
	- Client ID: postman.ro
	- Client Secret: secret
	- Scope: kyc
6. Click on Request Token. This should generate a new token from the Identity Server and add it as a header.
7. Switch to the Body tab and choose form-data.
8. Add the key `file`, select File from the next dropdown and then browse for a file of a passport's biometric page.
9. Now send the KYC request and this should start a KYC call.
10. A 200 OK response signals a successful verification.
11. Further details can be obtained by going through the IdentityDb.User and KycDb.Kyc tables.

## CLI statements used
Generated C# classes from Abbyy OCR XSD
- xsd.exe /c 'C:\kavir\KnowYourCustomer\src\Kyc\KnowYourCustomer.Kyc.MrzProcessor.Abbyy\Schemas\Document.xsd' /outputdir:C:\

Generated EF Core migrations for IdentityServer
- dotnet ef migrations add InitialIdentityServerPersistedGrantDbMigration -c PersistedGrantDbContext -o Data/Migrations/IdentityServer/PersistedGrantDb
- dotnet ef migrations add InitialIdentityServerConfigurationDbMigration -c ConfigurationDbContext -o Data/Migrations/IdentityServer/ConfigurationDb
- dotnet ef migrations add InitialApplicationDbMigration -c ApplicationDbContext -o Data/Migrations/Identity/ApplicationDb
- dotnet ef migrations add InitialKycDbMigration -c KycDbContext -o Infrastructure/Migrations