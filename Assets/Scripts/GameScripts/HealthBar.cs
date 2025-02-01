using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class HealthBar : MonoBehaviour
{
    public GameObject CanvasHealth; //Ссылка на объект Канвас, содержащий ХелсБар
    public Image Bar; // Ссылка на ЮАй Имаге, полоска здоровья


    public void UpdateHealthBar(int maxHealth, int currentHealth)
    {
        float healthPercentage = (float)currentHealth / maxHealth; //Вычисление процента хп от максимального

        Bar.fillAmount = healthPercentage; //Обновление значения fillAmount в UI Bar, которая представляет полосуку здоровья
    }



}
