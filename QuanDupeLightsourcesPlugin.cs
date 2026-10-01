using System.Linq;
using BepInEx;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using Jotunn.Utils;

namespace QuanDupeLightsources
{
    /// <summary>
    /// Registers identical, differently-named copies of NoSmokeStayLit's default fire prefabs.
    /// No patches, no runtime logic: lit/fuel/timer state is left entirely to NoSmokeStayLit,
    /// which picks the clones up via its "Custom Items Keep Lit" list.
    /// </summary>
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
    public class QuanDupeLightsourcesPlugin : BaseUnityPlugin
    {
        public const string PluginGuid = "Quandru.QuanDupeLightsources";
        public const string PluginName = "QuanDupeLightsources";
        public const string PluginVersion = PluginInfo.Version; // generated from <Version> in the .csproj

        private const string PrefabPrefix = "Quan_";
        private const string NameSuffix = " (Always Lit)";

        // NoSmokeStayLit's default supported objects (fe_<prefab> keys, Thunderstore table).
        // Hardcoded deliberately: a config-driven list could differ between server and clients,
        // which would leave pieces missing on some machines.
        private static readonly string[] BasePrefabs =
        {
            "fire_pit",                 // Campfire
            "bonfire",                  // Bonfire
            "hearth",                   // Hearth
            "piece_walltorch",          // Sconce
            "piece_groundtorch",        // Standing iron torch
            "piece_groundtorch_wood",   // Standing wood torch
            "piece_groundtorch_green",  // Standing green-burning iron torch
            "piece_groundtorch_blue",   // Standing blue-burning iron torch
            "piece_brazierfloor01",     // Standing brazier
            "piece_brazierceiling01",   // Hanging brazier
            "piece_jackoturnip",        // Jack-o-turnip
            "piece_oven",               // Stone oven
            "piece_bathtub",            // Hot tub
            "smelter",                  // Smelter
            "blastfurnace",             // Blast furnace
            "eitrrefinery",             // Eitr refinery
        };

        private void Awake()
        {
            PrefabManager.OnVanillaPrefabsAvailable += AddClones;
        }

        private void AddClones()
        {
            // Only ever register once per session.
            PrefabManager.OnVanillaPrefabsAvailable -= AddClones;

            foreach (var baseName in BasePrefabs)
            {
                var basePrefab = PrefabManager.Instance.GetPrefab(baseName);
                var basePiece = basePrefab ? basePrefab.GetComponent<Piece>() : null;
                if (basePiece == null)
                {
                    Logger.LogWarning($"Base prefab '{baseName}' not found or has no Piece component; skipped.");
                    continue;
                }

                var config = new PieceConfig
                {
                    // Valheim localises embedded $tokens, so this renders as e.g.
                    // "Standing wood torch (Always Lit)" in whatever language the client uses.
                    Name = basePiece.m_name + NameSuffix,
                    Description = basePiece.m_description,
                    PieceTable = PieceTables.Hammer,
                    Category = basePiece.m_category.ToString(),
                    CraftingStation = basePiece.m_craftingStation ? basePiece.m_craftingStation.name : string.Empty,
                    Requirements = basePiece.m_resources
                        .Where(r => r != null && r.m_resItem != null)
                        .Select(r => new RequirementConfig(r.m_resItem.name, r.m_amount, r.m_amountPerLevel, r.m_recover))
                        .ToArray()
                };

                // CustomPiece(name, baseName, config) deep-clones the vanilla prefab,
                // so every component (Fireplace, Smelter, WearNTear etc.) comes across unchanged.
                PieceManager.Instance.AddPiece(new CustomPiece(PrefabPrefix + baseName, baseName, config));
            }

            Logger.LogInfo("Light source duplicates registered. Add to NoSmokeStayLit Custom Items Keep Lit: "
                           + string.Join(",", BasePrefabs.Select(b => PrefabPrefix + b)));
        }
    }
}
