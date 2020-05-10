# Scraps

Scrap.TF Raffle Joiner

---

The idea Scraps was conceived during one of my periods of "devious programming". It is built on .NET Core 3.0 and relies on no third-parties aside from [Serilog.](https://serilog.net/)

It uses no exploits, all operations use the website's AJAX endpoints to handle loading of raffles and joining them. The timings are tuned for high efficiency as to avoid hitting rate limits and errors.

Before you even think about using this, see the [disclaimer](#disclaimer) below.

## Usage
The first time you run Scraps, you will be prompted to enter your `scr_session` cookie value. This will then be saved so you won't need to enter it again.

Finally just let it do its thing.

## How I got banned
There are various measures taken by Scrap&#46;TF to prevent these types of programs:
* A rate limit on the raffles index if you refresh too often
  * This rate limit appears to persist so long as you keep accessing the page while limited, resulting in you having to wait a very long time and hope it is gone by the time you check again
* A rate limit if you try to enter a raffle too quickly after just entering one
  * A delay of around 3.5 to 5 seconds will stop you from hitting this
* A second form of rate limiting if you hit the above limit too often, resulting in you being required to enter a CAPTCHA to enter a raffle

The way I _think_ I got caught is from the very first rate limit. While I was tuning the delay on the raffle scanning operation (which, at the time, involved loading the page HTML once before using AJAX to get the rest of the raffles) I got hit with the limit. However, I noticed that you could still send requests to the AJAX endpoints to load raffles. I'm sure it was _totally not suspicious at all_ that I was entering raffles I shouldn't even be able to see because of being rate limited.

Had I waited for this limit to be lifted, I might not have gotten banned. However this is only speculation as I don't know what the website logs and what their methods of detecting this are.

## Disclaimer

This tool is in no way associated with Scrap&#46;TF and using it could likely involve you being banned (like I was).

This tool was initially private but is now open source (albeit no releases are provided) as a way of presenting a proof of concept and to perhaps show how easy it is to make scripts to automate this aspect of the Scrap&#46;TF website in the hope that maybe there can be more protection against it.