<p align="center">
	<table>
		<tbody>
			<td align="center">
				<h1>Scraps</h1>
				<p>A Scrap.TF Raffle Bot</p>
				<p>
					<a href="https://github.com/depthbomb/Scraps/commits"><img src="https://img.shields.io/github/last-commit/depthbomb/Scraps.svg?label=Updated&logo=github&style=flat-square"></a>
					<img src="https://img.shields.io/github/repo-size/depthbomb/Scraps.svg?label=Repo%20Size&logo=github&style=flat-square">
					<a href="https://github.com/depthbomb/Scraps/releases"><img src="https://img.shields.io/github/downloads/depthbomb/Scraps/total.svg?label=Downloads&logo=github&style=flat-square"></a>
					<a href="https://github.com/depthbomb/Scraps/blob/main/LICENSE"><img src="https://img.shields.io/github/license/depthbomb/Scraps.svg?label=License&logo=apache&style=flat-square"></a>
				</p>
				<p>
					<a href="https://github.com/depthbomb/Scraps/releases/latest"><img src="https://img.shields.io/github/release/depthbomb/Scraps.svg?label=Stable&logo=github&style=flat-square"></a>
					<a href="https://github.com/depthbomb/Scraps/releases/latest"><img src="https://img.shields.io/github/release-date/depthbomb/Scraps.svg?label=Released&logo=github&style=flat-square"></a>
					<a href="https://github.com/depthbomb/Scraps/releases/latest"><img src="https://img.shields.io/github/downloads/depthbomb/Scraps/latest/total.svg?label=Downloads&logo=github&style=flat-square"></a>
				</p>
				<p>
					<a href='https://ko-fi.com/O4O1DV77' target='_blank'><img height='36' src='https://cdn.ko-fi.com/cdn/kofi1.png?v=3' alt='Buy Me a Coffee at ko-fi.com' /></a>
				</p>
				<img width="2000" height="0">
			</td>
		</tbody>
	</table>
</p>

<p align="center">
	<img src="https://i.imgur.com/xrXwZj7.png">
</p>

The idea for Scraps was conceived during one of my periods of "devious programming".

It uses no exploits, doesn't rely on slow and clunky browser automation, and all operations use the website's AJAX endpoints to handle loading of raffles and joining them.

## Features

- Fast and lighweight; no need to install Python/Node.JS/Java/etc, doesn't use browser automation
- Configurable
- Detailed logging so you can see everything it does and makes it easier to report problems
- Honeypot/trap raffle detection; avoids fake raffles created by staff members that will get you banned

## Requirements

- [.NET Desktop Runtime 6.x](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

## Finding your Cookie

Scraps requires your Scrap.TF session cookie, `scr_session` to function. This can be found by accessing your browers's dev tools (usually by pressing `F12`), going to **Application** and then **Cookies**. This process may differ depending on your browser.

## Launch Arguments

- `/Debug` Enable logging debug info to the app's log window
- `/Silent` Launches Scraps minimized and immediately starts the raffle runner
- `/SkipUpdates` Skips the update check
- `/SkipAnnouncements` Skips announcement fetching

## Avoiding Bans

The default settings are set so as to avoid hitting any rate limits and drawing suspicion. Since staff can see if you are using the same IP for multiple accounts, it is highly recommended that you run a VPN alongside Scraps to hide your IP if you are to use multiple accounts.

## Disclaimer

This tool is in no way associated with Scrap.TF (obviously) and using this tool is against their community guidelines so there is always a risk your Scrap.TF account may get banned. Use this tool with caution.
