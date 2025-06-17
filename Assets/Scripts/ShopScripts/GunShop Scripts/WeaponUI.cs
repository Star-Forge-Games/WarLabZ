using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class WeaponUI : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI dmg, aspd, crit, critChance, level, upgrade;
    [SerializeField] Button upgradeButton;
    [SerializeField] int id;

    private void Start()
    {
        Refresh();
    }

    public static Action OnUpgrade;

    public virtual void Refresh()
    {
        int lvl = YG2.saves.weaponLevels[id];
        WeaponSettings settings = WeaponDataStorage.instance.GetWeaponSettings(id);
        WeaponSettings.Level wslevel = settings.levels[lvl];
        if (lvl == settings.levels.Length - 1)
        {
            upgradeButton.interactable = false;
            upgrade.text = "MAX";
            level.text = string.Empty;
        } else
        {
            level.text = $"Lv. {lvl + 1}";
            WeaponSettings.Level wslevelNext = settings.levels[lvl + 1];
            if (wslevelNext.cost > YG2.saves.cash)
            {
                upgradeButton.interactable = false;
            }
        }
        dmg.text = $"+{wslevel.damageBuff}";
        aspd.text = $"+{wslevel.aspdBuff}";
        crit.text = $"+{wslevel.critBuff}";
        critChance.text = $"+{wslevel.critChanceBuff}";
        if (!YG2.saves.unlockedWeapons.Contains(id))
        {
            Lock();
        }
        if (YG2.saves.selectedWeapon == id) Select();
        else Deselect();
    }

    public void Select()
    {
        // selection logic
    }

    public void Deselect()
    {
        // deselection logic
    }

    public void Lock()
    {
        
    }

    public void Unlock()
    {
        // unlock logic
    }

    public virtual void Upgrade()
    {
        int lvl = YG2.saves.weaponLevels[id];
        WeaponSettings settings = WeaponDataStorage.instance.GetWeaponSettings(id);
        WeaponSettings.Level wslevel = settings.levels[lvl + 1];
        YG2.saves.cash = YG2.saves.cash - wslevel.cost;
        var temp = YG2.saves.weaponLevels;
        temp[id]++;
        YG2.saves.weaponLevels = temp;
        YG2.SaveProgress();
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            transform.parent.GetChild(i).GetComponent<WeaponUI>().Refresh();
        }
        OnUpgrade?.Invoke();
    }

}
