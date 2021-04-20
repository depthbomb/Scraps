# Settings

This page documents the Scraps settings file by describing each setting key's purpose.

---

- `Version`
  - Used to force a backup of old settings while creating a new settings file if this version differs from the application's internal value. Its primary use is to make it easier to add or remove settings in future releases. **DO NOT** modify this yourself.
- `Cookie`
  - Your Scrap.TF `scr_session` cookie
- `EnableToastNotification` = `true`
  - Whether to display a toast notification when you have won any raffles (Windows only)
- `Paranoid` = `false`
  - Whether to enable "Paranoid" mode which will make Scraps act stricter when checking raffles before entering as to avoid honeypots. May result in false positives but better safe than sorry.
- `Delays`
  - `IncrementScanDelay` = `true`
    - Whether to increment the scan delay by 1 second each time a scan returns no raffles until at least one available raffle is found.
  - `ScanDelay` = `5000`
    - The delay between each scan in milliseconds. Setting this below the default may result in rate-limiting.
  - `PaginateDelay` = `500`
    - The delay between scanning each page of raffle results in milliseconds. Setting this below the default may result in the "refreshing too much" error page. However, that does not affect operation but it may appear suspicious.
  - `JoinDelay` = `4000`
    - The delay between joining queued raffles in milliseconds. Setting this below the default may result in captchas appearing due to rate-limiting.
- `Proxies`
  - A list of HTTPS/SSL proxy addresses, including port. Having at least 1 address in this setting key will make Scraps attempt to use the proxies.