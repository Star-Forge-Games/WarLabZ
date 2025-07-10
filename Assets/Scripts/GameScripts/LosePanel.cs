
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
    [SerializeField] GameObject recommendPanel;
    [SerializeField] TextMeshProUGUI recText;

    private void OnEnable()
    {
        if (YG2.saves.playedBefore == 0)
        {
            YG2.saves.playedBefore = 1;
        }
        int rec = YG2.saves.record;
        int waves = ess.GetTotalWave();
        record.text = $"{Loc("record")} {rec}";
        wavesLived.text = $"{Loc("lived")} {waves}";
        money.text = $"{MoneySystem.instance.GetCollectedMoney()}$";
        zombiesKilled.text = $"{KillsCount.kills}";
        bossesKilled.text = $"{KillsCount.bosses}";
        if (waves > rec)
        {
            rec = waves;
            YG2.saves.record = waves;
            newRecordAnimator.SetActive(true);
            YG2.SetLeaderboard("WarLabRecords", rec);
        }
        YG2.SaveProgress();
    }

    public void Recommend()
    {
        if (!YG2.reviewCanShow) return;
        if (!YG2.saves.reviewed) YG2.ReviewShow();
    }

    private void ProcessRecommendation(bool result)
    {
        if (!result) return;
        YG2.reviewCanShow = false;
        YG2.saves.reviewed = true;
        int m = MoneySystem.instance.GetCollectedMoney();
        int rm = Mathf.CeilToInt(Mathf.Clamp(m / 2f, 100, m));
        rm -= rm % 50;
        YG2.saves.cash += rm;
        YG2.SaveProgress();
        recText.text = $"{Loc("review")} <color=green>{rm}$</color>.";
        recommendPanel.SetActive(true);

    }

    public void DestroyReview()
    {
        recommendPanel.SetActive(false);
    }

    public void SwitchScene(int i)
    {
        AudioListener.volume = 0;
        if (YG2.isTimerAdvCompleted) YG2.InterstitialAdvShow();
        else SwitchToMenu();
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
        YG2.onCloseInterAdv += SwitchToMenu;
        YG2.onReviewSent += ProcessRecommendation;
    }

    private void OnDestroy()
    {
        YG2.onCloseInterAdv -= SwitchToMenu;
        YG2.onReviewSent -= ProcessRecommendation;
    }



}
