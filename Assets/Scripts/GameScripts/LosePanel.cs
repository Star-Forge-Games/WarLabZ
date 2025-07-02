using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;
using static LocalizationHelperModule;

public class LosePanel : MonoBehaviour
{

    [SerializeField] Animator fader;
    [SerializeField] TextMeshProUGUI record, wavesLived, money, zombiesKilled, bossesKilled;
    [SerializeField] EnemySpawnSystem ess;
    [SerializeField] GameObject newRecordAnimator;

    private void OnEnable()
    {
        int rec = YG2.saves.record;
        int waves = ess.GetTotalWave();
        record.text = $"{Loc("record")} {rec}";
        if (waves > rec)
        {
            rec = waves;
            YG2.saves.record = waves;
            YG2.SaveProgress();
            newRecordAnimator.SetActive(true);
        }
        wavesLived.text = $"{Loc("lived")} {waves}";
        money.text = $"{MoneySystem.instance.GetCollectedMoney()}$";
        zombiesKilled.text = $"{KillsCount.kills}";
        bossesKilled.text = $"{KillsCount.bosses}";
    }

    public void SwitchScene(int i)
    {
        AudioListener.volume = 0;
        YG2.InterstitialAdvShow();
    }

    private void SwitchToMenu()
    {
        AudioListener.volume = YG2.saves.soundOn ? 1 : 0;
        fader.gameObject.SetActive(true);
        fader.Play("Fade");
        StartCoroutine(nameof(SwitchSceneCoroutine));
    }

    private IEnumerator SwitchSceneCoroutine()
    {
        yield return new WaitForSeconds(1.2f);
        SceneManager.LoadScene(0);
    }

    private void Start()
    {
        YG2.onErrorInterAdv += SwitchToMenu;
        YG2.onCloseInterAdv += SwitchToMenu;
    }

    private void OnDestroy()
    {
        YG2.onErrorInterAdv -= SwitchToMenu;
        YG2.onCloseInterAdv -= SwitchToMenu;
    }



}
