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

    private void Start()
    {
        YG2.onCorrectLang += ExternalLocalize;
    }

    private void ExternalLocalize(string lang)
    {
        if (lang == "ru")
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
            locale.image.sprite = localeSprites[1];
        }
        else if (lang != "en")
        {
            YG2.SwitchLanguage("en");
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
            locale.image.sprite = localeSprites[0];
        } else
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
            locale.image.sprite = localeSprites[0];
        }
    }
    void OnEnable()
    {
        sound.image.sprite = soundSprites[(int)AudioListener.volume];
        locale.image.sprite = localeSprites[YG2.lang == "ru"? 1 : 0];
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
        if (YG2.lang == "ru")
        {
            YG2.SwitchLanguage("en");
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
            locale.image.sprite = localeSprites[0];
        } else
        {
            YG2.SwitchLanguage("ru");
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
            locale.image.sprite = localeSprites[1];
        }
    }

    private void OnDestroy()
    {
        YG2.onCorrectLang -= ExternalLocalize;
    }
}
