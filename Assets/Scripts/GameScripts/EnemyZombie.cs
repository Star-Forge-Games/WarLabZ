using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class EnemyZombie : MonoBehaviour
{

    [SerializeField] private Animator anim;
    [SerializeField] private Image healthBar;
    [SerializeField] private TMP_Text healthAmount;
    [SerializeField] private int maxHealth = 10;
    private int currentHealth;
    [SerializeField] private float speed;
    private Vector3 direction = Vector3.zero;
    private CharacterController characterController;

    public static Action OnZombieHitPlayer;

    public void Start()
    {
        direction.z = -speed;
        characterController = GetComponent<CharacterController>();
        currentHealth = maxHealth;
        UpdateHealthUI(maxHealth, currentHealth);
    }

    private void Update()
    {
        characterController.Move(direction * Time.deltaTime);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
        UpdateHealthUI(maxHealth, currentHealth);
    }

    public void UpdateHealthUI(int maxHealth, int currentHealth)
    {
        healthBar.fillAmount = (float) currentHealth / maxHealth; // Update healthbar based on health percentage
        healthAmount.text = $"{currentHealth}"; // Update health text
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Player"))
        {
            OnZombieHitPlayer?.Invoke();
        }
    }

    public void Stop()
    {
        anim.speed = 0;
        enabled = false;
    }

}
