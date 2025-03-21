using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class Wall : MonoBehaviour
{
    [SerializeField] private TMP_Text healthAmount;
    [SerializeField] private Image wallHpBar;
    [SerializeField] private WallSettings settings;
    [SerializeField] private float protectionTime;

    private bool invulnerable = false;

    private float protectedTime;

    public static Action OnWallDeath;

    private Action<bool> action;

    private void Awake()
    {
        action = (pause =>
        {
            if (!pause) SelfUnpause();
            else SelfPause();
        });
        PauseSystem.OnPauseStateChanged += action;
    }

    public void Protect()
    {
        invulnerable = true;
        StartCoroutine(StopProtection(protectionTime));
    }

    private IEnumerator StopProtection(float time)
    {
        yield return new WaitForSeconds(time);
        invulnerable = false;
        protectedTime = 0;
    }

    private int health;
    private int maxHealth;


    private void Start()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            if (i != YG2.saves.wallLevel) transform.GetChild(i).gameObject.SetActive(false);
            else transform.GetChild(i).gameObject.SetActive(true);
        }
        EnemyZombie.OnZombieHitWall += TakeDamage;
        maxHealth = settings.wallLevelsHp[YG2.saves.wallLevel];
        health = maxHealth;
        UpdateWallHp(maxHealth, health);
    }

    private void Update()
    {
        if (invulnerable)
        {
            protectedTime += Time.deltaTime;
        }
    }

    public void TakeDamage(int damage)
    {
        if (invulnerable) return;
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

    private void SelfPause()
    {
        if (!invulnerable) return;
        StopCoroutine(nameof(StopProtection));
    }

    private void SelfUnpause()
    {
        if (!invulnerable) return;
        StartCoroutine(StopProtection(protectionTime - protectedTime));
    }

}
