using UnityEditor;
using YG;

public class WarlabZYG
{
    [MenuItem("WarLabZ/Reset Progress")]
    public static void ResetProgress()
    {
        //YG2.saves.levelStars = new int[] { -1, -1, -1, -1 };
        //YG2.saves.points = 0;
        //YG2.saves.level = 0;
        //YG2.saves.achievements = new int[] { 0 };
        //YG2.saves.keys = 0;
        //YG2.saves.donateCash = 0;
        YG2.saves.playedBefore = false;
        YG2.saves.cash = 0;
        YG2.saves.unlockedWeapons = new int[] { 0 };
        YG2.saves.supplies = new int[] { 1, 2, 0 };
        YG2.saves.wallLevel = 0;
        YG2.saves.weaponLevels = new int[] { 0, 0, 0, 0, 0 };
        YG2.saves.selectedWeapon = 0;
        YG2.saves.soundOn = true;
        YG2.saves.localeId = -1;
        YG2.saves.energyLeft = 0;
        YG2.saves.lastEnergySpentTimeStamp = 0;
    YG2.SaveProgress();
    }

    [MenuItem("WarLabZ/Add 100 cash")]
    public static void Add100Cash()
    {
        YG2.saves.cash = YG2.saves.cash + 100;
        YG2.SaveProgress();
    }
}
