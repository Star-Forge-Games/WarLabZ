using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillsPanel : MonoBehaviour
{

    private List<int> modifiersLeft = new() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25 };
    public static Action<int> OnTurretSkillSelect;
    [SerializeField] Image b1, b2;
    [SerializeField] Modifier[] modifiers;

    [Serializable]
    public struct Modifier
    {
        public Sprite sprite;
        public string text;
    }

    private int b1id, b2id;

    public void OnEnable()
    {
        b1id = modifiersLeft[UnityEngine.Random.Range(0, modifiersLeft.Count)];
        b2id = modifiersLeft[UnityEngine.Random.Range(0, modifiersLeft.Count)];
        while (b2id == b1id)
        {
            b2id = modifiersLeft[UnityEngine.Random.Range(0, modifiersLeft.Count)];
        }
        b1.sprite = modifiers[b1id].sprite;
        b2.sprite = modifiers[b2id].sprite;
        b1.GetComponentInChildren<TextMeshProUGUI>().text = modifiers[b1id].text;
        b2.GetComponentInChildren<TextMeshProUGUI>().text = modifiers[b2id].text;
    }

    public void Select(int id)
    {
        int i = id == 0 ? b1id : b2id;
        switch (i)
        {
            case 0:
                Weapon.instance.SetInstaKill();
                break;
            case 1:
                Weapon.instance.SetThrough();
                break;
            case 2:
                Weapon.instance.SetTwinShot();
                break;
            case 3:
                MoneySystem.instance.SetBonus();
                break;
            case 4:
                Wall.instance.Heal();
                break;
            case 5:
                BombSystem.instance.Enable();
                break;
            case 6:
                Wall.instance.SetBlademail();
                break;
            case 7:
                Weapon.instance.SetShotgun();
                break;
            case 8:
                Weapon.instance.IncreaseCritChance(10);
                break;
            case 9:
                Weapon.instance.IncreaseDamageModifier(true, 2);
                break;
            case 10:
                Weapon.instance.IncreaseDamageModifier(false, 1);
                break;
            case 11:
                Weapon.instance.IncreaseRateModifier(true, 2);
                break;
            case 12:
                Weapon.instance.IncreaseRateModifier(false, 1);
                break;
            case 13:
                Weapon.instance.SetBomb();
                break;
            case 14:
                Weapon.instance.SetMultiShot();
                break;
            case 15:
                Weapon.instance.IncreaseCritDamage(0.1f);
                break;
            default:
                OnTurretSkillSelect?.Invoke(i);
                break;
        }
        modifiersLeft.Remove(i);
        PauseSystem.instance.Unpause();
        gameObject.SetActive(false);
    }
}
