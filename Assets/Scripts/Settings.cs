using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using YG;

public class Settings : MonoBehaviour
{
    [SerializeField] private Button sound, locale;
    [SerializeField] private Sprite[] localeSprites;
    void OnEnable()
    {
        if (AudioListener.volume == 0)
        {
            var c = sound.colors;
            c.normalColor = Color.red;
            sound.colors = c;
        } else
        {
            var c = sound.colors;
            c.normalColor = Color.white;
            sound.colors = c;
        }
        locale.image.sprite = localeSprites[YG2.saves.localeId];
    }

    public void TurnSound()
    {
        var c = sound.colors;
        if (AudioListener.volume == 0)
        {
            c.normalColor = Color.white;
            AudioListener.volume = 1;
            YG2.saves.soundOn = true;
        } else
        {
            c.normalColor = Color.red;
            AudioListener.volume = 0;
            YG2.saves.soundOn = false;
        }
        sound.colors = c;
        YG2.SaveProgress();
    }

    public void NextLocale()
    {
        int nextIndex = YG2.saves.localeId + 1;
        if (nextIndex >= LocalizationSettings.AvailableLocales.Locales.Count)
        {
            nextIndex = 0;
        }
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[nextIndex];
        YG2.saves.localeId = nextIndex;
        YG2.SaveProgress();
        locale.image.sprite = localeSprites[nextIndex];
    }
}
