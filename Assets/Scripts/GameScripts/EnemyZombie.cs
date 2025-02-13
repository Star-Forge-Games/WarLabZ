using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Collections;
using Random = UnityEngine.Random;

public class EnemyZombie : MonoBehaviour
{


    [SerializeField] private GameObject dollars;
    [SerializeField] private float moneyDropChance;

    [SerializeField] private Animator anim;
    [SerializeField] private Image healthBar;
    [SerializeField] private TMP_Text healthAmount;
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private int damage;

    private int currentHealth;
    [SerializeField] private float speed;
    private Vector3 direction = Vector3.zero;
    private CharacterController characterController;
    private bool wall = false;

    public static Action OnZombieHitPlayer;
    public static Action<int> OnZombieHitWall;

    float random;

    public void Start()
    {
        direction.z = -speed;
        characterController = GetComponent<CharacterController>();
        currentHealth = maxHealth;
        UpdateHealthUI(maxHealth, currentHealth);

    }

    private void Update()
    {
        if (!wall)
            characterController.Move(direction * Time.deltaTime);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            random = Random.Range(0, 100);
            if (random <= moneyDropChance)
            {
                dollars = Instantiate(dollars, transform.position, Quaternion.identity);
                KillsCount.kills += 1;
                PlayerController.money += 1;
                Destroy(gameObject);
            }
            else
            {
                KillsCount.kills += 1;
                Destroy(gameObject);
            }

        }
        UpdateHealthUI(maxHealth, currentHealth);
    }

    public void UpdateHealthUI(int maxHealth, int currentHealth)
    {
        healthBar.fillAmount = (float)currentHealth / maxHealth; // Update healthbar based on health percentage
        healthAmount.text = $"{currentHealth}";
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Player"))
        {
            OnZombieHitPlayer?.Invoke(); // death
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

    public void Attack()
    {
        OnZombieHitWall?.Invoke(damage);
    }

}
