# KnowYourCustomer

- Passport biometric page is sent to Abbyy OCR and an XML response is received back.
- Send parsed user details from Abbyy to a KYC check at Trulioo

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


Generated C# classes from Abbyy OCR XSD
- xsd.exe /c 'C:\kavir\KnowYourCustomer\src\Kyc\KnowYourCustomer.Kyc.MrzProcessor.Abbyy\Schemas\Document.xsd' /outputdir:C:\