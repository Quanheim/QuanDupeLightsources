# QuanDupeLightsources: design

What the mod does is in `README.md` (the repo's and `Package/README.md`); the game code and prefabs it relies on are
in `docs/valheim-dependencies.md`. This file holds decisions and open items.

## Open items

### Done in 1.1.1: soft dependencies instead of hard ones (Quandru, 16 Nov 2026)

**Now (1.1.0):** hard dependencies on NoSmokeStayLit (`TastyChickenLegs-NoSmokeStayLit`) and QuanPieces, as
`<ThunderstoreDependency>` items in the `.csproj` and, for QuanPieces, a `[BepInDependency]` in the plugin. Because
NoSmokeStayLit isn't on Hexium, the Publish workflow holds this mod back from Hexium (`missing-dependencies` in
`.publishing/status.json`), so it's the one Quandru mod still installed from Thunderstore in the server profile.

**Decided (Quandru, 16 Nov 2026):**
- **No hard dependencies.** Soft `[BepInDependency(..., BepInDependency.DependencyFlags.SoftDependency)]` on
  NoSmokeStayLit and QuanPieces, **for load order only**. Drop both `<ThunderstoreDependency>` items, so the manifest
  no longer lists them and Hexium can take the mod.
- **Always register the duplicates.** The vanilla duplicates are registered whether or not NoSmokeStayLit is
  installed; without it they work exactly like the originals, and which mods to install is the player's or server
  owner's choice (the readme says so). Placed duplicates therefore never lose their prefab when NoSmokeStayLit is
  removed.
- **Street light duplicates need QuanPieces:** they're clones of its street lights, so they're registered only when
  QuanPieces is loaded (nothing to clone otherwise). That's the one conditional group.
- **Log at start-up** which groups were registered, and that the street light group was skipped (and why) when
  QuanPieces is missing, per the logging convention.
- **Server and player mismatch isn't a concern** (Quandru): each mod's own version check already makes server and
  players match on the mods they have. A player without QuanPieces can't join a server that has it, so they can't
  end up missing just the street light duplicates.

**Built in 1.1.1 (16 Nov 2026):** soft dependencies on `quandru.QuanPieces` and `TastyChickenLegs.NoSmokeStayLit`
(read from NoSmokeStayLit 2.4.4's DLL); street light group gated on `Chainloader.PluginInfos`; both
`<ThunderstoreDependency>` items removed; readmes, changelog and dependency doc updated. A patch version: no new
pieces and no change in behaviour (Quandru). Quandru judged the risk negligible and released it without an in-game
test.