using CoolMod.Customs;
using Kitchen;
using KitchenData;
using KitchenLib;
using KitchenLib.Event;
using KitchenLib.References;
using KitchenMods;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

// Namespace should have "Kitchen" in the beginning
namespace MethMod
{
    public class Mod : BaseMod, IModSystem
    {
        public const string MOD_GUID = "com.icecreamsandwch.breakingkitchen";
        public const string MOD_NAME = "Cook Meth";
        public const string MOD_VERSION = "0.1.0";
        public const string MOD_AUTHOR = "IceCreamSandwch";
        public const string MOD_GAMEVERSION = ">=1.1.7";

        // Boolean constant whose value depends on whether you built with DEBUG or RELEASE mode, useful for testing
#if DEBUG
        public const bool DEBUG_MODE = true;
#else
        public const bool DEBUG_MODE = false;
#endif

        public static AssetBundle Bundle;

        //Game Data Objects already in the game
        internal static Item Cheese => GetExistingGDO<Item>(ItemReference.Cheese);
        //GDO from the helpful "IngredientLib"
        public static Item Garlic => Find<Item>(IngredientLib.References.GetIngredient("garlic"));

        //processes, like how the character interacts
        internal static Process Cook => GetExistingGDO<Process>(ProcessReferences.Cook);
        internal static Process Chop => GetExistingGDO<Process>(ProcessReferences.Chop);
        internal static Process Knead => GetExistingGDO<Process>(ProcessReferences.Knead);

        //item, can be combined and carried around
        internal static Item CookedMeth => GetModdedGDO<Item, CookedMeth>();
        //dish
        internal static Dish MethDish => GetModdedGDO<Dish, MethDish>();

        public Main() : base(MOD_ID, MOD_NAME, MOD_AUTHOR, MOD_VERSION, $"{MOD_GAMEVERSION}", Assembly.GetExecutingAssembly())
        {
            string bundlePath = Path.Combine(new string[] { Directory.GetParent(Application.dataPath).FullName, "Mods", ModID });

            Debug.Log($"{MOD_NAME} {MOD_VERSION} {MOD_AUTHOR}: Loaded");
            Debug.Log($"Assets Loaded From {bundlePath}");
        }

        protected override void OnInitialise()
        {
            LogWarning($"{MOD_GUID} v{MOD_VERSION} in use!");
        }

        private void AddGameData()
        {
            LogInfo("Attempting to register game data...");

            //add the GDOs you need 
            AddGameDataObject<CookedMeth>();
            AddGameDataObject<MethDish>();

            LogInfo("Done loading game data.");
        }

        protected override void OnUpdate()
        {

        }

        protected override void OnPostActivate(KitchenMods.Mod mod)
        {
            // TODO: Uncomment the following if you have an asset bundle.
            // TODO: Also, make sure to set EnableAssetBundleDeploy to 'true' in your ModName.csproj
            LogInfo("Attempting to load asset bundle...");
            bundle = mod.GetPacks<AssetBundleModPack>().SelectMany(e => e.AssetBundles).First();
            LogInfo("Done loading asset bundle.");

            // Register custom GDOs
            AddGameData();
        }
        #region Logging
        public static void LogInfo(string _log) { Debug.Log($"[{MOD_NAME}] " + _log); }
        public static void LogWarning(string _log) { Debug.LogWarning($"[{MOD_NAME}] " + _log); }
        public static void LogError(string _log) { Debug.LogError($"[{MOD_NAME}] " + _log); }
        public static void LogInfo(object _log) { LogInfo(_log.ToString()); }
        public static void LogWarning(object _log) { LogWarning(_log.ToString()); }
        public static void LogError(object _log) { LogError(_log.ToString()); }
        #endregion
    }
}
