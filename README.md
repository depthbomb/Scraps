# Scraps

A Scrap.TF Raffle Bot

---

![In Action](https://i.imgur.com/KRWbtbl.png)

The idea for Scraps was conceived during one of my periods of "devious programming".

It uses no exploits, doesn't rely on slow and clunky browser automation, and all operations use the website's AJAX endpoints to handle loading of raffles and joining them.

## Features

- Cross-platform
- Fast and lighweight; no need to install Python/Node.JS/Java/etc, doesn't use browser automation
- Easy to set up and configure, simply extract and run
- Detailed logging so you can see everything it does and makes it easier to report problems
- Honeypot/trap raffle detection; avoids fake raffles created by staff members that will get you banned

## Requirements

Windows 7+ platforms require the .NET 5 Runtime to be installed which you can download from [here.](https://dotnet.microsoft.com/download/dotnet/5.0)

![Clicky click](https://i.imgur.com/yDF2FpF.png)

Unix builds are self-contained and do not require a runtime to be installed.

## Usage

See [INSTRUCTIONS.md](https://github.com/depthbomb/Scraps/blob/master/INSTRUCTIONS.md)

## Avoiding Bans

The default settings are set so as to avoid hitting any rate limits and drawing suspicion. Since staff can see if you are using the same IP for multiple accounts, it is highly recommended that you run a VPN alongside Scraps to hide your IP if you are to use multiple accounts.

## Help

If you need help with Scraps then send me a DM on Discord `depthbomb#0163`.

## Known Issues

- Windows notifications are currently not working because the official `Microsoft.Toolkit.Uwp.Notifications` library isn't supported on .NET 5 yet (lmao)

## Disclaimer

This tool is in no way associated with Scrap.TF (obviously) and using this tool is against their community guidelines so there is always a risk your Scrap.TF account may get banned. Use this tool with caution or study it as a proof of concept.