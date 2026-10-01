# QuanDupeLightsources

Adds an **(Always Lit)** duplicate of each vanilla fire piece to the Hammer, so you can choose per placement
whether a light source follows NoSmokeStayLit's day/night timer or stays lit permanently.

This mod only creates the duplicate pieces. All lit, fuel and timer behaviour comes from
[NoSmokeStayLit](https://thunderstore.io/c/valheim/p/TastyChickenLegs/NoSmokeStayLit/).

## Duplicated pieces

Campfire, bonfire, hearth, sconce, standing iron/wood/green/blue torches, standing brazier, hanging brazier,
jack-o-turnip, stone oven, hot tub, smelter, blast furnace and eitr refinery.

Each duplicate has the same recipe, crafting station and build category as the original. Its prefab name is the
original's with a `Quan_` prefix, e.g. `Quan_piece_groundtorch_wood`.

## Setup

1. Install on the server **and** every client. Clients without it can't connect.
2. In NoSmokeStayLit's config, add the duplicates to **Custom Items Keep Lit**:

```
Quan_fire_pit,Quan_bonfire,Quan_hearth,Quan_piece_walltorch,Quan_piece_groundtorch,Quan_piece_groundtorch_wood,Quan_piece_groundtorch_green,Quan_piece_groundtorch_blue,Quan_piece_brazierfloor01,Quan_piece_brazierceiling01,Quan_piece_jackoturnip,Quan_piece_oven,Quan_piece_bathtub,Quan_smelter,Quan_blastfurnace,Quan_eitrrefinery
```

3. Leave them out of **Custom Items on Timers**.

## Removing the mod

Placed duplicates are separate pieces. If you uninstall the mod, any you've built will be missing from the world.

If you remove NoSmokeStayLit but keep this mod, placed duplicates stay in the world. They then behave exactly like
the vanilla originals: they use fuel normally and have no timer or always-lit behaviour.
