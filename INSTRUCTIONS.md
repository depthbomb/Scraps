## Disclaimer

Scraps is in no way associated with _Scrap.TF_. Using this program is in violation of Scrap.TF's community guidelines. By using this program you risk getting banned from Scrap.TF.

**You have been warned!**

---

## Before you start

In order for Scraps to work, it needs your Scrap.TF cookie. Scraps will then use this cookie to "log in" as you so it may enter raffles on your behalf.

1. Visit Scrap.TF in a browser where you are logged into the site
2. Go to your browser's developer tools, click Application at the top, and open up the Cookies menu on the side [(Image)](https://i.imgur.com/mJ3hfnr.png)
3. Copy the entire value for the cookie named `scr_session` and paste it into the program when it asks for it
    - You can right-click into the program window to paste
    - You can also paste by right-clicking the top border of the window, hovering over **Edit** and clicking **Paste**
4. Your cookie will be saved to your settings file `Settings.xml` located in **My Documents\Caprine Logic\Scraps**
5. Scraps will remain logged in as you so long as you do not log out of Scrap.TF

If you need to give Scraps a new cookie value, you can

- Run Scraps with the `--config` flag to open your settings file so you may change the cookie
- Delete your `Settings.xml` file and restart the program
  - This will reset _all_ of your settings in the process

## Using a proxy

Since [**2.7.0.0**](https://github.com/depthbomb/Scraps/releases/tag/2.7.0.0-PreRelease) Scraps can use a proxy to hide your IP address so you can't be detected as an alt account of a banned user. Follow the steps below to use this new feature:

1. Create a `proxies.txt` file in the same location as `Scraps.exe`
2. Add proxy addresses to this file, separated by one line break
   - Only `http://` proxies are supported
   - Do not include the `http://` portion of the address, only the IP/host + port
3. Start Scraps with the `--proxy` argument
4. Scraps will iterate through the proxies file and stop and use the first proxy address that changes your IP address.

## Updating Scraps
To update the program, simply download the latest release, run the extractor and overwrite all of the files in your existing Scraps installation. Your settings will remain unchanged as they are stored elsewhere.