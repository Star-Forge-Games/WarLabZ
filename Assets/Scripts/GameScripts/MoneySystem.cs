using TMPro;
using UnityEngine;
using YG;


public class MoneySystem : MonoBehaviour
{

    [SerializeField] private GameObject dollars;
    [SerializeField] private TMP_Text moneyText;

    private static int money;

    private void Awake()
    {
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

    public static void SaveMoney()
    {
        YG2.saves.cash = money;
        YG2.SaveProgress();
    }

    private void OnDestroy()
    {
        EnemyZombie.OnZombieDie -= ZombieDeath;
    }

}
