using System.Collections;
using TMPro;
using UnityEngine;
using YG;


public class MoneySystem : MonoBehaviour
{

    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private Animator dollarAnim;
    private int levelMoney;
    private bool bonusMoney;
    public static MoneySystem instance;
    private bool shaking;

    private void Awake()
    {
        instance = this;
        EnemyZombie.OnZombieDie += ZombieDeath;
        moneyText.text = "0";
    }

    public void ZombieDeath(EnemyZombie z, float chance, int money)
    {
        int random = UnityEngine.Random.Range(0, 100);
        if (random <= chance)
        {
            if (bonusMoney) money *= 2;
            levelMoney += money;
            string mf = "";
            if (levelMoney < 1000) mf = $"{levelMoney}";
            else if (levelMoney < 1000000)
            {
                mf = $"{((double)levelMoney / 1000).ToString("F1")}K";
            }
            else if (levelMoney < 1000000000)
            {
                mf = $"{((double)levelMoney / 1000000).ToString("F1")}M";
            }
            else
            {
                mf = $"{((double)levelMoney / 1000000000).ToString("F1")}B";
            }
            moneyText.text = mf;
            if (!shaking)
            {
                shaking = true;
                dollarAnim.Play("MoneyAdd");
                StartCoroutine(nameof(StopShake));
            }
        }
    }

    public void SaveMoney()
    {
        YG2.saves.cash += levelMoney * (bonusMoney? 2 : 1);
        YG2.SaveProgress();
    }

    private void OnDestroy()
    {
        EnemyZombie.OnZombieDie -= ZombieDeath;
    }

    public void SetBonus()
    {
        bonusMoney = true;
    }

    public int GetCollectedMoney()
    {
        return levelMoney * (bonusMoney ? 2 : 1);
    }

    private IEnumerator StopShake()
    {
        yield return new WaitForSeconds(0.1f);
        shaking = false;
    }

}
