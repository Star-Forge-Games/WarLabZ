using UnityEditor;
using YG;

public class WarlabZYG
{
    [MenuItem("WarLabZ/Reset Progress")]
    public static void ResetProgress()
    {
        YG2.saves.playedBefore = -1;
        YG2.saves.cash = 0;
        YG2.saves.supplies = new int[] { 1, 1, 1 };
        YG2.saves.wallLevel = 0;
        YG2.saves.weaponLevels = new int[] { 0, -1, -1, -1, -1 };
        YG2.saves.selectedWeapon = 0;
        YG2.saves.soundOn = true;
        YG2.saves.localeId = -1;
        YG2.saves.energyLeft = 0;
        YG2.saves.lastEnergySpentTimeStamp = 0;
        YG2.saves.record = 0;
        YG2.saves.idSave = 0;
        YG2.SaveProgress();
    }

    [MenuItem("WarLabZ/Add 100 cash")]
    public static void Add100Cash()
    {
        YG2.saves.cash = YG2.saves.cash + 100;
        YG2.SaveProgress();
    }
}
