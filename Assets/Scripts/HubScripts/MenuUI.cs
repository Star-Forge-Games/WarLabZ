using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

public class MenuUI : MonoBehaviour
{

    [SerializeField] private Animator fader;
    [SerializeField] private TextMeshProUGUI cash;
    [SerializeField] Energy energy;
    [SerializeField] TextMeshProUGUI playerName;
    [SerializeField] Image playerIcon;
    [SerializeField] GameObject playerPanel;
    public ImageLoadYG photoImageLoad;

    private void Start()
    {
        YG2.onCloseInterAdv += SwitchToGame;
        YG2.onErrorInterAdv += SwitchToGame;
        fader.gameObject.SetActive(true);
        cash.text = $"{YG2.saves.cash} $";
        AudioListener.volume = YG2.saves.soundOn? 1 : 0;
        LocalizationSettings.InitializationOperation.WaitForCompletion();
        if (YG2.saves.localeId == -1)
        {
            YG2.saves.localeId = LocalizationSettings.SelectedLocale.SortOrder;
            YG2.SaveProgress();
        } else
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[YG2.saves.localeId];
        }
        playerName.text = YG2.player.name;
        playerPanel.SetActive(true);
        energy.gameObject.SetActive(true);
        if (photoImageLoad != null && YG2.player.auth)
        {
            photoImageLoad.Load(YG2.player.photo);
        }
    }

    private void OnEnable()
    {
        if (YG2.isSDKEnabled) cash.text = $"{YG2.saves.cash} $";
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
        YG2.InterstitialAdvShow();
    }

    private IEnumerator SwitchScene()
    {
        yield return new WaitForSeconds(1.2f);
        SceneManager.LoadScene("GameWorld");
    }

    private void OnDestroy()
    {
        YG2.onErrorInterAdv -= SwitchToGame;
        YG2.onCloseInterAdv -= SwitchToGame;
    }

}
