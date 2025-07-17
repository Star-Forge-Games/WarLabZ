using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using YG;
using static LocalizationHelperModule;

public class Settings : MonoBehaviour
{
    [SerializeField] private Button sound, locale;
    [SerializeField] private Sprite[] localeSprites, soundSprites;
    [SerializeField] private TextMeshProUGUI graphicsText;
    [SerializeField] private Light lightSource;
    void OnEnable()
    {
        sound.image.sprite = soundSprites[(int)AudioListener.volume];
        if (YG2.saves.localeId == -1)
        {
            YG2.saves.localeId = LocalizationSettings.SelectedLocale.SortOrder;
            YG2.SaveProgress();
        }
        locale.image.sprite = localeSprites[YG2.saves.localeId];
        if (YG2.saves.shadows)
        {
            graphicsText.text = Loc("high");
        } else
        {
            graphicsText.text = Loc("low");
        }
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

    public void TurnGraphics()
    {
        YG2.saves.shadows = !YG2.saves.shadows;
        YG2.SaveProgress();
        if (YG2.saves.shadows)
        {
            graphicsText.text = Loc("high");
            lightSource.shadows = LightShadows.Hard;
        } else
        {
            graphicsText.text = Loc("low");
            lightSource.shadows = LightShadows.None;
        }
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
}//
