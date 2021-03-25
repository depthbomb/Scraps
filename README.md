# Scraps

Scrap.TF Raffle Bot

---

![In Action](https://i.imgur.com/4Z1l3so.png)

The idea for Scraps was conceived during one of my periods of "devious programming".

It uses no exploits, doesn't rely on slow and clunky browser automation, and all operations use the website's AJAX endpoints to handle loading of raffles and joining them.

## Features

- Fast and lighweight; no need to install Python/Node.JS/Java/etc, doesn't use browser automation
- Easy to set up and configure, simply extract and run
- Detailed logging so you can see everything it does and makes it easier to report problems
- Honeypot/trap raffle detection; avoids fake raffles created by staff members that will get you banned

## Requirements

Scraps was developed on Windows 10 >=v2004 but it will probably work on Windows 8/8.1.

**As of 2.15.0.0**, Scraps is published as "self-contained" which means it shouldn't require the .NET Core 3.1 runtime. However, if you encounter problems then you can get the SDK or Desktop Runtime [here](https://dotnet.microsoft.com/download/dotnet-core/3.1).

![Clicky clicky](https://i.imgur.com/j61pqXS.png)

## Usage

### Conventional usage

Before you start, make sure to fill out your `Settings.xml` file. Upon starting the program for the first time, Scraps will open the folder where your settings file is located so you may fill it out. After it is filled out, you may press enter in Scraps to restart it.

For more info, please consult the [instructions.](https://github.com/depthbomb/Scraps/blob/master/INSTRUCTIONS.md)

### Command line usage

There are a few options that can be passed to Scraps when you run it

* `--verbose`/`-v` - Log debug info to the console window, not really any use to the normal user unless you just want to see more of what's going on
* `--config`/`-c` - Opens your settings file in your OS's default program and closes Scraps

## Avoiding Bans

The default settings are set so as to avoid hitting any rate limits and drawing suspicion. Since staff can see if you are using the same IP for multiple accounts, it is highly recommended that you run a VPN alongside Scraps to hide your IP if you are to use multiple accounts.

## Help

If you need help with Scraps then send me a DM on Discord `depthbomb#0163`.

## Disclaimer

This tool is in no way associated with Scrap&#46;TF (obviously) and using this tool is against their community guidelines so there is always a risk your Scrap&#46;TF account may get banned. Use this tool with caution or study it as a proof of concept.