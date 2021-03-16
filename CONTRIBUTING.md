# How to contribute
Thank you for showing interest in contributing to Smittestopp. By contributing to Smittestopp, you also agree to abide by its [Code of Conduct](CODE_OF_CONDUCT.md) at all times.

## Intro
We use GitHub at all times to work with the codebase. All of the feature development and bugfixes are handled via pull requests submitted to GitHub repo that undergo a review process by team members of FHI organisation. Feel free to comment something you think is important under pull requests and issues at any time.

## Issue creation
Issue is not always a bug you discover in the app but potential enhancements, new features, reporting of code smells or perhaps something else? Begin with checking out if the issue exist from before. In case it is, do not create a duplicate but rather comment under the existing issue.

### Submitting an issue
- Give the issue a short, clear title that describes the bug or feature request
- Include what version of Smittestopp you are using. If you are reporting a bug, include details on the device you get it:
  - _OS Version_: iOS 14.4 or Android 9
  - _Device_, e.g., iPhone SE, iPhone 12, Samsung Galaxy S20
  - _Language_, e.g., Norwegian/English
  - _Google Play Services version_, if you see the issue on Android
- Include steps to reproduce the issue
- Include screenshots, if possible
- Mark issue with an appropriate label, e.g., `is:bug`, `affects:ios`
- Use markdown formatting as appropriate to make the issue and code more readable.
- Do not include any personal information in the issue description and in the screenshots.

### Confirming issues
If you have resources and possess required devices, you can contribute by confirming that the issue exists. We will try to confirm the issue ourself since we have a good set of hardware to test Smittestopp on. However, due to variety of devices, especially on Android, we are not always able to do it. Feel free to contribute by confirming other people's issue by yourself.

### Working on an issue
If you want to fix an existing issue, please claim it first by commenting on the GitHub issue that you will work on it. This prevents duplicated efforts from other contributors on the same issue.

You can start working on the Pull Request after the team assigns the issue to you.

If you have questions about one of the issues, please comment on them.

## Creating pull requests
When you get assigned an issue, you can start working on a pull request to address it. Please ensure to do the following steps:
1. Fetch the latest code. 
2. Create a branch from latest `dev` and give it a proper name, e.g., bugfix/issue-1-crash-on-main-page
3. Create a draft pull request from `your branch` to `dev` and push your changes regularly. You can also indicate your work in progress using labels and WIP in the pull request title.
4. Connect issue and pull request on GitHub so the development process is transparent.
5. Push your code regularly to remote.
6. After you finish, move the pull request from draft, and delete WIP form the title. Add lable `ready_for_review` to your pull request.

After this steps, the team will start a review process of the PR.

## Licence
Copyright (c) 2020 Agency for Digitisation (Denmark), 2020 Norwegian Institute of Public Health (Norway), 2020 Netcompany Group AS

Smittestopp is Open Source software released under the [MIT license](LICENSE.md)
