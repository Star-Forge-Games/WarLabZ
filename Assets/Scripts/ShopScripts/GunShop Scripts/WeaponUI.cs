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
    [SerializeField] GameObject weaponModel;
    [SerializeField] GameObject lockObject;
    [SerializeField] int id;
    private Image bg;

    private void Start()
    {
        bg = GetComponent<Image>();
        Refresh();
        RefreshWeapons += Refresh;
    }

    public static Action OnUpgrade;
    public static Action RefreshWeapons;

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
        } else
        {
            Unlock();
        }
        if (YG2.saves.selectedWeapon == id)
        {
            Select();
        }
        else
        {
            Deselect();
        }
    }

    public void Select()
    {
        bg.color = new Color(236/255f, 250/255f, 65/255f, 137/255f);
    }

    public void SelectWeapon()
    {
        YG2.saves.selectedWeapon = id;
        YG2.SaveProgress();
        RefreshWeapons.Invoke();
    }

    public void Deselect()
    {
        bg.color = new Color(196/255f, 178/255f, 150/255f, 137/255f);
    }

    public void Lock()
    {
        lockObject.SetActive(true);
        weaponModel.SetActive(false);
    }

    public void Unlock()
    {
        lockObject.SetActive(false);
        weaponModel.SetActive(true);
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
