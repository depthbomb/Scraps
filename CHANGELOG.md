# Changelog

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