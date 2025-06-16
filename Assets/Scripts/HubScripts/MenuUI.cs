using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using YG;

public class MenuUI : MonoBehaviour
{

    [SerializeField] private Animator fader;
    [SerializeField] private TextMeshProUGUI cash, donateCash, keys;

    private void Start()
    {
        cash.text = $"$ {YG2.saves.cash}";
        donateCash.text = $"(G) {YG2.saves.donateCash}";
        keys.text = $"()--E {YG2.saves.keys}";
        AudioListener.volume = YG2.saves.soundOn? 1 : 0;
        if (YG2.saves.localeId == -1)
        {
            YG2.saves.localeId = LocalizationSettings.SelectedLocale.SortOrder;
            YG2.SaveProgress();
        } else
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[YG2.saves.localeId];
        }
    }

    public void LoadShopScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(3);
    }

    public void StartGame()
    {
        fader.Play("Fade");
        StartCoroutine(nameof(Game));
    }
    private IEnumerator Game()
    {
        yield return new WaitForSeconds(1.2f);
        SceneManager.LoadScene(1);
    }

    public void LoadMap()
    {
        SceneManager.LoadScene(2);
    }

}
