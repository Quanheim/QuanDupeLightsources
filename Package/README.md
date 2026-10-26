# QuanDupeLightsources

Adds an **(Always Lit)** duplicate of each light source that NoSmokeStayLit can put on a day/night timer, so you
can choose per placement whether it follows the timer or stays lit permanently. For example, timed torches
outside and always-lit ones in buildings with no natural light.

This mod only creates the duplicate pieces. All lit, fuel and timer behaviour comes from
[NoSmokeStayLit](https://thunderstore.io/c/valheim/p/TastyChickenLegs/NoSmokeStayLit/), which is a required
dependency and installs alongside this mod. If you just want the street lights as ordinary fuelled pieces, install
[QuanPieces](https://thunderstore.io/c/valheim/p/Quandru/QuanPieces/) on its own instead.

## Duplicated pieces

**Vanilla:** sconce, standing iron/wood/green/blue torches, standing brazier, hanging brazier and jack-o-turnip:
the pieces with an "on timer" setting in NoSmokeStayLit. Other fire pieces (campfire, hearth, smelters and so on)
have no timer, so an always-lit duplicate of them would behave exactly like the original. Each duplicate's prefab
name is the original's with a `Quan_` prefix, e.g. `Quan_piece_groundtorch_wood`.

**[QuanPieces](https://thunderstore.io/c/valheim/p/Quandru/QuanPieces/):** the Brazier Street Light and the
Dvergr Street Light. Their duplicates' prefab names have an `_alwayslit` suffix, e.g.
`Quan_streetlight_brazier_alwayslit`. QuanPieces is also a required dependency and installs alongside this mod.

Each duplicate has the same recipe and crafting station as the original, and they're all together in their own
**QuanAlwaysLit** tab on the hammer.

## Setup

1. Install on the server **and** every client. Clients without it can't connect.
2. In NoSmokeStayLit's config, add the duplicates to **Custom Items Keep Lit** (comma-separated, after any
   existing entries):

   ```
   Quan_piece_walltorch,Quan_piece_groundtorch,Quan_piece_groundtorch_wood,Quan_piece_groundtorch_green,Quan_piece_groundtorch_blue,Quan_piece_brazierfloor01,Quan_piece_brazierceiling01,Quan_piece_jackoturnip,Quan_streetlight_brazier_alwayslit,Quan_streetlight_lantern_alwayslit
   ```

3. Leave them out of **Custom Items on Timers**.
4. To put the QuanPieces originals on the timer, add `Quan_streetlight_brazier,Quan_streetlight_lantern` to
   **Custom Items on Timers**.

## Removing the mod

Placed duplicates are separate pieces. If you uninstall the mod, any you've built will be missing from the world.

If you remove NoSmokeStayLit but keep this mod, placed duplicates stay in the world. They then behave exactly like
the vanilla originals: they use fuel normally and have no timer or always-lit behaviour.
