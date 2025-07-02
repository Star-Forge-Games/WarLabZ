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
        if (waves > rec)
        {
            rec = waves;
            YG2.saves.idSave = waves;
            newRecordAnimator.SetActive(true);
        }
        record.text = $"{Loc("record")} {rec}";
        wavesLived.text = $"{Loc("lived")} {waves}";
        money.text = $"{MoneySystem.instance.GetCollectedMoney()}";
        zombiesKilled.text = $"{KillsCount.kills}";
        bossesKilled.text = $"{KillsCount.bosses}";
    }

    public void SwitchScene(int i)
    {
        fader.gameObject.SetActive(true);
        fader.Play("Fade");
        StartCoroutine(nameof(SwitchSceneCoroutine), i);
    }

    private IEnumerator SwitchSceneCoroutine(int i)
    {
        yield return new WaitForSeconds(1.2f);
        SceneManager.LoadScene(i);
    }



}
