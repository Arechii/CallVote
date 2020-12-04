# CallVote
RocketMod 4 plugin for Unturned to vote for things to happen.

## Rewrite Dec 2020
- The plugin has been rewritten for RocketMod 4 (with support for OpenMod coming soon).
- You can customize anything now per vote: color, icon, minimum players, percentage, timer, cooldown, etc.
- Votes can be run simultaneously now (e.g. you can have a kick and day vote running at the same time).
- The custom votes such as AirdropAll, ItemAll, etc. are now standalone commands.
- Third party plugins can add their own votes.
- You can pass arguments to each vote's command.

## Votes
You can turn any vanilla or RocketMod (plugin) command into a vote, along with some custom votes:
- airdropall
- healall
- itemall [id]
- vehicleall [id]
- maxskills

These votes can also be used as standalone commands. The plugin comes with these preconfigured.

## Customization
Each vote can be customized even further:
- name
- alias
- command
- minimum arguments
- color
- icon
- enabled
- minimum players
- required percent
- timer
- cooldown time

The plugin includes default values for these.