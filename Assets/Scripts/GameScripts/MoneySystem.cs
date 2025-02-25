using UnityEngine;

public class MoneySystem : MonoBehaviour
{

    [SerializeField] private GameObject dollars;

    private void Awake()
    {
        EnemyZombie.OnZombieDie += ZombieDeath;
    }

    public void ZombieDeath(EnemyZombie z, float chance)
    {
        int random = Random.Range(0, 100);
        if (random <= chance)
        {
            Instantiate(dollars, z.transform.position, Quaternion.identity);
            PlayerController.money += 1;
        }
    }

    private void OnDestroy()
    {
        EnemyZombie.OnZombieDie -= ZombieDeath;
    }

}
