using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Collections;

public class EnemyZombie : MonoBehaviour
{

    [SerializeField] private int moneyDropChance;
    [SerializeField] private Animator anim;
    [SerializeField] private Image healthBar;
    [SerializeField] private TMP_Text healthAmount;
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private int damage;
    [SerializeField] private float speed;
    [SerializeField] private int difficulty;
    [SerializeField] private int money;
    [SerializeField] private float slowStartZ = 35, slowEndZ = 10;
    [SerializeField] private bool boss;

    private int currentHealth;
    private Vector3 direction = Vector3.zero;
    private CharacterController characterController;
    private bool wall = false, stunned = false;
    public static Action<EnemyZombie, int> OnZombieHitWall;
    public static Action<EnemyZombie, float, int> OnZombieDie;

    public int GetDifficulty()
    {
        return difficulty;
    }

    public void Start()
    {
        direction.z = -speed;
        if (transform.position.z <= slowStartZ && transform.position.z >= slowEndZ)
        {
            direction.z *= 0.9f;
        }
        characterController = GetComponent<CharacterController>();
        if (boss)
        {
            if (SkillsPanel.bossHealthReduction)
            {
                maxHealth = (int) (maxHealth * 0.95f);
                currentHealth = maxHealth;
            }
        } else
        {
            if (SkillsPanel.zHealthReduction)
            {
                maxHealth = (int)(maxHealth * 0.975f);
                currentHealth = maxHealth;
            }
        }
            UpdateHealthUI(maxHealth, currentHealth);
        Wall.OnWallDeath += RunFurther;
    }

    public void MultiplyHp(float health)
    {
        maxHealth = (int)(health * maxHealth);
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (!wall && !stunned) characterController.Move(direction * Time.deltaTime);
    }

    public void TakeDamage(int damage, bool crit)
    {
        currentHealth -= damage;
        if (crit)
        {
            // визуал крита
        }
        if (currentHealth <= 0)
        {
            KillsCount.kills += 1;
            if (SkillsPanel.lifesteal) Wall.instance.Lifesteal(boss);
            OnZombieDie?.Invoke(this, moneyDropChance, money);
            // DEATH ANIMATION
            Destroy(gameObject);
            return;
        }
        UpdateHealthUI(maxHealth, currentHealth);
    }

    public void UpdateHealthUI(int maxHealth, int currentHealth)
    {
        healthBar.fillAmount = (float)currentHealth / maxHealth;
        healthAmount.text = $"{currentHealth}";
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Wall"))
        {
            wall = true;
            anim.Play("Attack");
        }
    }

    public void SelfPause()
    {
        anim.speed = 0;
        enabled = false;
    }

    public void SelfUnpause()
    {
        anim.speed = 1;
        enabled = true;
    }

    public void Attack()
    {
        OnZombieHitWall?.Invoke(this, damage);
    }

    private void RunFurther()
    {
        wall = false;
    }

    private void OnDestroy()
    {
        Wall.OnWallDeath -= RunFurther;
    }

    internal void Stun()
    {
        stunned = true;
        anim.Play("Stun");
    }

    public void Unstun()
    {
        stunned = false;
        if (wall)
        {
            anim.Play("Attack");
        } else
        {
            anim.Play("Walk");
        }
    }
}
