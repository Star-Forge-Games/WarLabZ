using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using YG;

public class MenuUI : MonoBehaviour
{

    [SerializeField] private Animator fader;
    [SerializeField] private TextMeshProUGUI cash;
    [SerializeField] Energy energy;

    private void Start()
    {
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
        energy.gameObject.SetActive(true);
        // user info
    }

    public void LoadGame()
    {
        energy.Spend();
        fader.gameObject.SetActive(true);
        fader.Play("Fade");
        StartCoroutine(SwitchScene());
    }

    private IEnumerator SwitchScene()
    {
        yield return new WaitForSeconds(1.2f);
        SceneManager.LoadScene("GameWorld");
    }

}
