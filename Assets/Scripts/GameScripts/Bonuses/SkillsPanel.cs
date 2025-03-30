using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillsPanel : MonoBehaviour
{

    private List<int> modifiersLeft = new() { 0, 1, 2, 3, 4 };
    public static Action<int> OnSkillSelect;
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
        int i = id == 0? b1id : b2id;
        switch(i)
        {
            case 0:
                OnSkillSelect?.Invoke(i);
                Weapon.instance.SetInstaKill();
                break;
            case 1:
                Weapon.instance.SetThrough();
                break;
            case 2:
                OnSkillSelect?.Invoke(i);
                Weapon.instance.SetTwinShot();
                break;
            case 3:
                MoneySystem.instance.SetBonus();
                break;
            default:
                Wall.instance.Heal();
                break;
        }
        modifiersLeft.Remove(i);
        PauseSystem.instance.Unpause();
        gameObject.SetActive(false);
    }
}
