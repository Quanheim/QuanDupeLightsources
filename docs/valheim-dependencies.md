# Valheim Code Dependency

Everything this mod relies on from Valheim, so a game update can be checked quickly. `check-mods.ps1` in the
`.decompiles` repo reads the **Game code** tables and flags any entry the game changed. See "Valheim updates" in
`docs/DEVELOPMENT.md` for the whole check.

**Checked against Valheim:** 1.0.21 (2026-11-02)

Update this file in the same commit as any code that patches, calls or relies on something new in the game, and
update the version line above each time the mod passes the Valheim update check.

## Game code

The first column holds the game class or member in backticks, as `Class` or `Class.Member`. Nested types use their
parent's name, e.g. `Player.PlacementStatus`. Only the first column is read by the script; the rest is for people.

### Harmony patches

None. The mod only registers cloned pieces through Jotunn.

| Game code | Patch | What it does | If the game changes it |
|---|---|---|---|

### Private members reached by reflection

None.

| Game code | Kind | Used for |
|---|---|---|

### Public game code called or read

| Game code | Used for |
|---|---|
| `Piece.m_name` | Clone's name: the base's token plus " (Always Lit)". |
| `Piece.m_description` | Clone's description. |
| `Piece.m_craftingStation` | Clone's crafting station. |
| `Piece.m_resources` | Clone's recipe. |
| `Piece.Requirement` | `m_resItem`, `m_amount`, `m_amountPerLevel` and `m_recover` copied into the clone's recipe. |

### Behaviour the mod assumes

| Game code | Assumption |
|---|---|
| `Fireplace` | The clones are deep copies, so they keep the base's Fireplace. NoSmokeStayLit keeps them lit by prefab name, outside the game's code. |

## Prefabs and assets

Not game code, so no script can check these. Test them in game after each update.

| Prefab or asset | Used for | Breaks if |
|---|---|---|
| `piece_walltorch` | Cloned as `Quan_piece_walltorch`. | Renamed or removed: the clone is skipped with a warning, and placed copies in the world lose their prefab. |
| `piece_groundtorch` | Cloned. | As above. |
| `piece_groundtorch_wood` | Cloned. | As above. |
| `piece_groundtorch_green` | Cloned. | As above. |
| `piece_groundtorch_blue` | Cloned. | As above. |
| `piece_brazierfloor01` | Cloned. | As above. |
| `piece_brazierceiling01` | Cloned. | As above. |
| `piece_jackoturnip` | Cloned. | As above. |
| QuanPieces `Quan_streetlight_brazier`, `Quan_streetlight_lantern` | Cloned as `..._alwayslit`. | QuanPieces renames them; covered by QuanPieces' own check. |
| Hammer piece table | Clones go in the hammer's `QuanAlwaysLit` tab. | Jotunn's `PieceTables.Hammer` changes. |
