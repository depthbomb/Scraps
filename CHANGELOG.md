# Changelog

## 4.2.2.0

The console version of Scraps is now also available as a [Docker image!](https://hub.docker.com/repository/docker/depthbomb/scraps)

- The error reason will be displayed if WebView2 user data cannot be deleted

## 4.2.1.1

- Fixed the Clear User Data button on the settings page being broken

## 4.2.1.0

- Improved WebView2 installation process
- Changed location of WebView2 user data
- Updated to a prerelease version of WebView2
  - Seems to work fine for me
- Application data located in *My Documents\Caprine Logic\Scraps* will be deleted upon uninstallation
  - This includes logs, WebView2 user data, temporary files, and the Scraps CLI application config file
  - Your Scraps GUI application settings will not be affected

## 4.2.0.0

- Greatly improved toast notifications
  - No more useless tray icon!
- Added a debug mode that you can see by launching the program with the `--debug` flag
- Updated WebView2 window design
- WebView2 user data is no longer automatically cleared when you change cookies
  - Make sure to do it manually yourself
- The update routine will run first before initializing settings
- The settings window will now be opened if you try to start the raffle runner without a cookie

## 4.1.5.0

- Added a check for profiles that are not properly set up
- Scraps will now successfully stop if it encounters a problem while obtaining the CSRF token (such as if the account is banned)
- WebView2 user data will now be automatically cleared when the cookie value is changed

## 4.1.4.0

- Updated dependencies
- RegEx patterns are now compiled at runtime which should result in a performance gain for some operations
- Simplified the method for checking for won raffles

## 4.1.3.0

- Added ability to clear the WebView2 user data folder in the settings window
- WebView2 windows will no longer act as a dialog
  - This means that they won't block interaction with the application while they are open

## 4.1.2.0

- Added announcement system
  - This will check the ANNOUNCEMENT file located in this repo and display its message in the console on startup

## 4.1.1.0

- Improved design of WebView windows
- Clicking the raffles won notification will now open the won raffles page in the WebView window

## 4.1.0.0

**THE INSTALLATION FOLDER HAS BEEN CHANGED TO `C:\Users\USERNAME\AppData\Local\Programs\Caprine Logic\Scraps`**

- Added a new **Won Raffles** button that will open a window allowing you to see your won raffles on the Scrap.TF website
  - This feature requires the WebView2 Runtime, which the app will ask you to download if it isn't installed
- Closing the application while the raffle runner is operating will now instantly close instead of waiting for the runner to stop
- Added custom images to the installer to make it look pretty

## 4.0.3.0

- Updated dependencies

## 4.0.2.0

- Common values between the Console and GUI app are now stored in the new **Scraps.Common** library
- Added a small delay after the updater downloads the installer before it will execute it to fix a UI bug

#### Console

- Removed operations related to plugins

## Version 4.0.1.0

- Removed progress bar from status strip

## Version 4.0.0.1

**Scraps is now available as a Windows GUI desktop application.** The cross-platform console application version will remain.

From here on, changes in the changelog will be for the GUI app unless stated otherwise.

#### Desktop

- Settings are now entirely managed through the application
- Added ability to stop and start operations

#### Plugins

Plugins are currently discontinued as their introduction was primarily an excuse for me to practice. However, their functionality may be implemented into the desktop app.

#### Console

- Removed most Windows-only functionality to streamline development and encourage use of the desktop version
  - Removed toast notifications
  - Fixed update process not taking into account the OS it is running on, so Linux builds won't try to download and run an exe file

## Version 3.7.4.0

- Update icon
- Fixed UpdateService using wrong class logger
- The installer executed from updating will now have _Launch Scraps_ checked by default on the finish screen

## Version 3.7.3.0

- Scraps will now stop operation gracefully upon pressing <kbd>CTRL</kbd>+<kbd>C</kbd>
- Re-added the _OnCsrfTokenObtained_ event and added a new _OnPaginate_ event
- Updated third-party libraries

##### ApiServer Plugin

- Scraps can now be stopped by sending a GET request to the `/Stop` endpoint

## Version 3.7.2.1

- Various bug fixes involving updating

## Version 3.7.2.0

- Scraps for Windows is now installed via an installer
  - This makes it easier for the new updater system
  - Scraps is now installed to `Program Files (x86)`
  - Official plugins may also be installed through this process
- Windows 8.1 and x86 releases are discontinued in favor of Windows 10 x64
- Improved update process

## Version 3.7.1.0

- Added a build for `linux-x64` which should work on most distros
- Scraps's data folder on Linux is now created at a new location that does not require elevated permissions to write to

## Version 3.7.0.0

- CSRF token will no longer be logged to make sharing of logs for debugging easier
- Replaced sound notification setting with toast notification setting
- Added option to change the sorting of raffles to scan
- Discontinued self-contained builds because I couldn't be bothered
  - Installing the runtime isn't that bad anyways

## Version 3.6.0.0

- Removed `-c`/`--config` launch switch
- Added config validation
- Improved how backup configs are created
- Dropped support for Windows 7
  - It's time to move on, Windows 10 really isn't that bad
- Self-contained Windows builds (`winX-<arch>-scd`) are now available
  - These include the .NET 5 runtime assemblies with them so you do not need to install it onto your system
  - However, this does mean that the file size of the release will be quite larger

## Version 3.5.0.0

- Added ApiServer optional plugin

## Version 3.4.1.0

- Added CommentOnRaffles optional plugin
- Required directories will now be created when needed, possibly fixes #4

## Version 3.4.0.0

- Added plugins system
- Poll voting via RaffleActions has been replaced with an official plugin
- Replaced logger library with NLog

## Version 3.3.0.0

- Temporarily removed `Microsoft.Toolkit.Uwp.Notifications` package until it is supported
- Added new setting to enable a sound notification for when raffles are won
  - Windows only

## Version 3.2.0.0

- Scraps will look for a config file in the assembly's path if it cannot be found in the usual storage path for whatever reason
- An error message will be displayed on Windows if Scraps needs to be ran with administrator permissions instead of trying to restart with permissions

## Version 3.1.0.0

- Added basic proxy support
  - You can add a list of HTTPS/SSL proxy addresses to your config file and Scraps will test teach one on startup to see if they work
  - If a working proxy could not be found then you will have the option to continue without using a proxy
- Trailing commas are now allowed in the config file
  - This may help users who are unfamiliar with JSON that may accidentally leave trailing commas
- Fixed backup config not being properly created


## Version 3.0.0.1

- Added a 10 second delay after a pagination response returns an error to avoid spamming requests

## Version 3.0.0.0

- Scraps has undergone a complete rewrite that makes it easier to develop new features for
  - Now running on .NET 5 and requires the runtime to be installed (see README for more info)
  - Builds for Windows 7 are now available (untested)
  - Settings file is now in JSON format
    - You will need to edit your new settings file after running for the first time

## Version 2.15.0.0

- Scraps is now supported on Linux
  - Only tested on ARM64 (Raspberry Pi 4, for example)
  - Application data is stored in `/var/lib/scraps`
  - Removed some unimportant features that prevented the port
- Greatly improved toast notifications
- Dropped support for Windows 7

## Version 2.14.0.0

- Due to feature creep, the following features have been removed
  - Stats
  - Discord Rich Presence
- Temporarily removed accepting of site rules since it appears to not be required anymore

Scraps is now compiled to a single executable and binaries for older version of Windows (on different architectures) are available but __untested.__

## Version 2.13.1.0

- Added maintenance check during login
- Added a log line for when Scraps has finished entering raffles in a queue
- You will now be alert of won raffles via the new _Won Raffles_ page and won raffles will be added to the **Entered Raffles** list
  - No "raffled ended, skipping" warning because it treated a won raffle as an available one

## Version 2.13.0.0

- Added paranoid mode
  - This enables stricter checks in an attempt to avoid bans
  - MAY result in skipping legit raffles, but better safe than sorry
- Fixed a typo

## Version 2.12.0.0

- Cleaned up pre-operation log lines
- Added Windows toast notification support for certain events
  - Configurable in your settings file

## Version 2.11.0.0

- Stats are now saved via `System.Configuration.ConfigurationManager`, which should be more reliable and prevent corruption
- Entered raffle IDs will no longer be saved
  - This could cause problems when switching to a different account
- There will no longer be a delay after joining if the queue only contains one raffle
- Updated separator in statuses
- You will now be alerted if you've won any raffles

## Version 2.10.0.0

- Scan delays and join delays are now configurable in your settings file
- Entered raffles are now stored in your stats file
- Successful poll answering log lines will no longer be displayed
- Site rules are now accepted upon logging in (if needed)
- Stats are now also saved after each raffle join

## Version 2.9.0.0

- Added Discord Rich Presence support because why not
- Added persistent raffles joined count
  - Only raffles joined on or after this version of Scraps will be counted
- Added status for answering raffle polls
- Fixed polls not working if they allow multiple choice
- Chosen poll choice now shows the number as it appears rather than its programmatic number
- Added pluralization to poll log lines
- Updated app icon

## Version 2.8.0.0

- Added option to vote in polls
  - This chooses a random option from the list of available poll options
- Added message if you are running the latest program version
- Modified log lines that included instances of the word **Enter**
  - Enter -> \[Enter\]

## Version 2.7.1.0

- Fixed verbose mode not working

## Version 2.7.0.0

- Rewrote settings handling
- New releases are now checked for at the very start of the program

## Version 2.6.1.0

- Added an additional warning image to honeypot checker
- Improved handling of pagination errors
- Logging levels now follow a standard
  - Warning, Error, and Fatal will no longer be used for events that do not impact operation of the program

## Version 2.6.0.0

- Added invalid max entries check to honeypot checker
- Changed banned entry users check to look for at least 2 users instead of 3

## Version 2.5.2.0

- Removed debug logging inside honeypot checker

## Version 2.5.1.0

- Added honeypot raffle detection
  - These are modified raffles that ban you upon entering
- Improved error messages while paginating
- Program will now stop operations if it detects that the account has been banned
- Fixed status briefly saying the program is outdated when starting up
- Updated status formatting

## Version 2.4.0.0

- Added update checking

## Version 2.3.2.0

- Ended raffles will now be detected and skipped instead of displaying a "hash not found" warning

## Version 2.3.1.0

- Improved error handling for pagination operation
- Log file intervals are now by month and some levels are split into their own file

## Version 2.3.0.0

- Added raffles joined during session to titlebar
- Added a delay before joining the last raffle in a queue, eliminating a join error

## Version 2.2.0.0

- Removed jumplist functionality
- Added status to window title, useful for seeing what is going on when not running in verbose mode
- Fixed the program not working after today's website update

## Version 2.1.0.0

- The appropriate referer header is sent when joining a raffle
- Removed disclaimer
- Added setting to enable rescan delay incrementing

## Version 2.0.0.0

- Complete rewrite!
  - No longer uses browser automation for functionality
  - Greatly improved speeds
  - Improved logging

## Version 1.1.0.0

- Number of open raffles to join are now displayed after the loading step
- Bot will prompt you to open the settings file if the cookie value has not been modified since created
- Project version is now displayed in title
- Added option to run in "headless" mode (enabled by default)
- Adjusted messages to be less spammy
- Updated alert sound
- Alerts sound will now only play once until another raffle is won
- Added support for withdrawing items from won raffles
  - You will still need to confirm the trade offer on Steam

## Version 1.0.0.0

- First release!