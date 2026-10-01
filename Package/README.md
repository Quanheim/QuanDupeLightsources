# QuanDupeLightsources

Adds an **(Always Lit)** duplicate of each light source that NoSmokeStayLit can put on a day/night timer, so you
can choose per placement whether it follows the timer or stays lit permanently. For example, timed torches
outside and always-lit ones in buildings with no natural light.

This mod only creates the duplicate pieces. All lit, fuel and timer behaviour comes from
[NoSmokeStayLit](https://thunderstore.io/c/valheim/p/TastyChickenLegs/NoSmokeStayLit/).

## Duplicated pieces

Sconce, standing iron/wood/green/blue torches, standing brazier, hanging brazier and jack-o-turnip: the pieces
with an "on timer" setting in NoSmokeStayLit. Other fire pieces (campfire, hearth, smelters and so on) have no timer,
so an always-lit duplicate of them would behave exactly like the original.

Each duplicate has the same recipe, crafting station and build category as the original. Its prefab name is the
original's with a `Quan_` prefix, e.g. `Quan_piece_groundtorch_wood`.

## Setup

1. Install on the server **and** every client. Clients without it can't connect.
2. In NoSmokeStayLit's config, add the duplicates to **Custom Items Keep Lit** (comma-separated, after any
   existing entries):

```
Quan_piece_walltorch,Quan_piece_groundtorch,Quan_piece_groundtorch_wood,Quan_piece_groundtorch_green,Quan_piece_groundtorch_blue,Quan_piece_brazierfloor01,Quan_piece_brazierceiling01,Quan_piece_jackoturnip
```

3. Leave them out of **Custom Items on Timers**.

## Removing the mod

Placed duplicates are separate pieces. If you uninstall the mod, any you've built will be missing from the world.

If you remove NoSmokeStayLit but keep this mod, placed duplicates stay in the world. They then behave exactly like
the vanilla originals: they use fuel normally and have no timer or always-lit behaviour.
