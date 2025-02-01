using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class HealthBar : MonoBehaviour
{
    public GameObject CanvasHealth; //������ �� ������ ������, ���������� �������
    public Image Bar; // ������ �� ��� �����, ������� ��������


    public void UpdateHealthBar(int maxHealth, int currentHealth)
    {
        float healthPercentage = (float)currentHealth / maxHealth; //���������� �������� �� �� �������������

        Bar.fillAmount = healthPercentage; //���������� �������� fillAmount � UI Bar, ������� ������������ �������� ��������
    }



}
