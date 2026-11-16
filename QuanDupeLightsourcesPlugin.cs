using System.Linq;
using BepInEx;
using BepInEx.Bootstrap;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using Jotunn.Utils;

namespace QuanDupeLightsources
{
    /// <summary>
    /// Registers identical, differently-named copies of the fire pieces NoSmokeStayLit can put on a timer: the
    /// vanilla ones, and QuanPieces' street lights. No patches, no runtime logic: lit/fuel/timer state is left
    /// entirely to NoSmokeStayLit, which picks the clones up via its "Custom Items Keep Lit" list.
    /// </summary>
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    // Soft dependencies, for load order only (1.1.1). Neither is required: the vanilla duplicates are always
    // registered (without NoSmokeStayLit they work exactly like the originals), and the street light duplicates are
    // registered whenever QuanPieces is loaded, after it, so its pieces exist to be cloned.
    [BepInDependency(QuanPiecesGuid, BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency(NoSmokeStayLitGuid, BepInDependency.DependencyFlags.SoftDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
    public class QuanDupeLightsourcesPlugin : BaseUnityPlugin
    {
        public const string PluginGuid = "quandru.QuanDupeLightsources";
        public const string PluginName = "QuanDupeLightsources";
        public const string PluginVersion = PluginInfo.Version; // generated from <Version> in the .csproj

        private const string QuanPiecesGuid = "quandru.QuanPieces";
        private const string NoSmokeStayLitGuid = "TastyChickenLegs.NoSmokeStayLit";

        private const string PrefabPrefix = "Quan_";      // vanilla bases: Quan_<base>
        private const string PrefabSuffix = "_alwayslit";

        // Jotunn's MockManager.JVLMockPrefix (MockManager is internal).
        private const string JotunnMockPrefix = "JVLmock_";  // QuanPieces bases, already Quan_-prefixed: <base>_alwayslit
        private const string NameSuffix = " (Always Lit)";

        // Hammer tab for every duplicate. Jotunn creates the tab when a piece names a category it doesn't know.
        private const string HammerCategory = "QuanAlwaysLit";

        // The pieces NoSmokeStayLit can put on a day/night timer (its "<piece> on timer" settings).
        // Only these benefit from an always-lit duplicate: everything else NoSmokeStayLit handles is
        // either always lit or not, with no timer, so a duplicate would behave identically.
        // Hardcoded deliberately: a config-driven list could differ between server and clients,
        // which would leave pieces missing on some machines.
        private static readonly string[] BasePrefabs =
        {
            "piece_walltorch",          // Sconce (wall torch)
            "piece_groundtorch",        // Standing iron torch
            "piece_groundtorch_wood",   // Standing wood torch
            "piece_groundtorch_green",  // Standing green-burning iron torch
            "piece_groundtorch_blue",   // Standing blue-burning iron torch
            "piece_brazierfloor01",     // Standing brazier
            "piece_brazierceiling01",   // Hanging brazier
            "piece_jackoturnip",        // Jack-o-turnip
        };

        // QuanPieces' light sources. They're built on the vanilla Fireplace, so NoSmokeStayLit can time them.
        private static readonly string[] QuanPiecesPrefabs =
        {
            "Quan_streetlight_brazier", // Quan's Brazier Street Light
            "Quan_streetlight_lantern", // Quan's Dvergr Street Light
        };

        private void Awake()
        {
            PrefabManager.OnVanillaPrefabsAvailable += AddClones;
        }

        private void AddClones()
        {
            // Only ever register once per session.
            PrefabManager.OnVanillaPrefabsAvailable -= AddClones;

            var added = BasePrefabs.Select(b => AddClone(b, PrefabPrefix + b)).ToList();

            // The street light duplicates are clones of QuanPieces' pieces, so they need QuanPieces loaded.
            if (Chainloader.PluginInfos.ContainsKey(QuanPiecesGuid))
            {
                added.AddRange(QuanPiecesPrefabs.Select(b => AddClone(b, b + PrefabSuffix)));
            }
            else
            {
                Logger.LogInfo("QuanPieces isn't installed, so its street light duplicates (" +
                               string.Join(", ", QuanPiecesPrefabs.Select(b => b + PrefabSuffix)) + ") aren't " +
                               "added. The vanilla duplicates are unaffected. Install QuanPieces to get them.");
            }

            added.RemoveAll(name => name == null);
            Logger.LogInfo("Light source duplicates registered. Add to NoSmokeStayLit Custom Items Keep Lit: "
                           + string.Join(",", added));

            if (!Chainloader.PluginInfos.ContainsKey(NoSmokeStayLitGuid))
            {
                Logger.LogInfo("NoSmokeStayLit isn't installed. The duplicates are still added, but without it they " +
                               "work exactly like the originals (they burn fuel and have no timer or always-lit " +
                               "behaviour). Install NoSmokeStayLit and list them under Custom Items Keep Lit to " +
                               "keep them lit.");
            }
        }

        /// <summary>
        /// A piece added by another Jotunn mod (QuanPieces) still holds placeholder items and stations named
        /// "JVLmock_&lt;name&gt;" at this point; Jotunn swaps in the real ones later. Copying the placeholder name
        /// would leave an unresolvable recipe, so use the real name it stands for.
        /// </summary>
        private static string RealName(string name)
        {
            return name.StartsWith(JotunnMockPrefix, System.StringComparison.Ordinal)
                ? name.Substring(JotunnMockPrefix.Length)
                : name;
        }

        /// <summary>Registers an always-lit copy of <paramref name="baseName"/>; returns its name, or null if skipped.</summary>
        private string AddClone(string baseName, string cloneName)
        {
            var basePrefab = PrefabManager.Instance.GetPrefab(baseName);
            var basePiece = basePrefab ? basePrefab.GetComponent<Piece>() : null;
            if (basePiece == null)
            {
                Logger.LogWarning($"Base prefab '{baseName}' not found or has no Piece component; skipped.");
                return null;
            }

            var config = new PieceConfig
            {
                // Valheim localises embedded $tokens, so this renders as e.g.
                // "Standing wood torch (Always Lit)" in whatever language the client uses.
                Name = basePiece.m_name + NameSuffix,
                Description = basePiece.m_description,
                PieceTable = PieceTables.Hammer,
                Category = HammerCategory,
                CraftingStation = basePiece.m_craftingStation ? RealName(basePiece.m_craftingStation.name) : string.Empty,
                Requirements = basePiece.m_resources
                    .Where(r => r != null && r.m_resItem != null)
                    .Select(r => new RequirementConfig(RealName(r.m_resItem.name), r.m_amount, r.m_amountPerLevel, r.m_recover))
                    .ToArray()
            };

            // CustomPiece(name, baseName, config) deep-clones the base prefab,
            // so every component (Fireplace, Smelter, WearNTear etc.) comes across unchanged.
            PieceManager.Instance.AddPiece(new CustomPiece(cloneName, baseName, config));
            return cloneName;
        }
    }
}
