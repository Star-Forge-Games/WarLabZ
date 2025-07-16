using TMPro;
using UnityEngine;
using YG;
using static LocalizationHelperModule;

public class DonateUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI rewardText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private int price, reward;
    [SerializeField] private int id;
    [SerializeField] private TextMeshProUGUI moneyText;

    private void OnEnable()
    {
        if (priceText != null) priceText.text = $"{price} {Loc("rubles")}";
        rewardText.text = $"{reward}$";
        moneyText.text = MoneyFormat(YG2.saves.cash);
    }

    private void Start()
    {
        YG2.onPurchaseSuccess += ProcessPurchase;
        YG2.onPurchaseFailed += NoPurchase;
    }

    private void OnDestroy()
    {
        YG2.onPurchaseSuccess -= ProcessPurchase;
        YG2.onPurchaseFailed -= NoPurchase;
    }

    public void Purchase()
    {
        AudioListener.volume = 0;
        if (price <= 0)
        {
            YG2.RewardedAdvShow("admoney", () =>
            {
                YG2.saves.cash += reward;
                YG2.SaveProgress();
                AudioListener.volume = YG2.saves.soundOn ? 1 : 0;
                moneyText.text = MoneyFormat(YG2.saves.cash);
            });
            return;
        }
        YG2.BuyPayments("buymoney" + id);
    }

    private void ProcessPurchase(string id)
    {
        if (id != ("buymoney" + this.id)) return;
        YG2.saves.cash += reward;
        YG2.SaveProgress();
        AudioListener.volume = YG2.saves.soundOn ? 1 : 0;
        moneyText.text = MoneyFormat(YG2.saves.cash);
    }

    private void NoPurchase(string id)
    {
        AudioListener.volume = YG2.saves.soundOn ? 1 : 0;
    }

}
