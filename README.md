# KnowYourCustomer

- Passport biometric page is sent to Abbyy OCR and an XML response is received back.
- Send parsed user details from Abbyy to a KYC check at Trulioo.
- AutoMapper used for mapping.
- IHttpClientFactory used to create HttpClient objects.
- Registration and Login using ASP.NET Core Identity.
- API Authentication using IdentityServer4.

todo:
- Use configuration settings to flip call to Abbyy.
- Setup events and handle Abbyy status check asynchronously.
- Persist all details and documents to a relational data store.
- Dockerise the whole system.
- Add authentication.
- Return resourceId in initiate kyc call to adhere to REST.
- Check which methods can be made async.
- Add AutoMapper
- Add Caching -- cache country code in Verifier
- Create custom exceptions that map to specific HTTP codes.
- Add Swagger
- Strategy pattern based on appsettings to choose Trulioo verifier implementation.


Generated C# classes from Abbyy OCR XSD
- xsd.exe /c 'C:\kavir\KnowYourCustomer\src\Kyc\KnowYourCustomer.Kyc.MrzProcessor.Abbyy\Schemas\Document.xsd' /outputdir:C:\

Generated EF Core migrations for IdentityServer
- dotnet ef migrations add InitialIdentityServerPersistedGrantDbMigration -c PersistedGrantDbContext -o Data/Migrations/IdentityServer/PersistedGrantDb
- dotnet ef migrations add InitialIdentityServerConfigurationDbMigration -c ConfigurationDbContext -o Data/Migrations/IdentityServer/ConfigurationDb
- dotnet ef migrations add InitialApplicationDbMigration -c ApplicationDbContext -o Data/Migrations/Identity/ApplicationDb