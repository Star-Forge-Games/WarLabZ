using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;
using static LocalizationHelperModule;

public class SkillsPanel : MonoBehaviour
{

    private List<int> modifiersLeft = new() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18 };
    public static Action<int> OnTurretSkillSelect;
    [SerializeField] Image b1, b2;
    [SerializeField] Modifier[] modifiers;
    [SerializeField] PausePanel pausePanel;
    [SerializeField] private int maxSkills;
    [SerializeField] private PlayerController player;
    [SerializeField] GameObject slowZone;
    [SerializeField] GameObject adButton, adText, adPanel;
    private int skillsSelected = 0;
    public static bool zombieSlow, lifesteal, zHealthReduction, bossHealthReduction;
    private bool turretSkillsAdded = false;

    [Serializable]
    public struct Modifier
    {
        public Sprite sprite;
        public string key;
    }

    private int b1id, b2id;

    public void OnEnable()
    {
        adButton.SetActive(true);
        adText.SetActive(true);
        if (!turretSkillsAdded)
        {
            turretSkillsAdded = true;
            if (YG2.saves.wallLevel >= 2)
            {
                modifiersLeft.AddRange(new int[] { 19, 20, 21, 22, 23 });
            }
        }
        if (modifiersLeft.Count == 1)
        {
            b1id = modifiersLeft[UnityEngine.Random.Range(0, modifiersLeft.Count)];
            b1.sprite = modifiers[b1id].sprite;
            b1.GetComponentInChildren<TextMeshProUGUI>().text = Loc(modifiers[b1id].key);
            Vector3 pos = b1.rectTransform.anchoredPosition;
            pos.x = 0;
            b1.rectTransform.anchoredPosition = pos;
            b2.gameObject.SetActive(false);
            return;
        }
        b1id = modifiersLeft[UnityEngine.Random.Range(0, modifiersLeft.Count)];
        b2id = modifiersLeft[UnityEngine.Random.Range(0, modifiersLeft.Count)];
        while (b2id == b1id)
        {
            b2id = modifiersLeft[UnityEngine.Random.Range(0, modifiersLeft.Count)];
        }
        b1.sprite = modifiers[b1id].sprite;
        b2.sprite = modifiers[b2id].sprite;
        b1.GetComponentInChildren<TextMeshProUGUI>().text = Loc(modifiers[b1id].key);
        b2.GetComponentInChildren<TextMeshProUGUI>().text = Loc(modifiers[b2id].key);
    }

    public void Select(int id)
    {
        int i = id == 0 ? b1id : b2id;
        pausePanel.AddSkill(id == 0 ? b1.sprite : b2.sprite, skillsSelected);
        switch (i)
        {
            case 0:
                Weapon.instance.SetMultiShot();
                break;
            case 1:
                Weapon.instance.IncreaseCritDamage(0.5f);
                break;
            case 2:
                Weapon.instance.IncreaseDamageModifier(true, 1);
                break;
            case 3:
                Weapon.instance.IncreaseRateModifier(true, 0.5f);
                break;
            case 4:
                Weapon.instance.IncreaseCritChance(2f);
                break;
            case 5:
                Weapon.instance.SetShotgun();
                break;
            case 6:
                Weapon.instance.SetThrough();
                break;
            case 7:
                Weapon.instance.AddStunChance(7);
                break;
            case 8:
                bossHealthReduction = true;
                break;
            case 9:
                Weapon.instance.SetInstaKill();
                break;
            case 10:
                BombSystem.instance.Enable();
                break;
            case 11:
                Wall.instance.SetBlademail();
                break;
            case 12:
                Weapon.instance.SetBomb();
                break;
            case 13:
                player.IncreaseSpeed();
                break;
            case 14:
                zombieSlow = true;
                slowZone.SetActive(true);
                break;
            case 15:
                zHealthReduction = true;
                break;
            case 16:
                lifesteal = true;
                break;
            case 17:
                MoneySystem.instance.SetBonus();
                break;
            case 18:
                Wall.instance.Heal();
                break;
            default:
                OnTurretSkillSelect?.Invoke(i);
                break;
        }
        modifiersLeft.Remove(i);
        PauseSystem.instance.Unpause(true);
        gameObject.SetActive(false);
        skillsSelected++;
    }

    public bool ReachedMaxSkillLimit()
    {
        return maxSkills == skillsSelected;
    }

    public void ShowAd()
    {
        adPanel.SetActive(true);
        gameObject.SetActive(false);
        AudioListener.volume = 0;
        YG2.RewardedAdvShow("2", () => Reroll());
    }

    public void Reroll()
    {
        gameObject.SetActive(true);
        adButton.SetActive(false);
        adText.SetActive(false);
        adPanel.SetActive(false);
        AudioListener.volume = YG2.saves.soundOn ? 1 : 0;
    }
}
