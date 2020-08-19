# Scraps

Scrap.TF Raffle Joiner

---

![In Action](https://i.imgur.com/3gugL2I.png)

The idea Scraps was conceived during one of my periods of "devious programming".

It uses no exploits, all operations use the website's AJAX endpoints to handle loading of raffles and joining them. The timings are tuned for high efficiency as to avoid hitting rate limits and errors.

## Requirements

Scraps was developed on Windows 10 >=v2004. It will probably work on Windows 8 and maybe even Windows 7 but those are untested.

You will also need to download the .NET Core Runtime installer for Windows from [here](https://dotnet.microsoft.com/download/dotnet-core/3.1) and install it.

![Clicky clicky](https://i.imgur.com/iXnKeqZ.png)

## Usage

### Conventional usage

Simply double-click the Scraps.exe file. The first time it is ran, you will be prompted to enter your `scr_session` cookie value that is stored in your browser while you are logged in to the website. You can paste values into the console window by right-clicking the output area or by right-clicking the window's titlebar, going to Edit, and clicking Paste.

For more info, please consult the [instructions.](https://github.com/depthbomb/Scraps/blob/master/INSTRUCTIONS.md)

### Command line usage

There are a few options that can be passed to Scraps when you run it

* `--verbose`/`-v` - Log debug info to the console window, not really any use to the normal user unless you just want to see more of what's going on
* `--config`/`-c` - Opens your settings file in your OS's default program and closes Scraps

## Help

If you need help with Scraps then send me a DM on Discord `depthbomb#0163`. Avoid joining my Discord server from my GitHub profile as you may get mixed in with other people looking to join for other reasons.

## Disclaimer

This tool is in no way associated with Scrap&#46;TF and using it puts your Scrap&#46;TF at risk of getting banned (although it is probably unlikely).