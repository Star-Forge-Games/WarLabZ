using UnityEngine.Localization.Settings;

public static class LocalizationHelperModule
{

    public static string Loc(string key)
    {
        return LocalizationSettings.StringDatabase.GetLocalizedString(key);
    }

    public static string MoneyFormat(int money)
    {
        string mf;
        if (money < 1000) mf = $"{money}$";
        else if (money < 1000000)
        {
            mf = $"{((double)money / 1000).ToString("F1")}K$";
        }
        else if (money < 1000000000)
        {
            mf = $"{((double)money / 1000000).ToString("F1")}M$";
        }
        else
        {
            mf = $"{((double)money / 1000000000).ToString("F1")}B$";
        }
        return mf;
    }

}
