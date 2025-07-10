using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using YG;
using static LocalizationHelperModule;
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
    private bool prevFull;

    private static int curId;

    public void Setup(int id, WeaponSettings ws, bool prevFull)
    {
        this.id = id;
        this.ws = ws;
        int lv = YG2.saves.weaponLevels[id];
        lvl = lv;
        this.prevFull = prevFull;
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
        {
            var l = ws.levels[0];
            dmg.text = $"<color=green>{l.damage}</color>";
            aspd.text = $"<color=green>{l.aspd}</color>";
            crit.text = $"<color=green>{l.crit * 100}%</color>";
            critChance.text = $"<color=green>{l.critChance}%</color>";
            if (!prevFull)
            {
                buyUpgradeButtonText.text = $"{Loc("buyprevwall")}\n{l.cost}$";
                buyUpgradeButton.interactable = false;
                return;
            }
            buyUpgradeButtonText.text = $"{Loc("buy")}\n{l.cost}$";
        }
        else if (lvl == ws.levels.Length - 1)
        {
            var l = ws.levels[lvl];
            buyUpgradeButtonText.text = Loc("max");
            buyUpgradeButton.interactable = false;
            dmg.text = $"<color=green>{l.damage}</color>";
            aspd.text = $"<color=green>{l.aspd}</color>";
            crit.text = $"<color=green>{l.crit * 100}%</color>";
            critChance.text = $"<color=green>{l.critChance}%</color>";
            return;
        }
        else
        {
            buyUpgradeButtonText.text = $"{Loc("upgrade")}\n{ws.levels[lvl + 1].cost}$";
            var l = ws.levels[lvl];
            var l2 = ws.levels[lvl + 1];
            dmg.text = $"<color=green>{l.damage}</color>" + (l2.damage > l.damage ? $" <color=red>(+{l2.damage - l.damage})</color>" : "");
            aspd.text = $"<color=green>{l.aspd}</color>" + (l2.aspd > l.aspd ? $" <color=red>(+{(decimal)l2.aspd - (decimal)l.aspd})</color>" : "");
            crit.text = $"<color=green>{l.crit * 100}%</color>" + (l2.crit > l.crit ? $" <color=red>(+{((decimal)l2.crit - (decimal)l.crit) * 100}%)</color>" : "");
            critChance.text = $"<color=green>{l.critChance}%</color>" + (l2.critChance > l.critChance ? $" <color=red>(+{(decimal)l2.critChance - (decimal)l.critChance}%)</color>" : "");
        }
        buyUpgradeButton.interactable = ws.levels[lvl + 1].cost <= YG2.saves.cash;
    }

    private void OnDisable()
    {
        if (weaponsTransform != null)
        for (int i = 0; i < weaponsTransform.childCount; i++)
        {
            weaponsTransform.GetChild(i).gameObject.SetActive(false);
        }
    }

}
