# <img src="/src/icon.png" height="30px"> NServiceBus.Community.MicrosoftLogging

[![Build status](https://img.shields.io/appveyor/build/SimonCropp/nservicebus-community-MicrosoftLogging)](https://ci.appveyor.com/project/SimonCropp/nservicebus-community-MicrosoftLogging)
[![NuGet Status](https://img.shields.io/nuget/v/NServiceBus.Community.MicrosoftLogging.svg)](https://www.nuget.org/packages/NServiceBus.Community.MicrosoftLogging/)

Add support for [NServiceBus](https://particular.net/nservicebus) to log to [Microsoft.Extensions.Logging](https://github.com/aspnet/Logging).

**See [Milestones](../../milestones?state=closed) for release notes.**

<!--- StartOpenCollectiveBackers -->

[Already a Patron? skip past this section](#endofbacking)


## Community backed

**It is expected that all developers [become a Patron](https://opencollective.com/nservicebuscommunity/contribute/patron-6976) to use NServiceBus Community Extensions. [Go to licensing FAQ](https://github.com/NServiceBusCommunity/Home/#licensingpatron-faq)**


### Sponsors

Support this project by [becoming a Sponsor](https://opencollective.com/nservicebuscommunity/contribute/sponsor-6972). The company avatar will show up here with a website link. The avatar will also be added to all GitHub repositories under the [NServiceBusCommunity organization](https://github.com/NServiceBusCommunity).


### Patrons

Thanks to all the backing developers. Support this project by [becoming a patron](https://opencollective.com/nservicebuscommunity/contribute/patron-6976).

<img src="https://opencollective.com/nservicebuscommunity/tiers/patron.svg?width=890&avatarHeight=60&button=false">

<a href="#" id="endofbacking"></a>

<!--- EndOpenCollectiveBackers -->


## NuGet package

https://nuget.org/packages/NServiceBus.Community.MicrosoftLogging/
https://nuget.org/packages/NServiceBus.Community.MicrosoftLogging.Hosting


## Usage

snippet: MsLoggingInCode


## Usage when hosting

As `LoggerFactory` implements [IDisposable](https://msdn.microsoft.com/en-us/library/system.idisposable.aspx) it must be disposed of after stopping the NServiceBus endpoint. The process for doing this will depend on how the endpoint is being hosted.


### In a generic host

Disposing the `LoggerFactory` is done by the underlying infrastructure.

snippet: MsLoggingInGenericHost

Note: `UseMicrosoftLogFactoryLogger` requires adding `NServiceBus.MicrosoftLogging.Hosting` as a package dependency.


### In a windows service

When [hosting in a windows service](https://docs.particular.net/nservicebus/hosting/windows-service) `LoggerFactory` should be disposed of as part of the [ServiceBase.OnStop](https://msdn.microsoft.com/en-us/library/system.serviceprocess.servicebase.onstop.aspx) execution.

snippet: MsLoggingInService


## Icon

[Abstract](https://thenounproject.com/term/abstract/847344/) designed by [Neha Shinde](https://thenounproject.com/neha.shinde) from [The Noun Project](https://thenounproject.com).
