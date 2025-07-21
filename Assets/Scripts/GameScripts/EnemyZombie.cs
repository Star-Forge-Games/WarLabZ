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
    public int maxHealth = 10;
    [SerializeField] protected int damage;
    [SerializeField] protected float speed;
    [SerializeField] private int difficulty;
    [SerializeField] private int money;
    [SerializeField] protected float slowStartZ = 35, slowEndZ = 10;
    [SerializeField] protected bool boss;
    [SerializeField] protected GameObject healthCanvas;
    [SerializeField] protected GameObject critCanvas, instaKillCanvas;
    [SerializeField] protected GameObject slowCanvas;
    [SerializeField] AudioClip deathSound;
    [SerializeField] protected float eHpMult, eDmgMult, eHpMultAddPerWave, eDmgMultAddPerWave;
    public float endlessDelayToNextZombie;

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
    protected bool wall = false, stunned = false;
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
        
        UpdateHealthUI(maxHealth, currentHealth);
        Wall.OnWallDeath += RunFurther;
    }

    public void Update()
    {
        if (dead) return;
        if (SkillsPanel.zombieSlow)
        {
            if (transform.position.z <= slowStartZ && transform.position.z >= slowEndZ)
            {
                direction.z = -speed * 0.6f;
                slowCanvas.SetActive(true);
            }
            else
            {
                direction.z = -speed;
                slowCanvas.SetActive(false);
            }
        } else
        {
            slowCanvas.SetActive(false);
        }
        if (!wall && !stunned)
        {
            characterController.Move(direction * Time.deltaTime);
            Vector3 pos = transform.position;
            pos.y = 1;
            transform.position = pos;
        }
    }

    public virtual void TakeDamage(int damage, bool crit, bool instaKill)
    {
        if (instaKill&&!boss)
        {
            instaKillCanvas.SetActive(true);
            StartCoroutine(StopCanvas(false));
            currentHealth = 0;
        }
        else
        {
            currentHealth -= damage;
        }
        if (crit)
        {
            critCanvas.SetActive(true);
            StartCoroutine(StopCanvas(true));
        }
        if (currentHealth <= 0)
        {
            GameObject g = new GameObject("DeathSound", typeof(AudioSource));
            g.transform.parent = transform;
            g.transform.localPosition = Vector3.zero;
            g.GetComponent<AudioSource>().clip = deathSound;
            g.GetComponent<AudioSource>().volume = 0.025f;
            g.GetComponent<AudioSource>().Play();
            critCanvas.SetActive(false);
            slowCanvas.SetActive(false);
            dead = true;
            Destroy(GetComponent<CharacterController>());
            Destroy(healthCanvas);
            KillsCount.kills++;
            if (boss) KillsCount.bosses++;
            if (SkillsPanel.lifesteal == true) Wall.instance.Lifesteal(boss);
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

    protected IEnumerator StopCanvas(bool crit)
    {
        yield return new WaitForSeconds(0.25f);
        if (crit) critCanvas.SetActive(false);
        else instaKillCanvas.SetActive(false);
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

    public virtual void SelfPause()
    {
        anim.speed = 0;
        enabled = false;
    }

    public virtual void SelfUnpause()
    {
        anim.speed = 1;
        enabled = true;
    }

    public virtual void Attack()
    {
        OnZombieHitWall?.Invoke(this, damage);
    }

    protected void RunFurther()
    {
        stunned = false;
        wall = true;
        if (!dead) anim.Play("ZombieRoar");
    }

    private void OnDestroy()
    {
        Wall.OnWallDeath -= RunFurther;
    }

    public virtual void Stun()
    {
        if (boss) return;
        if (stunned) return;
        stunned = true;
        anim.Play("Stun");
    }

    public void Unstun()
    {
        stunned = false;
        if (dead) return;
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

    internal void Setup(float hpm, float dm, float mm, float sm, int endlessWave)
    {
        if (endlessWave >= 0)
        {
            float hpmult = eHpMult + eHpMultAddPerWave * (endlessWave + 1);
            float dmult = eDmgMult + eDmgMultAddPerWave * (endlessWave + 1);
            maxHealth = (int)Mathf.Clamp((hpmult * maxHealth), maxHealth + 1, int.MaxValue);
            damage = (int)Mathf.Clamp((dmult * damage), damage + 1, int.MaxValue);
            speed = speed * 1.25f;
            moneyDropChance = 1;
        }
        else
        {
            if (hpm != 1) maxHealth = (int)Mathf.Clamp((hpm * maxHealth), maxHealth + 1, int.MaxValue);
            if (dm != 1) damage = (int)Mathf.Clamp((dm * damage), damage + 1, int.MaxValue);
            moneyDropChance = (int)(moneyDropChance * mm);
            speed = Mathf.Clamp((int)(sm * speed), speed, speed * 1.25f);
        }
        currentHealth = maxHealth;
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
    }
}
