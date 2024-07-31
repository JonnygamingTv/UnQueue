# UnQueue
RocketMod plugin to jump through queue
```xml
<?xml version="1.0" encoding="utf-8"?>
<Config xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <sync>false</sync>
  <BypassMaxPlayers>true</BypassMaxPlayers>
  <PrependPosition>0</PrependPosition>
  <MaxPlayers>0</MaxPlayers>
  <ReservedSlots>0</ReservedSlots>
  <Interval>5</Interval>
  <Permission>bypass.queue</Permission>
</Config>
```

`<sync>` is whether or not the plugin should run its' checker synchronously (will freeze server for about a ms every check if so)
> Recommended: `false`

`<BypassMaxPlayers>` if true; infinite reservedslots
> Recommended: `true`, or `false` if you have `<ReservedSlots>` configured

`<PrependPosition>` if player is behind this position in queue, put them first in queue. 
> Recommended: `0` (`0` = first in queue, so if behind first in queue, put them first)

`<MaxPlayers>` the maxplayer count that the server maxplayers will go down to when people who bypassed queue (reservedslots) leave (basically)
> Recommended: `24`, or whatever value you want to be the default playercount.
> 
> Tldr: max amount of players without permission (`0` = disabled)

`<ReservedSlots>` if `<MaxPlayers>` (e.g `24`) is set to something lower than `<ReservedSlots>` (e.g. `48`), it will make slots 25-48 permission-restricted
> Recommended: `48`

> Tldr: Real maxplayer count

`<Interval>` is how often the plugin should check the queue list, I didn't find a event to listen to for this purpose, so that is why it uses a timer instead. Should be fine since it runs asynchronously by default.
> Recommended: `10` (seconds), depends how long you wish to wait in queue in such scenarios

`<Permission>` the permission to check for to bypass queue.
> Recommended: `bypass.queue`
