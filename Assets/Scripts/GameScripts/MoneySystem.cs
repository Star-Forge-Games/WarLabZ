using System.Collections;
using TMPro;
using UnityEngine;
using YG;


public class MoneySystem : MonoBehaviour
{

    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private Animator dollarAnim;
    private int money, levelMoney;
    private bool bonusMoney;
    public static MoneySystem instance;
    private bool shaking;

    private void Awake()
    {
        instance = this;
        EnemyZombie.OnZombieDie += ZombieDeath;
        money = YG2.saves.cash;
        moneyText.text = "$: " + money;
    }

    public void ZombieDeath(EnemyZombie z, float chance, int money)
    {
        int random = UnityEngine.Random.Range(0, 100);
        if (random <= chance)
        {
            if (bonusMoney) money *= 2;
            levelMoney += money;
            moneyText.text = "$: " + this.money + " / " + levelMoney + " / " + (this.money + levelMoney);
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
        YG2.saves.cash = money + levelMoney * (bonusMoney? 2 : 1);
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
        yield return new WaitForSeconds(0.3f);
        shaking = false;
    }

}
