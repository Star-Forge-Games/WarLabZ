using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyZombie : MonoBehaviour
{

    [SerializeField] private int moneyDropChance;
    [SerializeField] protected Animator anim;
    [SerializeField] protected Image healthBar;
    [SerializeField] protected TMP_Text healthAmount;
    [SerializeField] protected int maxHealth = 10;
    [SerializeField] protected int damage;
    [SerializeField] protected float speed;
    [SerializeField] private int difficulty;
    [SerializeField] private int money;
    [SerializeField] private float slowStartZ = 35, slowEndZ = 10;
    [SerializeField] private bool boss;
    [SerializeField] protected GameObject healthCanvas;

    [Serializable]
    public struct ColoredPart
    {
        public GameObject obj;
        internal Color[] colors;
    }

    [SerializeField] protected ColoredPart[] parts;

    protected int currentHealth;
    protected Vector3 direction = Vector3.zero;
    protected CharacterController characterController;
    private bool wall = false, stunned = false;
    public static Action<EnemyZombie, int> OnZombieHitWall;
    public static Action<EnemyZombie, float, int> OnZombieDie;
    protected bool dead;

    public bool IsDead()
    {
        return dead;
    }

    public int GetDifficulty()
    {
        return difficulty;
    }

    public void Start()
    {
        for (int i = 0; i < parts.Length; i++)
        {
            var smr = parts[i].obj.GetComponent<SkinnedMeshRenderer>();
            parts[i].colors = (from m in smr.materials select m.color).ToArray();
        }
        direction.z = -speed;
        characterController = GetComponent<CharacterController>();
        if (boss)
        {
            if (SkillsPanel.bossHealthReduction)
            {
                maxHealth = (int)(maxHealth * 0.95f);
                currentHealth = maxHealth;
            }
        }
        else
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

    public void Update()
    {
        if (dead) return;
        if (SkillsPanel.zombieSlow)
        {
            if (transform.position.z <= slowStartZ && transform.position.z >= slowEndZ)
            {
                direction.z = -speed * 0.6f;
            }
            else
            {
                direction.z = -speed;
            }
        }
        if (!wall && !stunned) characterController.Move(direction * Time.deltaTime);
    }

    public virtual void TakeDamage(int damage, bool crit, bool instaKill)
    {
        if (instaKill)
        {
            if (!boss) currentHealth = 0;
        }
        else
        {
            currentHealth -= damage;
        }
        if (crit)
        {
            // визуал крита
        }
        if (currentHealth <= 0)
        {
            dead = true;
            Destroy(GetComponent<CharacterController>());
            Destroy(healthCanvas);
            KillsCount.kills += 1;
            if (SkillsPanel.lifesteal) Wall.instance.Lifesteal(boss);
            OnZombieDie?.Invoke(this, moneyDropChance, money);
            anim.Play("Death");
            StartCoroutine(Die());
            transform.parent = null;
        } else
        {
            StopCoroutine(nameof(StopFlash));
            foreach (var part in parts)
            {
                foreach (var mat in part.obj.GetComponent<SkinnedMeshRenderer>().materials)
                {
                    mat.color = Color.red;
                }
            }
            StartCoroutine(nameof(StopFlash));
            UpdateHealthUI(maxHealth, currentHealth);
        }
    }

    protected IEnumerator StopFlash()
    {
        yield return new WaitForSeconds(0.2f);
        foreach (var part in parts)
        {
            for (int i = 0; i < part.obj.GetComponent<SkinnedMeshRenderer>().materials.Length; i++)
            {
                part.obj.GetComponent<SkinnedMeshRenderer>().materials[i].color = part.colors[i];
            }
        }
    }

    public void UpdateHealthUI(int maxHealth, int currentHealth)
    {
        healthBar.fillAmount = (float)currentHealth / maxHealth;
        healthAmount.text = $"{currentHealth}";
    }

    public virtual void OnControllerColliderHit(ControllerColliderHit hit)
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
        anim.Play("Walk");
    }

    private void OnDestroy()
    {
        Wall.OnWallDeath -= RunFurther;
    }

    internal void Stun()
    {
        if (boss) return;
        if (stunned) return;
        stunned = true;
        anim.Play("Stun");
    }

    public void Unstun()
    {
        stunned = false;
        if (wall)
        {
            anim.Play("Attack");
        }
        else
        {
            anim.Play("Walk");
        }
    }

    protected IEnumerator Die()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}
