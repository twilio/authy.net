**This library is no longer actively maintained. Please visit the [Twilio Docs for Authy / .NET usage documentation](https://www.twilio.com/docs/authy/api?code-language=C%23)** or consider using [Twilio Verify with Twilio's .NET helper library for your SMS verification needs](https://www.twilio.com/docs/verify/api?code-language=C%23).

Forked and patched from the library of devinmartin in [https://bitbucket.org/devinmartin/authy.net/wiki/Home](https://bitbucket.org/devinmartin/authy.net/wiki/Home)

# Authy .NET

Documentation for .NET usage of the Authy API lives in the [official Twilio documentation](https://www.twilio.com/docs/authy/api/).

The Authy API supports multiple channels of 2FA:
* One-time passwords via SMS and voice.
* Soft token ([TOTP](https://www.twilio.com/docs/glossary/totp) via the Authy App)
* Push authentication via the Authy App

If you only need SMS and Voice support for one-time passwords, we recommend using the [Twilio Verify API](https://www.twilio.com/docs/verify/api) instead. 

[More on how to choose between Authy and Verify here.](https://www.twilio.com/docs/verify/authy-vs-verify)

### Authy Quickstart

For a full tutorial, check out the .NET Authy Quickstart in our docs:
* [.NET Core C# Authy Quickstart](https://www.twilio.com/docs/authy/quickstart/dotnet-core-csharp-two-factor-authentication)

## 2FA Workflow

1. [Create a user](https://www.twilio.com/docs/authy/api/users#enabling-new-user)
2. [Send a one-time password](https://www.twilio.com/docs/authy/api/one-time-passwords)
3. [Verify a one-time password](https://www.twilio.com/docs/authy/api/one-time-passwords#verify-a-one-time-password)

**OR**

1. [Create a user](https://www.twilio.com/docs/authy/api/users#enabling-new-user)
2. [Send a push authentication](https://www.twilio.com/docs/authy/api/push-authentications)
3. [Check a push authentication status](https://www.twilio.com/docs/authy/api/push-authentications#check-approval-request-status)

## <a name="phone-verification"></a>Phone Verification

[Phone verification now lives in the Twilio API](https://www.twilio.com/docs/verify/api) and has [C#/.NET support through the official Twilio helper libraries](https://www.twilio.com/docs/libraries/csharp-dotnet). 

**Verify V1 is not recommended for new development. Please consider using [Verify V2](https://www.twilio.com/docs/verify/api)**.

## Authy.NET installation
*Note: this library is no longer actively maintained and may be incompatible with the version of .NET in your project.* The documentation above will point you to maintained libraries and code samples.

With the Visual Studio IDE:
```
Install-Package Authy.Net
```

**OR**

With .NET Core command line tools:
```
dotnet add package Authy.Net
```

## Usage

To use the Authy client, create an instance of AuthyClient and initialize it with your production API Key found in the [Twilio Console](https://www.twilio.com/console/authy/applications/):

```
using Authy.Net;

var client = new AuthyClient("your_api_key", test:false);
```

![authy api key in console](https://s3.amazonaws.com/com.twilio.prod.twilio-docs/images/account-security-api-key.width-800.png)

All of the methods return a result object specific to the call. There is always a Status property and a RawResponse property. The raw response can be used to get additional information when debugging problems but generally isn't needed in actual code.

### Response statuses are:
1. Success
2. BadRequest
3. Unauthorized
    * This can mean that your API key is invalid or that the provided token is invalid. Once your integration has been tested it likely means that the token is invalid but the raw response will provide more info.
4. ServiceUnavailable

## Copyright

Copyright (c) 2015-2020 Authy Inc. See [LICENSE.txt](https://github.com/twilio/authy.net/blob/master/LICENSE.txt) for further details.
