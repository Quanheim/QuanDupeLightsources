# QuanDupeLightsources

A Valheim plugin that adds **(Always Lit)** duplicates of vanilla fire pieces and of QuanPieces' street lights,
for use with NoSmokeStayLit's Custom Items Keep Lit list. Since 1.1.1 NoSmokeStayLit and QuanPieces are soft
dependencies (`BepInDependency` with `SoftDependency`, for load order only; no Thunderstore dependencies): the vanilla
duplicates are always registered, and the street light ones whenever QuanPieces is loaded. See `docs/design.md` for
why, and `Package/README.md` for the player-facing details.

Building and releasing: see `docs/DEVELOPMENT.md`.
