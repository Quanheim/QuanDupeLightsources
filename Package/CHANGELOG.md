# Changelog

## 1.1.1 - 2026-11-16

- NoSmokeStayLit and QuanPieces are no longer required dependencies, so they don't install automatically any more:
  install them yourself. This mod still loads after them when they're there. Nothing else changes: the vanilla
  duplicates are always added (without NoSmokeStayLit they work like the originals), and the street light
  duplicates are added whenever QuanPieces is installed. The log says which were added and what's missing.
- Now on Hexium too, which doesn't carry NoSmokeStayLit.

## 1.1.0 - 2026-10-26

- Added (Always Lit) duplicates of QuanPieces' street lights: `Quan_streetlight_brazier_alwayslit` and
  `Quan_streetlight_lantern_alwayslit`.
- QuanPieces and NoSmokeStayLit are now required dependencies, installed automatically with this mod.
- All duplicates now sit together in their own QuanAlwaysLit tab on the hammer, instead of each original's tab.
  Pieces already placed are unaffected.

## 1.0.0 - 2026-10-01

- Initial release.
- Adds "(Always Lit)" duplicates of the 8 pieces NoSmokeStayLit can put on a timer (sconce, four standing torches,
  standing and hanging braziers, jack-o-turnip), prefixed `Quan_`, for use with its Custom Items Keep Lit list.
