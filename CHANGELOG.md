# Changelog

## Version 2.7.0.0

- Added proxy support

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