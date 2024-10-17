using UnityModManagerNet;

namespace EditorHistory;

public class Settings : UnityModManager.ModSettings, IDrawable
{
    [Draw("History entries (10 default)", Min = 0)]
    public int historyEntries = 10;

    public override void Save(UnityModManager.ModEntry modEntry)
    {
        Save(this, modEntry);
    }

    public void OnChange()
    {
    }
}
