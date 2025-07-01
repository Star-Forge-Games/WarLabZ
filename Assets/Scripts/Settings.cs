using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using YG;

public class Settings : MonoBehaviour
{
    [SerializeField] private Button sound, locale;
    [SerializeField] private Sprite[] localeSprites, soundSprites;
    void OnEnable()
    {
        sound.image.sprite = soundSprites[(int)AudioListener.volume];
        locale.image.sprite = localeSprites[YG2.saves.localeId];
    }

    public void TurnSound()
    {
        if (AudioListener.volume == 0)
        {
            AudioListener.volume = 1;
            YG2.saves.soundOn = true;
        } else
        {
            AudioListener.volume = 0;
            YG2.saves.soundOn = false;
        }
        sound.image.sprite = soundSprites[(int)AudioListener.volume];
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
