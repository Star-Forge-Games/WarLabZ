using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;
using static LocalizationHelperModule;

public class MenuUI : MonoBehaviour
{

    [SerializeField] private Animator fader;
    [SerializeField] private TextMeshProUGUI cash;
    [SerializeField] Energy energy;
    [SerializeField] TextMeshProUGUI playerName;
    [SerializeField] Image playerIcon;
    [SerializeField] GameObject playerPanel, playButton;
    [SerializeField] Transform handWeapons;
    [SerializeField] GameObject tutorial;
    [SerializeField] Button playerDonateButton;
    public ImageLoadYG photoImageLoad;
    [SerializeField] Light lightSource;

    private void Start()
    {
        YG2.onCorrectLang += ExternalLocalize;
        YG2.ConsumePurchases();
        //YG2.SetDefaultSaves();
        fader.gameObject.SetActive(true);
        YG2.onCloseInterAdv += SwitchToGame;
        cash.text = MoneyFormat(YG2.saves.cash);
        AudioListener.volume = YG2.saves.soundOn ? 1 : 0;
        playerName.text = (YG2.player.auth? YG2.player.name : Loc("unauthorized"));
        playerPanel.SetActive(true);
        energy.gameObject.SetActive(true);
        if (photoImageLoad != null && YG2.player.auth)
        {
            photoImageLoad.Load(YG2.player.photo);
        }
        int id = YG2.saves.selectedWeapon;
        for (int i = 0; i < handWeapons.childCount; i++)
        {
            if (i != id)
            {
                handWeapons.transform.GetChild(i).gameObject.SetActive(false);
            }
            else
            {
                handWeapons.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
        if (YG2.saves.shadows)
        {
            lightSource.shadows = LightShadows.Hard;
        }
        else
        {
            lightSource.shadows = LightShadows.None;
        }
        cash.text = MoneyFormat(YG2.saves.cash);
        playerDonateButton.interactable = YG2.saves.playedBefore == 1;
        Localize();
        if (YG2.saves.playedBefore == -1)
        {
            YG2.gameLabelCanShow = true;
            tutorial.SetActive(true);
            playButton.SetActive(false);
        }
        else if (YG2.saves.playedBefore == 0)
        {
            SceneManager.LoadScene("GameWorld");
            playButton.SetActive(true);
        } else
        {
            playButton.SetActive(true);
            if (!YG2.saves.hasLabel)
            {
                YG2.onGameLabelFail += LabelNo;
                YG2.onGameLabelSuccess += LabelYes;
                if (YG2.gameLabelCanShow)
                {
                    AudioListener.volume = 0;
                    YG2.gameLabelCanShow = false;
                    YG2.GameLabelShowDialog();
                }
            }
        }
        ((UniversalRenderPipelineAsset)GraphicsSettings.currentRenderPipeline).renderScale = 1;
    }

    private void LabelNo()
    {
        AudioListener.volume = YG2.saves.soundOn ? 1 : 0;
    }

    private void LabelYes()
    {
        AudioListener.volume = YG2.saves.soundOn ? 1 : 0;
        YG2.saves.hasLabel = true;
        YG2.SaveProgress();
    }

    private void Localize()
    {
        LocalizationSettings settings = LocalizationSettings.InitializationOperation.WaitForCompletion();
        if (YG2.lang == "ru")
        {
            settings.SetSelectedLocale(settings.GetAvailableLocales().Locales[1]);
        }
        else
        {
            settings.SetSelectedLocale(settings.GetAvailableLocales().Locales[0]);
        }
    }

    private void ExternalLocalize(string lang)
    {
        if (lang == "ru")
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
        } else if (lang != "en")
        {
            YG2.SwitchLanguage("en");
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
        } else
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
        }
    }

    private void OnEnable()
    {
        if (YG2.isSDKEnabled)
        {
            if (YG2.saves.shadows)
            {
                lightSource.shadows = LightShadows.Hard;
            }
            else
            {
                lightSource.shadows = LightShadows.None;
            }
            cash.text = MoneyFormat(YG2.saves.cash);
            //YG2.SetDefaultSaves();
            playerDonateButton.interactable = YG2.saves.playedBefore == 1;
            Localize();
            if (YG2.saves.playedBefore == -1)
            {
                tutorial.SetActive(true);
            }
            else if (YG2.saves.playedBefore == 0)
            {
                SceneManager.LoadScene("GameWorld");
            } else
            {
                playButton.SetActive(true);
            }
        }
    }

    public void SwitchToGame()
    {
        AudioListener.volume = YG2.saves.soundOn ? 1 : 0;
        energy.Spend();
        fader.gameObject.SetActive(true);
        fader.Play("Fade");
        StartCoroutine("SwitchScene");
    }

    public void LoadGame()
    {
        AudioListener.volume = 0;
        if (YG2.isTimerAdvCompleted) YG2.InterstitialAdvShow();
        else SwitchToGame();
    }

    private IEnumerator SwitchScene()
    {
        yield return new WaitForSeconds(1.2f);
        SceneManager.LoadScene("GameWorld");
    }

    private void OnDestroy()
    {
        YG2.onCloseInterAdv -= SwitchToGame;
        YG2.onGameLabelFail -= LabelNo;
        YG2.onGameLabelSuccess -= LabelYes;
        YG2.onCorrectLang -= ExternalLocalize;
    }

}
