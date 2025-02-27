using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class Wall : MonoBehaviour
{
    [SerializeField] private TMP_Text healthAmount;
    [SerializeField] private Image wallHpBar;
    [SerializeField] private WallSettings settings;

    public static Action OnWallDeath;

    private int health;
    private int maxHealth;


    private void Start()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            if (i != YG2.saves.wallLevel) transform.GetChild(i).gameObject.SetActive(false);
        }
        EnemyZombie.OnZombieHitWall += TakeDamage;
        maxHealth = settings.wallLevelsHp[YG2.saves.wallLevel];
        health = maxHealth;
        UpdateWallHp(maxHealth, health);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            EnemyZombie.OnZombieHitWall -= TakeDamage;
            OnWallDeath?.Invoke();
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
