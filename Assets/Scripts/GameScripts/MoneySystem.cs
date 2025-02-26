using TMPro;
using UnityEngine;


public class MoneySystem : MonoBehaviour
{

    [SerializeField] private GameObject dollars;
    [SerializeField] private TMP_Text moneyText;

    private static int money;

    private void Awake()
    {
        EnemyZombie.OnZombieDie += ZombieDeath;
        money = PlayerPrefs.GetInt("money");
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
        PlayerPrefs.SetInt("money", money);
        PlayerPrefs.Save();
    }

    private void OnDestroy()
    {
        EnemyZombie.OnZombieDie -= ZombieDeath;
    }

}
