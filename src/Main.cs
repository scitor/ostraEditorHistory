using HarmonyLib;
using System.Reflection;
using UnityModManagerNet;

namespace EditorHistory;

public static class Main
{
    public static Settings Settings { get; private set; } = new Settings();

    // Unity Mod Manage Wiki: https://wiki.nexusmods.com/index.php/Category:Unity_Mod_Manager
    private static bool Load(UnityModManager.ModEntry modEntry)
    {
        Settings = UnityModManager.ModSettings.Load<Settings>(modEntry);
        modEntry.OnGUI = OnDrawGUI;
        modEntry.OnSaveGUI = OnSaveGUI;
        modEntry.OnToggle = OnToggle;
        return true;
    }

    static void OnDrawGUI(UnityModManager.ModEntry entry)
    {
        Settings.Draw(entry);
    }

    static void OnSaveGUI(UnityModManager.ModEntry entry)
    {
        Settings.Save(entry);
    }

    private static bool OnToggle(UnityModManager.ModEntry modEntry, bool active)
    {
        Harmony harmony = new Harmony(modEntry.Info.Id);
        if (active) {
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        } else {
            harmony.UnpatchAll(modEntry.Info.Id);
        }
        return true;
    }
}
