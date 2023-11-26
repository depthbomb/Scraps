> [!IMPORTANT]
> Scraps development has ended due to a combination of tighter anti-bot measures to attempt to work around and my lack of interest (considering I haven't used the software myself to gain items in over a year, raffles won during development were not claimed).
>
> The repository is archived and thus won't accept any contributions. If you wish to continue development of the project then you can fork the repository.

---

<p align="center">
	<table>
		<tbody>
			<td align="center">
				<h1>Scraps</h1>
				<p>A Scrap.TF Raffle Joining Bot</p>
				<p>
					<a href="https://github.com/depthbomb/Scraps/commits"><img src="https://img.shields.io/github/last-commit/depthbomb/Scraps.svg?label=Updated&logo=github&style=flat-square&cacheSeconds=60"></a>
					<img src="https://img.shields.io/github/repo-size/depthbomb/Scraps.svg?label=Repo%20Size&logo=github&style=flat-square&cacheSeconds=60">
					<a href="https://github.com/depthbomb/Scraps/releases"><img src="https://img.shields.io/github/downloads/depthbomb/Scraps/total.svg?label=Total%20Downloads&logo=github&style=flat-square&cacheSeconds=60"></a>
					<a href="https://github.com/depthbomb/Scraps/blob/main/LICENSE"><img src="https://img.shields.io/github/license/depthbomb/Scraps.svg?label=License&logo=apache&style=flat-square&cacheSeconds=60"></a>
				</p>
				<p>
					<a href="https://github.com/depthbomb/Scraps/releases/latest"><img src="https://img.shields.io/github/release/depthbomb/Scraps.svg?label=Stable&logo=github&style=flat-square&cacheSeconds=60"></a>
					<a href="https://github.com/depthbomb/Scraps/releases/latest"><img src="https://img.shields.io/github/release-date/depthbomb/Scraps.svg?label=Released&logo=github&style=flat-square&cacheSeconds=60"></a>
					<a href="https://github.com/depthbomb/Scraps/releases/latest"><img src="https://img.shields.io/github/downloads/depthbomb/Scraps/latest/total.svg?label=Release%20Downloads&logo=github&style=flat-square&cacheSeconds=60"></a>
				</p>
				<img width="2000" height="0">
			</td>
		</tbody>
	</table>
</p>

<p align="center">
	<img src="https://i.imgur.com/Cn8r17B.png">
</p>

The idea for Scraps was conceived during one of my periods of "devious programming".

It uses no exploits, doesn't rely on slow and clunky browser automation, and all operations use the website's AJAX endpoints to handle loading of raffles and joining them.

## Features

- Fast and lightweight; no need to install Python/Node.JS/Java/etc, doesn't use browser automation
- Configurable
- Detailed logging so you can see everything it does and makes it easier to report problems

## Requirements

- Windows 10, version 2004 (10.0.19041.x)
- [.NET 7 Desktop Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)

## Finding your Cookie

Scraps requires your Scrap.TF session cookie, `scraptf` to function. This can be found by accessing your browser's dev tools (usually by pressing `F12`), going to **Application** and then **Cookies**. This process may differ depending on your browser.

## Launch Flags

- `/Debug` Enable debug mode

## Avoiding Bans

The default settings are set so as to avoid hitting any rate limits and drawing suspicion. Since staff can see if you are using the same IP for multiple accounts, it is highly recommended that you run a VPN alongside Scraps to hide your IP if you are to use multiple accounts.

## I can't use Scraps because of Cloudflare!

This usually means that Scrap.TF manually enabled more stricter protection that prevents from software like Scraps from working. No attempts will be made by Scraps to bypass this protection. Please do not create issues regarding this as they will be closed with no response.
