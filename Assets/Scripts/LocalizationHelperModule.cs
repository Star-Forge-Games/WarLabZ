using UnityEngine;
using UnityEngine.Localization.Settings;

public static class LocalizationHelperModule
{

    public static string Loc(string key)
    {
        return LocalizationSettings.StringDatabase.GetLocalizedString(key);
    }

}
