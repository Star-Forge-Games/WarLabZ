using UnityEngine;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{
    public Image bar; // Ссылка на ЮАй Имаге, полоска здоровья


    public void UpdateHealthBar(int maxHealth, int currentHealth)
    {
        float healthPercentage = (float)currentHealth / maxHealth; //Вычисление процента хп от максимального
        bar.fillAmount = healthPercentage; //Обновление значения fillAmount в UI Bar, которая представляет полосуку здоровья
    }



}
