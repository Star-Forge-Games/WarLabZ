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

    public static Action OnZombieHitPlayer;
    public static Action<int> OnZombieHitWall;
    public static Action<EnemyZombie, float> OnZombieDie;

    public void Start()
    {
        direction.z = -speed;
        characterController = GetComponent<CharacterController>();
     
        UpdateHealthUI(maxHealth, currentHealth);
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

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            OnZombieDie?.Invoke(this, moneyDropChance);
            KillsCount.kills += 1;
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
        if (hit.gameObject.CompareTag("Player"))
        {
            OnZombieHitPlayer?.Invoke();
        }
        if (hit.gameObject.CompareTag("Wall"))
        {
            wall = true;
            anim.Play("Attack");
        }
    }

    public void Stop()
    {
        anim.speed = 0;
        enabled = false;
    }

    public void Unpause()
    {
        anim.speed = 1;
        enabled = true;
    }

    public void Attack()
    {
        OnZombieHitWall?.Invoke(damage);
    }

}
