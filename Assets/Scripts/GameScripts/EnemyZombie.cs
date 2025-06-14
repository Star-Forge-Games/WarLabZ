using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class EnemyZombie : MonoBehaviour
{

    [SerializeField] private int moneyDropChance;
    [SerializeField] private Animator anim;
    [SerializeField] private Image healthBar;
    [SerializeField] private TMP_Text healthAmount;
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private int damage;
    [SerializeField] private float speed;

    private int currentHealth;
    private Vector3 direction = Vector3.zero;
    private CharacterController characterController;
    private bool wall = false;
    public static Action<EnemyZombie, int> OnZombieHitWall;
    public static Action<EnemyZombie, float> OnZombieDie;

    public void Start()
    {
        direction.z = -speed;
        characterController = GetComponent<CharacterController>();
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
        if (!wall) characterController.Move(direction * Time.deltaTime);
    }

    public void TakeDamage(int damage, bool crit)
    {
        currentHealth -= damage;
        if (crit)
        {
            // ������ �����
        }
        if (currentHealth <= 0)
        {
            KillsCount.kills += 1;
            OnZombieDie?.Invoke(this, moneyDropChance);
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

}
