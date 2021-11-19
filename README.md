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
	<img src="https://i.imgur.com/VOYwJYP.png">
</p>

The idea for Scraps was conceived during one of my periods of "devious programming".

It uses no exploits, doesn't rely on slow and clunky browser automation, and all operations use the website's AJAX endpoints to handle loading of raffles and joining them.

## Features

- Cross-platform version included
- Fast and lighweight; no need to install Python/Node.JS/Java/etc, doesn't use browser automation
- Easy to set up and configure, simply extract and run
- Detailed logging so you can see everything it does and makes it easier to report problems
- Honeypot/trap raffle detection; avoids fake raffles created by staff members that will get you banned
- Basic proxy support

## Using the Docker version

The Docker version of the console application allows you to easily run Scraps without needing to install anything onto your system. The Docker version has many features disabled and requires your cookie value to be passed to it as the very first argument when running it.

## Requirements

The **.NET Desktop Runtime 5.0.x** is required to run both the GUI and Console application on Windows. The runtime installer can be downloaded [here.](https://dotnet.microsoft.com/download/dotnet/5.0)

Unix builds are self-contained and do not require a runtime to be installed.

## Console Version Usage

See [INSTRUCTIONS.md](https://github.com/depthbomb/Scraps/blob/master/INSTRUCTIONS.md)

## Avoiding Bans

The default settings are set so as to avoid hitting any rate limits and drawing suspicion. Since staff can see if you are using the same IP for multiple accounts, it is highly recommended that you run a VPN alongside Scraps to hide your IP if you are to use multiple accounts.

## Help

If you need help with Scraps then send me a DM on Discord `depthbomb#0163`.

## Disclaimer

This tool is in no way associated with Scrap.TF (obviously) and using this tool is against their community guidelines so there is always a risk your Scrap.TF account may get banned. Use this tool with caution or study it as a proof of concept.
