using UnityEngine;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{
    public Image bar; // ������ �� ��� �����, ������� ��������


    public void UpdateHealthBar(int maxHealth, int currentHealth)
    {
        float healthPercentage = (float)currentHealth / maxHealth; //���������� �������� �� �� �������������
        bar.fillAmount = healthPercentage; //���������� �������� fillAmount � UI Bar, ������� ������������ �������� ��������
    }



}
