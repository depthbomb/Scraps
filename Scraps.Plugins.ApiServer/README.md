# ApiServer Plugin

This plugin starts a local HTTP server on `localhost:19318` that can be used to access some info on the running bot.

Current endpoints:

- `/Stats`
  - Returns number of raffles joined in the session, current outstanding won raffles, and the time the bot was started (in UTC)