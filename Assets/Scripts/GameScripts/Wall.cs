using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Wall : MonoBehaviour
{

    [SerializeField] public int maxHealth;
    [SerializeField] private TMP_Text healthAmount;
    private int health;
    public Image wallHpBar;


    private void Start()
    {
        EnemyZombie.OnZombieHitWall += TakeDamage;
        health = maxHealth;
        UpdateWallHp(maxHealth, health);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            EnemyZombie.OnZombieHitWall -= TakeDamage;
            Destroy(gameObject);
            // death
        }
        UpdateWallHp(maxHealth, health);
    }
    public void UpdateWallHp(int maxHealth, int currentHealth)
    {
        wallHpBar.fillAmount = (float)currentHealth / maxHealth;
        healthAmount.text = $"{currentHealth}";
    }

    private void OnDestroy()
    {
        EnemyZombie.OnZombieHitWall -= TakeDamage;
    }

}
