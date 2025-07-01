using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class WeaponUI : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI dmg, aspd, crit, critChance;
    [SerializeField] Button buyUpgradeButton;
    [SerializeField] TextMeshProUGUI buyUpgradeButtonText;
    [SerializeField] Transform weaponsTransform;
    [SerializeField] GunShopUIScript gunShop;
    private int id;
    private WeaponSettings ws;
    private int lvl;

    private static int curId;

    public void Setup(int id, WeaponSettings ws)
    {
        this.id = id;
        this.ws = ws;
        int lv = YG2.saves.weaponLevels[id];
        lvl = lv;
        Refresh();
        curId = id;
    }

    public static int GetCurrentId()
    {
        return curId;
    }

    public void BuyOrUpgrade()
    {
        YG2.saves.cash -= ws.levels[lvl + 1].cost;
        lvl++;
        var clone = YG2.saves.weaponLevels;
        clone[id]++;
        YG2.saves.weaponLevels = clone;
        YG2.SaveProgress();
        Refresh();
        gunShop.Refresh();
    }

    private void Refresh()
    {
        for (int i = 0; i < weaponsTransform.childCount; i++)
        {
            weaponsTransform.GetChild(i).gameObject.SetActive(i == id);
        }
        if (lvl == -1)
        {// localize everything
            var l = ws.levels[0];
            buyUpgradeButtonText.text = $"Buy\n{l.cost}$";
            dmg.text = $"Damage: {l.damage}";
            aspd.text = $"Attack speed: {l.aspd}";
            crit.text = $"Crit: {l.crit * 100}%";
            critChance.text = $"Crit chance: {l.critChance}%";
        }
        else if (lvl == ws.levels.Length - 1)
        {// localize everything
            var l = ws.levels[lvl];
            buyUpgradeButtonText.text = "MAX";
            buyUpgradeButton.interactable = false;
            dmg.text = $"Damage: {l.damage}";
            aspd.text = $"Attack speed: {l.aspd}";
            crit.text = $"Crit: {l.crit * 100}%";
            critChance.text = $"Crit chance: {l.critChance}%";
            return;
        }
        else
        { // localize everything
            buyUpgradeButtonText.text = $"Upgrade\n{ws.levels[lvl + 1].cost}$";
            var l = ws.levels[lvl];
            var l2 = ws.levels[lvl + 1];
            dmg.text = $"Damage: {l.damage}" + (l2.damage > l.damage ? $" (+{l2.damage - l.damage})" : "");
            aspd.text = $"Attack speed: {l.aspd}" + (l2.aspd > l.aspd ? $" (+{(decimal)l2.aspd - (decimal)l.aspd})" : "");
            crit.text = $"Crit: {l.crit * 100}%" + (l2.crit > l.crit ? $" (+{((decimal)l2.crit - (decimal)l.crit) * 100}%)" : "");
            critChance.text = $"Crit chance: {l.critChance}%" + (l2.critChance > l.critChance ? $" (+{(decimal)l2.critChance - (decimal)l.critChance}%)" : "");
        }
        buyUpgradeButton.interactable = ws.levels[lvl + 1].cost <= YG2.saves.cash;
    }

}
