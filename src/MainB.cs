#if !UMM
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System;

namespace EditorHistory
{
    [BepInPlugin(pluginGuid, pluginName, pluginVersion)]
    public class Main : BaseUnityPlugin
    {
        public const string pluginGuid = "space.scitor.Ostranauts.EditorHistory";
        public const string pluginName = "EditorHistory";
        public const string pluginVersion = "1.1.1";

        ConfigEntry<int> historyEntries = null!;

        private void Awake()
        {
            historyEntries = Config.Bind("General", "historyEntries", 10, "History entries (10 default)");
            historyEntries.SettingChanged += OnSettingChanged;

            Harmony harmony = new Harmony(pluginGuid);
            harmony.PatchAll(typeof(Patch).Assembly);

            Logger.LogInfo(pluginName + " v" + pluginVersion + " Ready");
        }

        private void OnSettingChanged(object sender, EventArgs e)
        {
            Patch.historyEntries = historyEntries.Value;
        }
    }
}
#endif
