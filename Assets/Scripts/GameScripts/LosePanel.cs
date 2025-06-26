using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class LosePanel : MonoBehaviour
{

    [SerializeField] Animator fader;
    [SerializeField] TextMeshProUGUI record, wavesLived, money, zombiesKilled;
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
        record.text = $"Твой рекорд волн: {rec}";
        wavesLived.text = $"Прожито волн: {waves}";
        money.text = $"Денег заработано: {MoneySystem.instance.GetCollectedMoney()}";
        zombiesKilled.text = $"Зомби убито: {KillsCount.kills}";
    }

    public void SwitchScene(int i)
    {
        fader.Play("Fade");
        StartCoroutine(nameof(SwitchSceneCoroutine), i);
    }

    private IEnumerator SwitchSceneCoroutine(int i)
    {
        yield return new WaitForSeconds(1.2f);
        SceneManager.LoadScene(i);
    }



}
