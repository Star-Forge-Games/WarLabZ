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

    public void ZombieDeath(EnemyZombie z, float chance)
    {
        int random = Random.Range(0, 100);
        if (random <= chance)
        {
            Instantiate(dollars, z.transform.position, Quaternion.identity);
            money += 1;
            moneyText.text = "$: " + money;
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

}
