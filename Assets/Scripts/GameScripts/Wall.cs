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
    [SerializeField] GameObject WallShield;
    [SerializeField] private int bonusHealth;
    [SerializeField] Button button;

    private bool invulnerable = false, blademail = false;

    private float protectedTime;

    public static Action OnWallDeath;

    private Action<bool> action;

    public static Wall instance;

    private void Awake()
    {
        instance = this;
        action = (pause =>
        {
            if (!pause) SelfUnpause();
            else SelfPause();
        });
        PauseSystem.OnPauseStateChanged += action;
        if (YG2.saves.supplies[0] == 0) button.interactable = false;
    }

    public void Protect()
    {
        var temp = YG2.saves.supplies;
        temp[0]--;
        YG2.saves.supplies = temp;
        YG2.SaveProgress();
        invulnerable = true;
        StartCoroutine(StopProtection(protectionTime));
        WallShield.SetActive(true);
    }

    private IEnumerator StopProtection(float time)
    {
        yield return new WaitForSeconds(time);
        invulnerable = false;
        protectedTime = 0;
        WallShield.SetActive(false);
        if (YG2.saves.supplies[0] != 0) button.interactable = true;
    }

    private int health;
    private int maxHealth;


    private void Start()
    {
      /*  for (int i = transform.childCount - 1; i >= 0; i--)
        {
            if (i != YG2.saves.wallLevel) transform.GetChild(i).gameObject.SetActive(false);
            else transform.GetChild(i).gameObject.SetActive(true);
        }*/
        EnemyZombie.OnZombieHitWall += TakeDamage;
        maxHealth = settings.wallLevelsHp[YG2.saves.wallLevel];
        health = maxHealth;
        UpdateWallHp();
    }

    private void Update()
    {
        if (invulnerable)
        {
            protectedTime += Time.deltaTime;
        }
    }

    public void TakeDamage(EnemyZombie z, int damage)
    {
        if (invulnerable) return;
        health -= damage;
        if (blademail)
        {
            z.TakeDamage(damage / 2, false);
        }
        if (health <= 0)
        {
            EnemyZombie.OnZombieHitWall -= TakeDamage;
            OnWallDeath?.Invoke();
            Destroy(gameObject);
            // death
        }
        UpdateWallHp();
    }
    public void UpdateWallHp()
    {
        wallHpBar.fillAmount = (float)health / maxHealth;
        healthAmount.text = $"{health}";
    }

    private void OnDestroy()
    {
        EnemyZombie.OnZombieHitWall -= TakeDamage;
        PauseSystem.OnPauseStateChanged -= action;
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

    public void Heal()
    {
        health += bonusHealth;
        if (health > maxHealth) maxHealth = health;
        UpdateWallHp();
    }

    public void SetBlademail()
    {
        blademail = true;
    }

}
