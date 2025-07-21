using System.Collections;
using TMPro;
using UnityEngine;
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
        //YG2.SetDefaultSaves();
        ((UniversalRenderPipelineAsset)GraphicsSettings.currentRenderPipeline).renderScale = 1;
        fader.gameObject.SetActive(true);
        YG2.onCloseInterAdv += SwitchToGame;
        cash.text = MoneyFormat(YG2.saves.cash);
        AudioListener.volume = YG2.saves.soundOn ? 1 : 0;
        playerName.text = YG2.player.name;
        playerPanel.SetActive(true);
        energy.gameObject.SetActive(true);
        if (photoImageLoad != null && YG2.player.auth)
        {
            photoImageLoad.Load(YG2.player.photo);
        }
        Localize();
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
        if (!YG2.saves.hasLabel)
        {
            YG2.onGameLabelFail += LabelNo;
            YG2.onGameLabelSuccess += LabelYes;
            Debug.Log("Time to show label");
            if (YG2.gameLabelCanShow)
            {
                Debug.Log("Showing Label");
                AudioListener.volume = 0;
                YG2.gameLabelCanShow = false;
                YG2.GameLabelShowDialog();
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
        }
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
        if (YG2.saves.localeId == -1)
        {
            YG2.saves.localeId = settings.GetSelectedLocale().SortOrder;
            YG2.SaveProgress();
        }
        else
        {
            settings.SetSelectedLocale(settings.GetAvailableLocales().Locales[YG2.saves.localeId]);
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
    }

}
