# Blazor Wake-on-LAN

[![Build Status](https://dev.azure.com/georg-jung/BlazorWoL/_apis/build/status/georg-jung.BlazorWoL?branchName=master)](https://dev.azure.com/georg-jung/BlazorWoL/_build/latest?definitionId=1&branchName=master)

This is a [Wake-on-LAN](https://en.wikipedia.org/wiki/Wake-on-LAN) app for your network, written in server-side blazor. I developed it for internal use at my workplace and because I wanted to build a small, limited-scope but fully-working and done-right blazor app. [Getting started](#getting-started) is as easy as downloading and running. Feel free to improve/fork/PR this if you think I could have done anything better.

## Download

The platform-specific releases have no prerequisits. Just unpack and double-click. The framework dependend release is smaller and portable but requires [.Net 6 Runtime to be installed](https://dotnet.microsoft.com/download/dotnet-core).

* [Windows x86](https://github.com/georg-jung/BlazorWoL/releases/latest/download/win-x86.zip)
  * This app does not take advantage of 64bit, so I chose to deploy x86 due to the smaller footprint.
* [Windows x86 IIS](https://github.com/georg-jung/BlazorWoL/releases/latest/download/win-x86-iis.zip)
  * All the linked files except from the *Runtime Dependent* one are self contained. Thus, you don't need to have .Net Core installed. They are published as single file executables too (you get i.e. one ~85mb exe file instead of hudnreds of smaller files). To be able to host a .Net Core application in IIS it must not be published as a single file. So, if you want to host BlazorWoL in IIS, you may choose this download.
* [Windows ARM](https://github.com/georg-jung/BlazorWoL/releases/latest/download/win-arm.zip)
* [Linux x64](https://github.com/georg-jung/BlazorWoL/releases/latest/download/linux-x64.zip)
* [Runtime Dependent (.Net 6.0)](https://github.com/georg-jung/BlazorWoL/releases/latest/download/fdd.zip) *previously known as framework dependent*

## Features

![Screenshot](screenshot-01-index.png)

* Wake arbitrary hosts on the network of the server where this is hosted via [Magic Packet](https://superuser.com/a/1066637)
* Add new hosts via the webinterface using either their hostname or their mac address.
* Detect the online status of saved hosts. To determine, they are at the same time [ping](https://en.wikipedia.org/wiki/Ping_(networking_utility))ed and we try to establish a TCP connection on port 3389. This port [is used](https://serverfault.com/a/12006) by the Microsoft Remote Desktop Protocol. This way, we can work with hosts that don't answer normal pings.
* Delete existing hosts from the list.
* When waking a host, the application repeatedly tries to reach the host and updates you about the status. You see immediately when the host finished booting so that you can connect via ssh/RDP/etc..

## Showcase

This application uses the following techniques and might be suitable as a simple but full-fledged example of how they work:

* [Blazor](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)
  * Components and Pages in Razor
  * UI-Server-interaction which would typically require AJAX/writing JavaScript. See the `Wake` page and the *Status* column of the index page.
* .Net 6
* [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/) Code First
  * Automatic Migrations
  * Supports T-SQL and SQLite, selection by connection string in `appsettings.json`
* Dependency Injection using [`Microsoft.Extensions.DependencyInjection`](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection/)
* Continuous Integration using Azure Pipelines.
* Dependency updates are partly automated using [Dependabot](https://dependabot.com/).
* Usage of Microsoft.Extensions.Logging with event ids and custom parameters.
* Usage of Application Insights to keep track of logged application events in production.

## Getting Started

1. [Download](#download) and unzip the [latest release](https://github.com/georg-jung/BlazorWoL/releases/latest/).
2. Run the application by starting `WoL.exe`
3. Open the shown location in a browser (probably `localhost:5000`) and add your first host.

You can add the application to IIS for more serious hosting. You can set up T-SQL as a backend for backup etc.. See the [`appsettings.json` file](WoL/appsettings.json) for details. If you host this in IIS you might want to [configure it to be always running](https://serverfault.com/a/823531), otherwise the first request after some idle time will take seconds.

## Known Limitations

* Adding hosts by hostname is currently impossible on platforms except Windows and Linux, as there is no appropriate ARP API.
