using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace EditorHistory;

[HarmonyPatch]
class Patch
{
    private const string Blank = "Blank";

    static List<string> HistoryEntryList = new List<string>();
    static string LastFilterString = Blank;

    [HarmonyPrefix]
    [HarmonyPatch(typeof(CrewSim), nameof(CrewSim.PopulatePartList))]
    static bool CrewSim_PopulatePartList__Prefix(string strCT, CrewSim __instance, ref Transform ___pnlPartsContent)
    {
        if (string.IsNullOrEmpty(strCT)) {
            strCT = Blank;
        }
        LastFilterString = strCT;
        if (!Blank.Equals(strCT)) {
            return true;
        }
        foreach (Component component in ___pnlPartsContent.transform) {
            Object.Destroy(component.gameObject);
        }
        foreach (string histEntry in HistoryEntryList) {
            CondOwner condOwner = DataHandler.GetCondOwner(histEntry);
            __instance.GetType()
                .GetMethod("PartListBtn", BindingFlags.NonPublic | BindingFlags.Instance)?
                .Invoke(__instance, new object[] { condOwner, ___pnlPartsContent });

            condOwner.Destroy();
        }
        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(CrewSim), "SetPartCursor")]
    static void CrewSim_SetPartCursor__Prefix(string strName, CrewSim __instance)
    {
        if (strName == null)
            return;

        HistoryEntryList.Insert(0, strName);
        HistoryEntryList = HistoryEntryList.Distinct().Take(Main.Settings.historyEntries).ToList();
        if (Blank.Equals(LastFilterString)) {
            __instance.PopulatePartList(Blank);
        }
    }
}