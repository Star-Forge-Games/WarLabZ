using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class Wall : MonoBehaviour
{
    [SerializeField] private TMP_Text healthAmount;
    [SerializeField] private Slider wallHpBar;
    [SerializeField] private WallSettings settings;
    [SerializeField] private float protectionTime;
    [SerializeField] GameObject WallShield;
    [SerializeField] private float bonusHealthPercent = 0.1f;
    [SerializeField] Button button;
    [SerializeField] private Animator wallHpBarAnimator;
    [SerializeField] int protectCooldownInSeconds;
    [SerializeField] Slider protectCooldownSlider;
    [SerializeField] private float bladeMailDemagePercent = 0.5f;
    [SerializeField] private GameObject bladeMailObject;
    [SerializeField] TextMeshProUGUI amount;
    [SerializeField] GameObject pauseButton, supPanel;
    [SerializeField] HealthLevel[] healthLevels;
    [SerializeField] Image fill;
    public static Action OnWallDeath;
    private float lifestealCounter = 0;

    [Serializable]
    public struct HealthLevel
    {
        public Color color;
        [Range(1, 100)]
        public int healthPercent;
    }

    private int health;
    internal int maxHealth;
    private bool paused = false;
    private float cooldown = 0;
    private bool shaking;
    private bool invulnerable = false, blademail = false;
    private float protectedTime;
    private Action<bool> action;
    public static Wall instance;
    [SerializeField] private Transform bulletContainer, enemyContainer;

    private void Awake()
    {
        instance = this;
        action = (pause =>
        {
            if (!pause) SelfUnpause();
            else SelfPause();
        });
        PauseSystem.OnPauseStateChanged += action;
        protectCooldownSlider.maxValue = protectCooldownInSeconds;
    }

    public void Protect()
    {
        var temp = YG2.saves.supplies;
        temp[0]--;
        if (temp[0] == 0) button.interactable = false;
        YG2.saves.supplies = temp;
        YG2.SaveProgress();
        amount.text = temp[0] == 0 ? "" : $"{temp[0]}";
        button.interactable = false;
        cooldown = protectCooldownInSeconds;
        invulnerable = true;
        WallShield.SetActive(true);
        StartCoroutine(StopProtection(protectionTime));
    }

    private IEnumerator StopProtection(float time)
    {
        yield return new WaitForSeconds(time);
        invulnerable = false;
        protectedTime = 0;
        WallShield.SetActive(false);
    }

    private void Start()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            if (i != YG2.saves.wallLevel) transform.GetChild(i).gameObject.SetActive(false);
            else
            {
                if (i >= 2) transform.GetChild(i).gameObject.GetComponent<TurretContainer>().Setup(bulletContainer, enemyContainer);
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }
        EnemyZombie.OnZombieHitWall += TakeDamage;
        maxHealth = settings.wallLevels[YG2.saves.wallLevel].hp;
        health = maxHealth;
        wallHpBar.maxValue = maxHealth;
        wallHpBar.value = health;
        UpdateWallHp();
        if (YG2.saves.supplies[0] == 0)
        {
            button.interactable = false;
            amount.text = "";
        }
        else
        {
            button.interactable = true;
            amount.text = $"{YG2.saves.supplies[0]}";
        }
        var col = GetComponent<BoxCollider>();
        Vector3 size = col.size;
        size.z = settings.wallLevels[YG2.saves.wallLevel].width;
        col.size = size;
    }

    private void Update()
    {
        if (invulnerable)
        {
            protectedTime += Time.deltaTime;
        }
        if (paused) return;
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
            protectCooldownSlider.value = cooldown;
            if (cooldown <= 0)
            {
                if (YG2.saves.supplies[0] != 0) button.interactable = true;
                protectCooldownSlider.value = 0;
            }
        }
    }

    public void TakeDamage(EnemyZombie z, int damage)
    {
        if (damage == -1)
        {
            if (invulnerable) return;
            health -= (int) (maxHealth * 0.2f);
            if (!shaking)
            {
                shaking = true;
                wallHpBarAnimator.Play("Shake");
                StartCoroutine(nameof(StopShake));
                if (health <= 0)
                {
                    EnemyZombie.OnZombieHitWall -= TakeDamage;
                    OnWallDeath?.Invoke();
                    wallHpBar.gameObject.SetActive(false);
                    pauseButton.SetActive(false);
                    supPanel.SetActive(false);
                    Destroy(gameObject);
                    return;
                }
                UpdateWallHp();
            }
            return;
        }
        if (blademail)
        {
            float bmDmg = damage * bladeMailDemagePercent;
            if (bmDmg < 1) bmDmg = 1;
            z.TakeDamage((int)bmDmg, false, false);
        }
        if (invulnerable) return;
        health -= damage;
        if (!shaking)
        {
            shaking = true;
            wallHpBarAnimator.Play("Shake");
            StartCoroutine(nameof(StopShake));
        }
        if (health <= 0)
        {
            EnemyZombie.OnZombieHitWall -= TakeDamage;
            OnWallDeath?.Invoke();
            wallHpBar.gameObject.SetActive(false);
            Destroy(gameObject);
            return;
        }
        UpdateWallHp();
    }
    public void UpdateWallHp()
    {
        wallHpBar.value = health;
        int percent = (int)(((float)health / maxHealth) * 100);
        healthAmount.text = $"{health}";
        for (int i = healthLevels.Length - 1; i >= 0; i--)
        {
            if (healthLevels[i].healthPercent < percent) continue;
            fill.color = healthLevels[i].color;
            return;
        }
    }

    private IEnumerator StopShake()
    {
        yield return new WaitForSeconds(0.3f);
        shaking = false;
    }

    private void OnDestroy()
    {
        EnemyZombie.OnZombieHitWall -= TakeDamage;
        PauseSystem.OnPauseStateChanged -= action;
    }

    private void SelfPause()
    {
        paused = true;
        if (!invulnerable) return;
        StopCoroutine(nameof(StopProtection));
    }

    private void SelfUnpause()
    {
        paused = false;
        if (!invulnerable) return;
        StartCoroutine(StopProtection(protectionTime - protectedTime));
    }

    public void Heal()
    {
        health += (int)(maxHealth * bonusHealthPercent);
        if (health > maxHealth) maxHealth = health;
        UpdateWallHp();
    }

    public void Lifesteal(bool boss)
    {
        float boost = boss ? 5 : 0.2f;
        lifestealCounter += boost;
        int bonus = 0;
        while (lifestealCounter > 1)
        {
            bonus++;
            lifestealCounter-=5;
        }
        if (bonus > 0)
        {
            if (health + bonus > maxHealth) health = maxHealth;
            else health += bonus;
            UpdateWallHp();
        }
    }

    public void SetBlademail()
    {
        blademail = true;
        bladeMailObject.SetActive(true);
    }

}
