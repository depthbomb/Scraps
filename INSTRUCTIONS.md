# Instructions

---

## Disclaimer

Scraps is in no way associated with _Scrap.TF_. Using this program is in violation of Scrap.TF's community guidelines. By using this program you risk getting banned from Scrap.TF.

**You have been warned!**

---

## Before you start

In order for Scraps to work, it needs your Scrap.TF cookie. Scraps will then use this cookie to "log in" as you so it may enter raffles on your behalf.

1. Visit Scrap.TF in a browser where you are logged into the site
2. Go to your browser's developer tools, click Application at the top, and open up the Cookies menu on the side [(Image)](https://i.imgur.com/mJ3hfnr.png)
3. Copy the entire value for the cookie named `scr_session` and paste it in your **Config.json** file.
4. Scraps will remain logged in as you so long as you do not log out of Scrap.TF

If you need to give Scraps a new cookie value, you can

- Run Scraps with the `--config` flag to open your settings file so you may change the cookie
- Delete your **Config.json** file and restart the program
  - This will reset _all_ of your settings in the process

## Configuring Scraps

See [SETTINGS.md](https://github.com/depthbomb/Scraps/blob/master/SETTINGS.md)

## Command line usage

There are a few options that can be passed to Scraps when you run it

* `--verbose`/`-v` - Log debug info to the console window, not really any use to the normal user unless you just want to see more of what's going on
* `--config`/`-c` - Opens your settings file in your OS's default program and closes Scraps

## Updating Scraps

To update the program, simply download the latest release, and extract the files into your existing Scraps installation directory. Your settings will remain unchanged.