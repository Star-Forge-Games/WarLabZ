using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;
using static LocalizationHelperModule;

public class SuppliesShopUI : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI wAmount, bAmount, aAmount, wPrice, bPrice, aPrice, moneyText, descText;
    [SerializeField] Button buyW, buyB, buyA;
    [SerializeField] int priceW, priceB, priceA;

    void OnEnable()
    {
        descText.text = Loc("suppliesdescription");
        Refresh();
    }

    void Refresh()
    {
        int money = YG2.saves.cash;
        int wa = YG2.saves.supplies[0];
        int ba = YG2.saves.supplies[1];
        int aa = YG2.saves.supplies[2];
        wAmount.text = $"{wa}";
        bAmount.text = $"{ba}";
        aAmount.text = $"{aa}";
        wPrice.text = $"{priceW}$";
        bPrice.text = $"{priceB}$";
        aPrice.text = $"{priceA}$";
        buyW.interactable = money >= priceW;
        buyB.interactable = money >= priceB;
        buyA.interactable = money >= priceA;
        moneyText.text = $"{money} $";
    }

    public void Buy(int id)
    {
        int price = id == 0? priceW : (id == 1? priceB : priceA);
        YG2.saves.cash -= price;
        var temp = YG2.saves.supplies;
        temp[id]++;
        YG2.saves.supplies = temp;
        YG2.SaveProgress();
        Refresh();
    }

    public void SetDescText(int i)
    {
        descText.text = Loc(i == 0? "shielddescription" : i == 1? "bomberdescription" : "raindescription");
    }
}
