# Changelog

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