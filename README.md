<h1 align="center"> Smittestopp Mobile Application <br/><img style="margin-right: 1%; margin-bottom: 0.5em; float: left;" src="https://www.fhi.no/globalassets/bilder/smittestopp/smittestopp-logo-temaside.png?preset=themeheaderimage"> </h1>
<br/>

[![GitHub last commit](https://img.shields.io/github/last-commit/folkehelseinstituttet/Fhi.Smittestopp.App)](https://github.com/folkehelseinstituttet/Fhi.Smittestopp.App/commits)
[![Open pull requests](https://img.shields.io/github/issues-pr/folkehelseinstituttet/Fhi.Smittestopp.App)](https://github.com/folkehelseinstituttet/Fhi.Smittestopp.App/pulls)
[![Open issues](https://img.shields.io/github/issues/folkehelseinstituttet/Fhi.Smittestopp.App)](https://github.com/folkehelseinstituttet/Fhi.Smittestopp.App/issues)

** This app has been retired as of August 10th 2022, see [information here](https://www.fhi.no/nyheter/2022/fhi-legger-ned-smittestopp/)**

The goal of this open-source project is to develop and maintain the official Smittestopp mobile app in Norway based on the Exposure Notification API from [Apple](https://www.apple.com/covid19/contacttracing/) and [Google](https://www.google.com/covid19/exposurenotifications/). This repository contains the code for frontend mobile application (iOS and Android). The app uses Xamarin cross-platform solution for Exposure Notification: [Nuget](https://www.nuget.org/packages/Xamarin.ExposureNotification) and [Source](https://github.com/xamarin/XamarinComponents/tree/master/XPlat/ExposureNotification).

If you are interested in backend server implementation, check out https://github.com/folkehelseinstituttet/Fhi.Smittestopp.Backend.

## Documentation
Documentation is available on GitHub [here](https://github.com/folkehelseinstituttet/Fhi.Smittestopp.Documentation).

Common questions as well as general information about Smittestopp is available on [Norwegian Institute of Public Health](https://www.fhi.no/om/smittestopp/) (Norwegian) and [Helsenorge.no](https://www.helsenorge.no/en/smittestopp/) (English) webpages.

## Azure Pipelines status (build and test)

|    Branch    | Status  |
|--------|---|
| master | [![Build Status](https://fhi.visualstudio.com/Fhi.Smittestopp/_apis/build/status/folkehelseinstituttet.Fhi.Smittestopp.App?branchName=master)](https://fhi.visualstudio.com/Fhi.Smittestopp/_build/latest?definitionId=268&branchName=master)  |
| dev    | [![Build Status](https://fhi.visualstudio.com/Fhi.Smittestopp/_apis/build/status/folkehelseinstituttet.Fhi.Smittestopp.App?branchName=dev)](https://fhi.visualstudio.com/Fhi.Smittestopp/_build/latest?definitionId=268&branchName=dev)  |

### App Center builds

|            | iOS | Android |
|------------|-----|---------|
| Alpha (Dev)      |[![Build status](https://build.appcenter.ms/v0.1/apps/792affd9-35ea-4b53-8b32-b3a62c760300/branches/dev/badge)](https://appcenter.ms)     |   [![Build status](https://build.appcenter.ms/v0.1/apps/08d51545-9c04-43e2-a2a2-a2ef34cd87df/branches/dev/badge)](https://appcenter.ms)      |
| Beta       | [![Build status](https://build.appcenter.ms/v0.1/apps/cad9c263-729f-482a-9eab-8b32d3510909/branches/dev/badge)](https://appcenter.ms)    |     [![Build status](https://build.appcenter.ms/v0.1/apps/a78a302f-01af-4280-a409-d0d6975e726b/branches/dev/badge)](https://appcenter.ms)    |
| Pre-production |  [![Build status](https://build.appcenter.ms/v0.1/apps/cad9c263-729f-482a-9eab-8b32d3510909/branches/master/badge)](https://appcenter.ms)   |    [![Build status](https://build.appcenter.ms/v0.1/apps/a78a302f-01af-4280-a409-d0d6975e726b/branches/master/badge)](https://appcenter.ms)     |
| Production |  [![Build status](https://build.appcenter.ms/v0.1/apps/e4af3bb5-44d0-47ae-9f80-714f44f231ab/branches/master/badge)](https://appcenter.ms)   |    [![Build status](https://build.appcenter.ms/v0.1/apps/ff50860e-7ced-40ce-87cd-04e3f9e5ff8a/branches/master/badge)](https://appcenter.ms)     |

## Development
### Prerequisites
- Visual Studio 2019
- Xcode 12 or higher (iOS only)

### Getting started
1. Clone this repository using `git clone https://github.com/folkehelseinstituttet/Fhi.Smittestopp.App.git`
2. Open the solution file `NDB.Covid19.sln` in Visual Studio
3. Restore Nuget Packages
4. Build the project and run it.

### Project structure
The app is written in Xamarin (C#) and platform specific UI implementation (Android XML and UIStoryboards) for additional flexibility when working with UI.
Overall, the solution contains four projects:
- **NDB.Covid19:** Contains shared business logic between iOS and Android, i.e., Exposure Notifications handler, locales, log utils, models, viewModels, services.<br/><br/>
- **NDB.Covid19.Droid:** Android related code, UI Activities/Fragments, implementation of services and handlers (for Dependency Injection) etc.<br/><br/>
- **NDB.Covid19.iOS:** iOS related code, UI Storyboards/ViewControllers, implementation of services and handlers (for Dependency Injection) etc.<br/><br/>
- **NDB.Covid19.Test:** Unit and integration tests.<br/><br/>
- **IdentityServerMock:** Contains Identity Server 4 mock for tests purposes.

## Contributing
Feedback and contribution are always welcome. For more information about how to contribute, refer to [Contribution Guidelines](CONTRIBUTING.md). By contributing to this project, you also agree to abide by its [Code of Conduct](CODE_OF_CONDUCT.md) at all times.

## Download Smittestopp
<a href="https://play.google.com/store/apps/details?id=no.fhi.smittestopp_exposure_notification"><img style="margin-right: 1%; margin-bottom: 0.5em; float: left;" src="https://www.helsenorge.no/globalassets/mobilapp/badges/google-play-badge-en.png" width="200" height="60" alt="Get it on Google Play"></a>
<a href="https://apps.apple.com/app/id1540967575"><img style="margin-right: 1%; margin-bottom: 0.5em; float: left;" src="https://www.helsenorge.no/globalassets/mobilapp/badges/apple-app-store-badge-en.png" width="180" height="60" alt="Download on the App Store"></a>


## Licence
Copyright (c) 2020 Agency for Digitisation (Denmark), 2020 Norwegian Institute of Public Health (Norway), 2020 Netcompany Group AS

Smittestopp is Open Source software released under the [MIT license](LICENSE.md)

