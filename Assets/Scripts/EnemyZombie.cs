using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class EnemyZombie : MonoBehaviour
{

    public int MaxHealth = 10;
    public int CurrentHealth;
    public HealthBar HealthBar;

    [SerializeField] private TMP_Text healthAmount;

    public void Start()
    {
        CurrentHealth = MaxHealth;
        healthAmount.text = CurrentHealth.ToString();
    }


    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        healthAmount.text = CurrentHealth.ToString(); 

        if (CurrentHealth <= 0)
        {
            Destroy(gameObject);
        }

        HealthBar.UpdateHealthBar(MaxHealth, CurrentHealth);
       

    }



}
