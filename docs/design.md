# QuanDupeLightsources: design

What the mod does is in `README.md` (the repo's and `Package/README.md`); the game code and prefabs it relies on are
in `docs/valheim-dependencies.md`. This file holds decisions and open items.

## Open items

### Soft dependencies instead of hard ones (Quandru, 16 Nov 2026)

**Now (1.1.0):** hard dependencies on NoSmokeStayLit (`TastyChickenLegs-NoSmokeStayLit`) and QuanPieces, as
`<ThunderstoreDependency>` items in the `.csproj` and, for QuanPieces, a `[BepInDependency]` in the plugin. Because
NoSmokeStayLit isn't on Hexium, the Publish workflow holds this mod back from Hexium (`missing-dependencies` in
`.publishing/status.json`), so it's the one Quandru mod still installed from Thunderstore in the server profile.

**Wanted:**
- No hard dependencies. Soft `[BepInDependency(..., BepInDependency.DependencyFlags.SoftDependency)]` on
  NoSmokeStayLit and QuanPieces, so the mod loads after them when they're installed but doesn't require them. Drop
  both `<ThunderstoreDependency>` items, so the manifest no longer lists them and Hexium can take the mod.
- Register each duplicate only when what it's for is present:
  - the vanilla duplicates only when NoSmokeStayLit is loaded;
  - the street light duplicates only when QuanPieces is loaded (and, presumably, NoSmokeStayLit too, since
    without it they're the same as the originals).
- Without NoSmokeStayLit a duplicate would work exactly like the vanilla original; which mods to install is the
  player's (or server owner's) choice, and the readme says so.
- Log at start-up which groups were registered and which were skipped and why, per the logging convention.

**To settle before building:**
1. **Placed duplicates when a mod is removed.** If a duplicate isn't registered (say NoSmokeStayLit is removed
   from a world that has them), placed copies have no prefab: the game doesn't load them, so they vanish from the
   world (and come back if the mod returns). The alternative is to always register the duplicates and only use the
   soft dependencies for load order, so they behave as plain fires when NoSmokeStayLit is absent. Quandru's call.
2. **Server and players must agree.** Building pieces must exist on everyone, so a player missing NoSmokeStayLit
   would lack pieces the server has. Fine if the server profile always carries both mods; worth a line in the
   readme for other servers.
3. **NoSmokeStayLit's plugin GUID** for the soft dependency and the "is it loaded" check (read it from its DLL).
   QuanPieces' is `quandru.QuanPieces`.
