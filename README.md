# EngineBay.DataProtection

[![NuGet version](https://badge.fury.io/nu/EngineBay.DataProtection.svg)](https://badge.fury.io/nu/EngineBay.DataProtection)
[![Maintainability](https://api.codeclimate.com/v1/badges/0a557d3b4b3c577472d5/maintainability)](https://codeclimate.com/github/engine-bay/data-protection/maintainability)
[![Test Coverage](https://api.codeclimate.com/v1/badges/0a557d3b4b3c577472d5/test_coverage)](https://codeclimate.com/github/engine-bay/data-protection/test_coverage)

DataProtection module for EngineBay published to [EngineBay.DataProtection](https://www.nuget.org/packages/EngineBay.DataProtection/) on NuGet.

## About

Registering and configuring this module will add a [DataProtectionProvider](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.dataprotection.dataprotectionprovider?view=aspnetcore-7.0) to your service collection, making it available for dependency injection. You can use this provider to encrypt and decrypt text, which is useful when storing sensitive data. 

For any fields that you wish to be encrypted while stored in your database, the DataProtectionProvider should be used to encrypt before storing, and decrypt when you wish to access the value. This means that the data protection mechanism is agnostic to which database system you use.

**Warning**: Whilst this module provides the options to store your encryption keys in a local files system or on a redis server, it cannot back up your keystore. Ensure you have a secure backup strategy. If you lose the keys, you will lose access to the data.

Read more about data protection [here](https://learn.microsoft.com/en-us/aspnet/core/security/data-protection/introduction?view=aspnetcore-7.0).

## Usage

If the module is registered, accessing it in any other module's class is as simple as normal dependency injection: specify `IDataProtectionProvider dataProtectionProvider` as a constructor parameter.

To use this, you will need to create a protector and use it to protect or unprotect your desired strings.

To encrypt:

```cs
var dataProtector = dataProtectionProvider.CreateProtector("Reason for encrypting");
var encryptedMessage = dataProtector.Protect(message);
```

To unencrypt:

```cs
var dataProtector = dataProtectionProvider.CreateProtector("Reason for encrypting");
var message = dataProtector.Unprotect(encryptedMessage);
```

For an example of a model with a field that is stored encrypted at rest, see the EncryptedMessage field for [SessionLog](https://github.com/engine-bay/actor-engine/blob/main/EngineBay.ActorEngine/Models/SessionLog.cs#L57) from [EngineBay.ActorEngine](https://github.com/engine-bay/actor-engine/tree/main/EngineBay.ActorEngine).

### Registration

See the [Demo API registration guide](https://github.com/engine-bay/demo-api).

### Environment Variables

See the [Documentation Portal](https://github.com/engine-bay/documentation-portal/blob/main/EngineBay.DocumentationPortal/DocumentationPortal/docs/documentation/configuration/environment-variables.md#data-protection).