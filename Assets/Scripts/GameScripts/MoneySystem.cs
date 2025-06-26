using TMPro;
using UnityEngine;
using YG;


public class MoneySystem : MonoBehaviour
{

    [SerializeField] private GameObject dollars;
    [SerializeField] private TMP_Text moneyText;
    private int money, levelMoney;
    private bool bonusMoney;

    public static MoneySystem instance;

    private void Awake()
    {
        instance = this;
        EnemyZombie.OnZombieDie += ZombieDeath;
        money = YG2.saves.cash;
        moneyText.text = "$: " + money;
    }

    public void ZombieDeath(EnemyZombie z, float chance, int money)
    {
        int random = Random.Range(0, 100);
        if (random <= chance)
        {
            if (bonusMoney) money *= 2;
            Instantiate(dollars, z.transform.position, Quaternion.identity);
            levelMoney += money;
            moneyText.text = "$: " + this.money + levelMoney;
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

}
